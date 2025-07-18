using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Services;

namespace ReverseProxyManager.Controllers
{
    public class CertificateController : Controller
    {
        private readonly ICertificationService certificationService;

        public CertificateController(ICertificationService certificationService)
        {
            this.certificationService = certificationService;
        }

        [HttpGet(ApiRoutes.Certification.RescanCertificates)]
        public async Task<IActionResult> RescanCertificates()
        {
            await this.certificationService.ImportSSlCertificates();
            return Ok();
        }

        [HttpDelete(ApiRoutes.Certification.DeleteCertificate)]
        public async Task<IActionResult> DeleteCertificate([FromRoute] int id)
        {
            await this.certificationService.DeleteCertificateAsync(id);
            return Ok();
        }

        [HttpPut(ApiRoutes.Certification.UpdateCertificateName)]
        public async Task<IActionResult> UpdateCertificateName([FromRoute] int id, [FromQuery] string name)
        {
            await this.certificationService.UpdateCertificateNameAsync(id, name);
            return Ok();
        }

        [HttpGet(ApiRoutes.Certification.GetAllCertificates)]
        public async Task<IActionResult> GetAllCertificates([FromQuery] string? filter, [FromQuery] string sortAfter, [FromQuery] bool asc)
        {
            var results = await this.certificationService.GetAllCertificatesAsync(filter, sortAfter, asc);
            return Ok(results);
        }

        [HttpGet(ApiRoutes.Certification.GetActiveCertificates)]
        public async Task<IActionResult> GetActiveCertiicates()
        {
            var results = await this.certificationService.GetActiveCertificatesShortAsync();
            return Ok(results);
        }
    }
}
