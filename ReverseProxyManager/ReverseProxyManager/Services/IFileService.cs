using Core.Entities;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Services
{
    public interface IFileService
    {
        Task<List<CreateCertificateRequest>> GetSSlCertificatesAsync();

        Task<List<string>> GetSSlCertificateNamesAsync();

        Task<bool> CheckForKeyFile(string fileName);

        Task DeleteSSlCertificateAsync(string name);

        Task CreateNginxConfigAsync(List<ServerEntity> serverEntities);
    }
}
