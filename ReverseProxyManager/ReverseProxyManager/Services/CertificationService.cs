using System.Linq;
using AutoMapper;
using Azure.Core;
using Core.Entities;
using Core.Enums;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using ReverseProxyManager.DTOs;
using ReverseProxyManager.Exceptions;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Services
{
    public class CertificationService : ICertificationService
    {
        private readonly ReverseProxyDbContext _dbContext;

        private readonly IFileService fileService;

        private readonly IMapper mapper;

        private readonly IManagementService managementService;

        public CertificationService(ReverseProxyDbContext dbContext, IFileService fileService, IMapper mapper, 
            IManagementService managementService)
        {
            this._dbContext = dbContext;
            this.fileService = fileService;
            this.mapper = mapper;
            this.managementService = managementService;
        }

        // This method adds a new certificate to the database.
        // This is never called via the controller but only via the file service when a new certificate is found in the file system.
        public async Task AddNewCertificateAsync(CreateCertificateRequest request)
        {
            var existingCertificate = this._dbContext.Certificates.FirstOrDefault(x => x.Name == request.Name);
            
            if (existingCertificate != null)
            {
                throw new AlreadyExistsException($"Certificate with name {request.Name} already exists!");
            }

            var newCertificate = new CertificateEntity
            {
                Name = request.Name,
                Issuer = request.Issuer,
                Subject = request.Subject,
                ValidNotAfter = request.ValidNotAfter,
                ValidNotBefore = request.ValidNotBefore,
                FileAttached = true,
            };

            this._dbContext.Add(newCertificate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteCertificateAsync(int id)
        {
            // Immediatly create new config if the certificate had been in use
            var existingCertificate = this._dbContext.Certificates
                .Include(x => x.ServerEntity)
                .FirstOrDefault(x => x.Id == id);

            if (existingCertificate == null)
            {
                throw new NotFoundException($"Certificate with id {id} not found.");
            }

            bool recreateConfig = false;
            // If there is a server associated with the certificate deactivate it because the ssl file is not found anymore
            // Also receraete the config
            if (existingCertificate.ServerEntity != null)
            {
                existingCertificate.ServerEntity.IsUpToDate = false;
                existingCertificate.ServerEntity.LastUpdated = DateTime.Now;
                existingCertificate.ServerEntity.Active = false;
                existingCertificate.ServerEntity.RedirectsToHttps = false;
                recreateConfig = true;
            }

            await this.fileService.DeleteSSlCertificateAsync(existingCertificate.Name);

            this._dbContext.Certificates.Remove(existingCertificate);
            await this._dbContext.SaveChangesAsync();

            if (recreateConfig)
            {
                await this.managementService.ApplyNewConfigAsync();
            }
        }

        public async Task<List<CertificateDto>> GetAllCertificatesAsync(string filter, string sortAfter, bool asc)
        {
            var certificates = this._dbContext.Certificates.Include(x => x.ServerEntity).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                certificates = certificates.Where(x => x.Name.ToLower().Contains(filter.ToLower()));
            }

            switch (sortAfter)
            {
                case nameof(SortCertificateListEnum.name):
                    certificates = asc ? certificates.OrderBy(x => x.Name) : certificates.OrderByDescending(x => x.Name);
                    break;
                case nameof(SortCertificateListEnum.issuer):
                    certificates = asc ? certificates.OrderBy(x => x.Issuer) : certificates.OrderByDescending(x => x.Issuer);
                    break;
                case nameof(SortCertificateListEnum.dateUpdated):
                    certificates = asc ? certificates.OrderBy(x => x.LastUpdated) : certificates.OrderByDescending(x => x.LastUpdated);
                    break;
                case nameof(SortCertificateListEnum.validNotAfter):
                    certificates = asc ? certificates.OrderBy(x => x.ValidNotAfter) : certificates.OrderByDescending(x => x.ValidNotAfter);
                    break;
                case nameof(SortCertificateListEnum.validNotBefore):
                    certificates = asc ? certificates.OrderBy(x => x.ValidNotBefore) : certificates.OrderByDescending(x => x.ValidNotBefore);
                    break;
                case nameof(SortCertificateListEnum.fileAttached):
                    certificates = asc ? certificates.OrderBy(x => x.FileAttached) : certificates.OrderByDescending(x => x.FileAttached);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Sort with name ${sortAfter} not found.");
            }

            return this.mapper.Map<List<CertificateDto>>(certificates.ToList());
        }

        public async Task<List<IdNameDto>> GetActiveCertificatesShortAsync()
        {
            return this.mapper.Map<List<IdNameDto>>(this._dbContext.Certificates.Where(x => x.FileAttached).ToList());
        }

        public async Task ImportSSlCertificates()
        {
            var certificates = await this.fileService.GetSSlCertificatesAsync();

            var existingCertificates = this._dbContext.Certificates
                .Include(x => x.ServerEntity).ToList();

            foreach (var certificate in certificates)
            {
                var existingCertificate = this._dbContext.Certificates.FirstOrDefault(x => x.Name == certificate.Name);
                
                if (existingCertificate != null)
                {
                    await this.UpdateCertificateAsync(existingCertificate.Id, new EditCertificateRequest()
                    {
                        Issuer = certificate.Issuer,
                        Subject = certificate.Subject,
                        ValidNotAfter = certificate.ValidNotAfter,
                        ValidNotBefore = certificate.ValidNotBefore,
                        // In case the previous certificate file was not found and the certificate was disabled
                        // Here we know that the file exists so set the boolean to true
                        FileAttached = true
                    });
                    continue; 
                }

                await this.AddNewCertificateAsync(certificate);
            }

            await this._dbContext.SaveChangesAsync();
            
            foreach (var certificate in existingCertificates)
            {
                // If the file exists skip
                if (certificates.Select(x => x.Name).Contains(certificate.Name))
                {
                    continue;
                }

                // If there is no file mark it and deactivate the server if it has any
                certificate.FileAttached = false;
                this.ResetCertificateEntity(certificate);

                if (certificate.ServerEntity != null)
                {
                    certificate.ServerEntity.Active = false;
                    certificate.ServerEntity.IsUpToDate = false;
                    certificate.ServerEntity.LastUpdated = DateTime.Now;
                    certificate.ServerEntity.RedirectsToHttps = false;
                }
            }

            await this._dbContext.SaveChangesAsync();

            await this.managementService.ApplyNewConfigAsync();
        }

        // This method updates a certificate in the database.
        // This is never called via the controller but only via the file service when a updated certificate is found in the file system.
        public async Task UpdateCertificateAsync(int id, EditCertificateRequest request)
        {
            var certificate = this._dbContext.Certificates.FirstOrDefault(x => x.Id == id);

            if (certificate == null)
            {
                throw new NotFoundException($"Certificate with id {id} not found.");
            }

            certificate.Issuer = request.Issuer;
            certificate.Subject = request.Subject;  
            certificate.ValidNotBefore = request.ValidNotBefore;
            certificate.ValidNotAfter = request.ValidNotAfter;
            certificate.LastUpdated = DateTime.Now;
            certificate.FileAttached = request.FileAttached;

            await this._dbContext.SaveChangesAsync();
        }


        public async Task UpdateCertificateNameAsync(int id, string name)
        {
            var certificate = this._dbContext.Certificates
                .Include(x => x.ServerEntity).FirstOrDefault(x => x.Id == id);

            var certificateWithSameName = this._dbContext.Certificates.FirstOrDefault(x => x.Name == name);

            if (certificate == null)
            {
                throw new NotFoundException($"Certificate with id {id} not found.");
            }

            if (certificateWithSameName != null)
            {
                throw new AlreadyExistsException($"Certificate with name {name} already exists.");
            }

            certificate.Name = name;

            var certificateFiles = await this.fileService.GetSSlCertificateNamesAsync();

            // If there is no file attached set t to inactive and remove all the data
            var fileAttached = await this.fileService.CheckForValidFilesAsync(name);
            if (!fileAttached)
            {
                certificate.FileAttached = false;
                this.ResetCertificateEntity(certificate);
                //certificate.LastUpdated = DateTime.Now;
                //certificate.IsUpToDate = false;
                //certificate.Issuer = null;
                //certificate.Subject = null;
                //certificate.ValidNotAfter = null;
                //certificate.ValidNotBefore = null;
            }

            if (!fileAttached && certificate.ServerEntity != null)
            {
                certificate.ServerEntity.Active = false;
                certificate.ServerEntity.LastUpdated = DateTime.Now;
                certificate.ServerEntity.IsUpToDate = false;
                certificate.ServerEntity.RedirectsToHttps = false;
            }

            await this.managementService.ApplyNewConfigAsync();
        }

        private void ResetCertificateEntity(CertificateEntity certificateEntity)
        {
            certificateEntity.LastUpdated = DateTime.Now;
            certificateEntity.IsUpToDate = false;
            certificateEntity.Issuer = null;
            certificateEntity.Subject = null;
            certificateEntity.ValidNotAfter = null;
            certificateEntity.ValidNotBefore = null;
        }
    }
}
