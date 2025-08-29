
using Serilog;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReverseProxyManager.Services
{
    public class ProcessService : IProcessService
    {
        public async Task<bool> RestartNginxServer()
        {
            // No nginx on windows
            if (!OperatingSystem.IsLinux())
            {
                return true;
            }

            try
            {
                // Create a new ProcessStartInfo object
                var psi = new ProcessStartInfo
                {
                    // The filename is the shell (bash)
                    FileName = "/bin/bash",
                    // The arguments are the command to be executed with the '-c' flag
                    // which tells bash to execute a string as a command.
                    Arguments = $"-c service nginx restart",
                    // Redirect standard output and error to capture any messages
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    // Do not use the operating system shell to start the process
                    UseShellExecute = false,
                    // Do not show the window where the process is running
                    CreateNoWindow = true
                };

                // Start the process
                using (Process process = Process.Start(psi))
                {
                    // Read the output and error streams
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Wait for the process to exit
                    process.WaitForExit();

                    // Check the exit code to determine if the command was successful
                    if (process.ExitCode == 0)
                    {
                        Log.Information("Successfully restarted nginx");
                        return true;
                    }

                    Log.Error("Failed to restart nginx");
                    Log.Error(error);
                    return false;
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to restart nginx");
                Log.Error($"An error occurred while trying to restart the service: {ex.Message}");
            }

            return false;
        }
    }
}
