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

        public async Task CreateNginxConfigAsync(List<ServerEntity> serverEntities)
        {
            var configTxt = "";

            // Read the server config
            foreach (var server in serverEntities)
            {
                // If the user added his own settings write only them into the config
                if (server.RawSettings != null)
                {
                    configTxt = configTxt + server.RawSettings;
                    continue;
                }

                // Incase the user wants http to
                var httpString = server.UsesHttp ? "listen 80;" : string.Empty;    
                // https specific settings
                var httpsString = server.Certificate != null ? "listen 443 ssl http2;" : string.Empty;
                var sslCert = server.Certificate != null ? $"ssl_certificate {Path.Combine(SSL_FOLDER, server.Certificate.Name)}.crt;" : string.Empty;
                var sslKey = server.Certificate != null ? $"ssl_certificate_key {Path.Combine(SSL_FOLDER, server.Certificate.Name)}.key;" : string.Empty;
                var httpRedirectionString = server.RedirectsToHttps ? $@"
# Redirect {server.Name} to https
server {{
    listen 80;
    server_name {server.Name};

    
    return 301 https://{server.Name}$request_uri;
}}
" : string.Empty;

                configTxt = configTxt + $@"
## {server.Name}
server {{
    {httpsString}
    {httpsString}
    server_name {server.Name};
    {sslCert}
    {sslKey}
    include /etc/nginx/includes/ssl.conf;
    location / {{
        include /etc/nginx/includes/proxy.conf;
        proxy_pass {server.Target}:{server.TargetPort};
    }}
    access_log off;
    error_log /var/log/nginx/error.log error;

}}
{httpRedirectionString}
";
            }

            File.WriteAllText(Path.Combine(NGINX_FOLDER, "default.conf"), configTxt);
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
