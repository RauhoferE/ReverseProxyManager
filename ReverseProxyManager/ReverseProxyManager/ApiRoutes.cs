namespace ReverseProxyManager
{
    public static class ApiRoutes
    {
        private const string Api = "api";

        private const string Version = "v1";

        private const string Controller = "[controller]";

        public static string Base = Api + "/" + Version + "/" + Controller;

        public static class Auth
        {

        }

        public static class Certification
        {
            public const string RescanCertificates = "rescan";

            public const string DeleteCertificate = "{id:int}";

            public const string UpdateCertificateName = "{id:int}/update-name";
        }

        public static class Management
        {

        }
    }
}
