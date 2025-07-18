using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Requests;
using ReverseProxyManager.Services;

namespace ReverseProxyManager.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Base)]
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

        [HttpDelete(ApiRoutes.Certification.DeleteAndUpdateCertificate)]
        public async Task<IActionResult> DeleteCertificate([FromRoute] int id)
        {
            await this.certificationService.DeleteCertificateAsync(id);
            return Ok();
        }

        [HttpPut(ApiRoutes.Certification.DeleteAndUpdateCertificate)]
        public async Task<IActionResult> UpdateCertificateName([FromRoute] int id, [FromBody] UpdateCertificateNameRequest request)
        {
            await this.certificationService.UpdateCertificateNameAsync(id, request.Name);
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
