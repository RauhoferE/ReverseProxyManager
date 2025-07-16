namespace ReverseProxyManager.Services
{
    public interface IAppService
    {
        Task RestartNginxAsync();
    }
}
