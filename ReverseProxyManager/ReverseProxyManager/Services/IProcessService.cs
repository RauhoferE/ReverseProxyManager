namespace ReverseProxyManager.Services
{
    public interface IProcessService
    {
        Task<bool> RestartNginxServer();
    }
}
