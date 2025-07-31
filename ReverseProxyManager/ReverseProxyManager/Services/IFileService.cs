using Core.Entities;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Services
{
    public interface IFileService
    {
        Task<List<CreateCertificateRequest>> GetSSlCertificatesAsync();

        Task<List<string>> GetSSlCertificateNamesAsync();

        Task<bool> CheckForValidFilesAsync(string fileName);

        Task DeleteSSlCertificateAsync(string name);

        Task RenameSSLCertificateAsync(string oldName, string newName);

        Task CreateNginxConfigAsync(List<ServerEntity> serverEntities);
    }
}
