namespace ReverseProxyManager.Services
{
    public interface IUserService
    {
        Task Authenticate(string username, string password);

        Task Logout();
    }
}
