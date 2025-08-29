namespace ReverseProxyManager
{
    public static class ApiRoutes
    {
        private const string Api = "api";

        private const string Version = "v1";

        private const string Controller = "[controller]";

        public const string Base = Api + "/" + Version + "/" + Controller;

        public static class Auth
        {
            public const string Login = "login";
            public const string Logout = "logout";
        }

        public static class Certification
        {
            public const string RescanCertificates = "rescan";

            public const string DeleteAndUpdateCertificate = "{id:int}";

            public const string GetAllCertificates = "";

            public const string GetActiveCertificates = "active";
        }

        public static class Management
        {
            public const string GetServers = "";
            public const string UpdateAndDeleteServer = "{id:int}";
            public const string ApplyConfig = "apply-config";
            public const string RestartService = "restart";

        }
    }
}
