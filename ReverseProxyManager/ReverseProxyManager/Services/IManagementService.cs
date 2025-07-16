using Core.Entities;

namespace ReverseProxyManager.Services
{
    public interface IManagementService
    {
        Task AddNewServerAsync();

        Task DeleteServerAsync();

        Task UpdateServerAsync();

        Task<List<ServerEntity>> GetServerEntitiesAsync();

        Task<ServerEntity> GetServerEntityAsync();

        // This generates the default.conf for the nginx server and restarts it
        Task ApplyNewConfigAsync();
    }
}
