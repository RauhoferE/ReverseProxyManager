using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Requests;
using ReverseProxyManager.Services;

namespace ReverseProxyManager.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IManagementService managementService;


        public ManagementController(IManagementService managementService)
        {
            this.managementService = managementService;
        }


        public async Task<IActionResult> GetServers([FromQuery] string filter, [FromQuery] string sortAfter, [FromQuery] bool asc)
        {
            var results = await this.managementService.GetServerEntitiesAsync(filter, sortAfter, asc);
            return Ok(results);
        }

        public async Task<IActionResult> AddNewServer([FromBody] CreateServerRequest request)
        {
            await this.managementService.AddNewServerAsync(request);
            return Ok();
        }

        public async Task<IActionResult> UpdateServer([FromRoute] int id, [FromBody] EditServerRequest request)
        {
            await this.managementService.UpdateServerAsync(id, request);
            return Ok();
        }

        public async Task<IActionResult> DeleteServer([FromRoute] int id)
        {
            await this.managementService.DeleteServerAsync(id);
            return Ok();
        }

        public async Task<IActionResult> ApplyNewConfig()
        {
            await this.managementService.ApplyNewConfigAsync();
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
