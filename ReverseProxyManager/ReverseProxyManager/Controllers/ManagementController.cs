using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Services;

namespace ReverseProxyManager.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IManagementService managementService;


        public ManagementController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
