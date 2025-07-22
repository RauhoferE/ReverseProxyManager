using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Requests;
using ReverseProxyManager.Services;

namespace ReverseProxyManager.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Base)]
    [Authorize()]
    public class ManagementController : Controller
    {
        private readonly IManagementService managementService;


        public ManagementController(IManagementService managementService)
        {
            this.managementService = managementService;
        }

        [HttpGet(ApiRoutes.Management.GetServers)]
        public async Task<IActionResult> GetServers([FromQuery] string? filter, [FromQuery] string sortAfter, [FromQuery] bool asc)
        {
            var results = await this.managementService.GetServerEntitiesAsync(filter, sortAfter, asc);
            return Ok(results);
        }

        [HttpPost(ApiRoutes.Management.GetServers)]
        public async Task<IActionResult> AddNewServer([FromBody] EditServerRequest request)
        {
            await this.managementService.AddNewServerAsync(request);
            return Ok();
        }

        [HttpPut(ApiRoutes.Management.UpdateAndDeleteServer)]
        public async Task<IActionResult> UpdateServer([FromRoute] int id, [FromBody] EditServerRequest request)
        {
            await this.managementService.UpdateServerAsync(id, request);
            return Ok();
        }

        [HttpDelete(ApiRoutes.Management.UpdateAndDeleteServer)]
        public async Task<IActionResult> DeleteServer([FromRoute] int id)
        {
            await this.managementService.DeleteServerAsync(id);
            return Ok();
        }

        [HttpGet(ApiRoutes.Management.ApplyConfig)]
        public async Task<IActionResult> ApplyNewConfig()
        {
            await this.managementService.ApplyNewConfigAsync();
            return Ok();
        }
    }
}
