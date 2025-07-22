
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using ReverseProxyManager.DTOs;
using ReverseProxyManager.Settings;

namespace ReverseProxyManager.Services
{
    public class UserService : IUserService
    {
        private readonly UserSettings userSettings;

        public UserService(UserSettings userSettings)
        {
            this.userSettings = userSettings;
        }

        public async Task<CookieDto> Authenticate(string username, string password)
        {
            if (this.userSettings.Username == username && this.userSettings.Password == password)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return new CookieDto()
                {
                    ClaimsIdentity = claimsIdentity,
                    AuthenticationProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IssuedUtc = DateTime.Now,
                    }
                    
                };
            }
            
            throw new UnauthorizedAccessException("Invalid username or password.");
        }
    }
}
