using Core.Entities;
using Core.Enums;
using ReverseProxyManager.DTOs;
using ReverseProxyManager.Requests;

namespace ReverseProxyManager.Services
{
    public interface ICertificationService
    {
        // Adds the certificate to the db
        Task AddNewCertificateAsync(CreateCertificateRequest request);

        // This should also delete the file if possible
        Task DeleteCertificateAsync(int id);

        // This is only used when the certificate gets regenerated and rescanned by the file service
        Task UpdateCertificateAsync(int id, EditCertificateRequest request);

        // TODO: Also updates the crt file and server entitfy in the db and also the nginx config.
        Task UpdateCertificateNameAsync(int id, string name);

        Task<List<CertificateDto>> GetAllCertificatesAsync(string filter, string sortAfter, bool asc);

        Task<List<IdNameDto>> GetActiveCertificatesShortAsync();

        Task ImportSSlCertificates();
    }
}
