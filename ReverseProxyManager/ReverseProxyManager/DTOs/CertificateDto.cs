using Core.Entities;

namespace ReverseProxyManager.DTOs
{
    public class CertificateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Issuer { get; set; }

        public string Subject { get; set; }

        public DateTime ValidNotBefore { get; set; }

        public DateTime ValidNotAfter { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // This is used to determine if the nginx config is containing the new information
        public bool IsUpToDate { get; set; } = false;

        public IdNameDto? ServerEntity { get; set; } = null;

    }
}
