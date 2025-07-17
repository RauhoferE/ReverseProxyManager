using System.Security.Cryptography.X509Certificates;
using Core.Entities;
using ReverseProxyManager.Exceptions;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Services
{
    public class FileService : IFileService
    {
        // TODO: Change to correct linux folders for the docker container
        public const string NGINX_FOLDER = "";

        public const string SSL_FOLDER = "";

        public FileService()
        {
                
        }

        public async Task<bool> CheckForValidFilesAsync(string fileName)
        {
            return File.Exists(Path.Combine(SSL_FOLDER, $"{fileName}.crt")) && File.Exists(Path.Combine(SSL_FOLDER, $"{fileName}.key"));
        }

        public Task CreateNginxConfigAsync(List<ServerEntity> serverEntities)
        {
            throw new NotImplementedException();
        }

        // Deletes the certificate and key
        public async Task DeleteSSlCertificateAsync(string name)
        {
            var crtFile = Path.Combine(SSL_FOLDER, $"{name}.crt");
            var keyFile = Path.Combine(SSL_FOLDER, $"{name}.key");
            if (File.Exists(crtFile))
            {
                File.Delete(crtFile);
            }

            if (File.Exists(keyFile))
            {
                File.Delete(keyFile);
            }
        }

        public async Task<List<string>> GetSSlCertificateNamesAsync()
        {
            var fileNames = Directory.GetFiles(SSL_FOLDER, "*.crt");
            return fileNames
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .ToList();
        }

        public async Task<List<CreateCertificateRequest>> GetSSlCertificatesAsync()
        {
            var certificateNames = await GetSSlCertificateNamesAsync();

            List<CreateCertificateRequest> certificates = new List<CreateCertificateRequest>();
            foreach (var certName in certificateNames)
            {
                
                if (!await this.CheckForValidFilesAsync(certName))
                {
                    throw new NotFoundException($"Some files are missing for {certName}. Please ensure that there is a .crt and .key file.");
                }

                var certEntity = X509Certificate2.CreateFromCertFile(Path.Combine(SSL_FOLDER, $"{certName}.crt"));
                certificates.Add(new CreateCertificateRequest()
                {
                    Issuer = certEntity.Issuer,
                    Name = certName,
                    Subject = certEntity.Subject,
                    ValidNotAfter = DateTime.Parse(certEntity.GetEffectiveDateString()),
                    ValidNotBefore = DateTime.Parse(certEntity.GetExpirationDateString()),
                });
            }

            return certificates;
        }
    }
}
