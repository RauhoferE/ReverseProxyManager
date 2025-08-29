using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Requests;
using ReverseProxyManager.Services;
using Serilog;

namespace ReverseProxyManager.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Base)]
    [Authorize()]
    public class ManagementController : Controller
    {
        private readonly IManagementService managementService;

        private readonly IProcessService processService;

        public ManagementController(IManagementService managementService, IProcessService processService)
        {
            this.managementService = managementService;
            this.processService = processService;
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

        [HttpGet(ApiRoutes.Management.RestartService)]
        [AllowAnonymous]
        public async Task<IActionResult> RestartNginx()
        {
            var t = await this.processService.RestartNginxServer();

            if (!t.Item1)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
