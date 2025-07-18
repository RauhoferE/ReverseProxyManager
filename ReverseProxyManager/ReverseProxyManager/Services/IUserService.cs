using ReverseProxyManager.DTOs;

namespace ReverseProxyManager.Services
{
    public interface IUserService
    {
        Task<CookieDto> Authenticate(string username, string password);
    }
}
