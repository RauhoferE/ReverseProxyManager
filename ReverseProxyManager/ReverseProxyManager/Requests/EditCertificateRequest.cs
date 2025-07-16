namespace ReverseProxyManager.Requests
{
    public class EditCertificateRequest
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }

        public DateTime ValidNotBefore { get; set; }

        public DateTime ValidNotAfter { get; set; }
    }
}
