using Core.Entities;
using ReverseProxyManager.DTOs;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Services
{
    public interface IManagementService
    {
        Task AddNewServerAsync(CreateServerRequest request);

        Task DeleteServerAsync(int id);

        Task UpdateServerAsync(int id, EditServerRequest request);

        Task<List<ServerDto>> GetServerEntitiesAsync(string filter, string sortAfter, bool asc);

        // This generates the default.conf for the nginx server and restarts it
        Task ApplyNewConfigAsync();
    }
}
