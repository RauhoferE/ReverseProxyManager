using Core.Entities;

namespace ReverseProxyManager.Services
{
    public interface ICertificationService
    {
        // Adds the certificate to the db
        Task AddNewCertificateAsync();
        Task DeleteCertificateAsync(int id);
        Task UpdateCertificateAsync(int id);
        Task<List<CertificateEntity>> GetAllCertificatesAsync();
        Task<CertificateEntity> GetCertificateByIdAsync(int id);
    }
}
