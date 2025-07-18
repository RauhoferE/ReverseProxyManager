using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ReverseProxyManager.DTOs
{
    public class CookieDto
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }

        public AuthenticationProperties AuthenticationProperties { get; set; }
    }
}
