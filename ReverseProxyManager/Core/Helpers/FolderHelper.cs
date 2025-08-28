using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class FolderHelper
    {
        public static string GetSqliteFilePath()
        {
            if (OperatingSystem.IsWindows())
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite", "reverse_proxy.db");
            }

            if (OperatingSystem.IsLinux())
            {
                // Todo: create folder
                return Path.Combine("/etc/data", "reverse_proxy.db");
            }

            throw new NotSupportedException("Os not supported");
        }

        public static string GetSqliteFolderPath()
        {
            if (OperatingSystem.IsWindows())
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite");
            }

            if (OperatingSystem.IsLinux())
            {
                // Todo: create folder
                return "/etc/data";
            }

            throw new NotSupportedException("Os not supported");
        }

        public static string GetNginxConfigFilePath()
        {
            if (OperatingSystem.IsWindows())
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nginx\\conf.d", "default.conf");
            }
            if (OperatingSystem.IsLinux())
            {
                return Path.Combine("/etc/nginx/conf.d", "default.conf");
            }

            throw new NotSupportedException("Os not supported");
        }

        public static string GetSSlFolderPath()
        {
            if (OperatingSystem.IsWindows())
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ssl");
            }
            if (OperatingSystem.IsLinux())
            {
                return "/etc/ssl/certs/nginx";
            }

            throw new NotSupportedException("Os not supported");
        }

        public static void CreateSystemFolders()
        {
            if (!Directory.Exists(GetSqliteFolderPath()))
            {
                Directory.CreateDirectory(GetSqliteFolderPath());
            }

            if (!Directory.Exists(GetSSlFolderPath()))
            {
                Directory.CreateDirectory(GetSSlFolderPath());
            }

            if (!Directory.Exists(GetNginxConfigFilePath()))
            {
                Directory.CreateDirectory(GetNginxConfigFilePath());
            }

            //if (OperatingSystem.IsWindows())
            //{
            //    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite"));
            //    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nginx\\conf.d"));
            //    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ssl"));
            //    return;
            //}

            //if (OperatingSystem.IsLinux())
            //{
            //    Directory.CreateDirectory("/etc/data");
            //    Directory.CreateDirectory("/etc/nginx/conf.d");
            //    Directory.CreateDirectory("/etc/ssl/certs/nginx");
            //    return;
            //}

            //throw new NotSupportedException("Os not supported");

        }
    }
}
