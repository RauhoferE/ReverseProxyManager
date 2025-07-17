
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

        public async Task Authenticate(string username, string password)
        {
            if (this.userSettings.Username == username && this.userSettings.Password == password;)
            {
                // TOOD: Return auth cookie
            }
            
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
