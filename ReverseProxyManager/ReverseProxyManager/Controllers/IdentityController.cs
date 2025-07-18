
using System.Reflection.Metadata;
using System.Security.Claims;
using Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReverseProxyManager.Requests;
using ReverseProxyManager.Services;
using Microsoft.AspNetCore.Authorization;


namespace ReverseProxyManager.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Base)]
    [AllowAnonymous]
    public class IdentityController : Controller
    {
        private readonly IUserService userService;

        public IdentityController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await this.userService.Authenticate(request.Name, request.Password);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(token.ClaimsIdentity),
                token.AuthenticationProperties);
            return Ok();
        }

        [HttpGet(ApiRoutes.Auth.Logout)]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(
    CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
