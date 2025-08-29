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
    public class ManagementService : IManagementService
    {
        private readonly IFileService fileService;

        private readonly ReverseProxyDbContext dbContext;

        private readonly IMapper mapper;

        private readonly IProcessService processService;

        public ManagementService(IFileService fileService, ReverseProxyDbContext dbContext,
            IMapper mapper, IProcessService processService)
        {
            this.fileService = fileService;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.processService = processService;
        }

        public async Task AddNewServerAsync(EditServerRequest request)
        {
            var exisitingServer = this.dbContext.Servers.FirstOrDefault(x => x.Name == request.Name);

            if(exisitingServer != null)
            {
                throw new AlreadyExistsException($"Server with name {exisitingServer.Name} already exists!");
            }

            CertificateEntity? certificate = null;

            if (request.CertificateId > 0)
            {
                certificate = this.dbContext.Certificates.FirstOrDefault(x => x.Id == request.CertificateId);
            }

            if (request.CertificateId > 0 && certificate == null)
            {
                throw new NotFoundException($"Certificate with id {request.CertificateId} not found");
            }

            if (request.CertificateId > 0 && certificate.ServerId > 0)
            {
                throw new AlreadyExistsException($"Certificate with id {request.CertificateId} is already assigned to another server.");
            }

            // TODO: Check in validation if:
            // http and httpredirect are not both true
            // https true and certificate is not null

            // Attention the new server is not in the nginx config yet
            // The server is only in the config if IsUpToDate is true
            var serverEntity = new ServerEntity
            {
                Name = request.Name,
                UsesHttp = request.UsesHttp,
                RedirectsToHttps = request.RedirectsToHttps,
                Certificate = certificate,
                RawSettings = request.RawSettings,
                Active = request.Active,
                IsUpToDate = false,
                Target = request.Target,
                TargetPort = request.TargetPort
            };

            this.dbContext.Servers.Add(serverEntity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task ApplyNewConfigAsync()
        {
            var servers = this.dbContext.Servers.Include(x => x.Certificate).Where(x => x.Active);
            foreach (var server in servers)
            {
                server.IsUpToDate = true;
            }

            await this.dbContext.SaveChangesAsync();
            await this.fileService.CreateNginxConfigAsync(servers.ToList());
            await this.processService.RestartNginxServer();
        }

        public async Task DeleteServerAsync(int id)
        {
            var exisitingServer = this.dbContext.Servers
                .Include(x => x.Certificate)
                .FirstOrDefault(x => x.Id == id);

            if (exisitingServer == null)
            {
                throw new AlreadyExistsException($"Server with name {exisitingServer.Name} doesnt exists!");
            }

            exisitingServer.Certificate = null;
            await this.dbContext.SaveChangesAsync();

            this.dbContext.Servers.Remove(exisitingServer);
            await this.dbContext.SaveChangesAsync();

            await this.ApplyNewConfigAsync();
        }

        public async Task<List<ServerDto>> GetServerEntitiesAsync(string filter, string sortAfter, bool asc)
        {
            var servers = this.dbContext.Servers.Include(x => x.Certificate).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                servers = servers.Where(x => x.Name.ToLower().Contains(filter.ToLower()));
            }

            switch (sortAfter)
            {
                case nameof(SortServerListEnum.name):
                    servers = asc ? servers.OrderBy(x => x.Name) : servers.OrderByDescending(x => x.Name);
                    break;
                case nameof(SortServerListEnum.lastUpdated):
                    servers = asc ? servers.OrderBy(x => x.LastUpdated) : servers.OrderByDescending(x => x.LastUpdated);
                    break;
                case nameof(SortServerListEnum.active):
                    servers = asc ? servers.OrderBy(x => x.Active) : servers.OrderByDescending(x => x.Active);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Sort with name ${sortAfter} not found.");
            }

            return this.mapper.Map<List<ServerDto>>(servers.ToList());
        }

        public async Task UpdateServerAsync(int id, EditServerRequest request)
        {
            var exisitingServer = this.dbContext.Servers.FirstOrDefault(x => x.Id == id);

            if (exisitingServer == null)
            {
                throw new AlreadyExistsException($"Server with name {id} doesnt exists!");
            }

            CertificateEntity? certificate = null;

            if (request.CertificateId > 0)
            {
                certificate = this.dbContext.Certificates.FirstOrDefault(x => x.Id == request.CertificateId);
            }

            if (request.CertificateId > 0 && certificate == null)
            {
                throw new NotFoundException($"Certificate with id {request.CertificateId} not found");
            }

            exisitingServer.Name = request.Name;
            exisitingServer.UsesHttp = request.UsesHttp;
            exisitingServer.RedirectsToHttps = request.RedirectsToHttps;
            exisitingServer.Active = request.Active;
            exisitingServer.RawSettings = request.RawSettings;
            exisitingServer.Target = request.Target;
            exisitingServer.TargetPort = request.TargetPort;
            exisitingServer.IsUpToDate = false;
            exisitingServer.Certificate = certificate;
            exisitingServer.LastUpdated = DateTime.Now;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
