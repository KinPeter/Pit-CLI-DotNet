using System.Diagnostics;
using Pit.Logs;

namespace Pit.Process
{
    public class ProcessRunner
    {
        private readonly Logger log;
        private readonly ProcessStartInfo startInfo;
        private string lastCommand;
        private string error;
        private string output;
        
        public ProcessRunner()
        {
            startInfo = new ProcessStartInfo
            {
                FileName = "pwsh.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            log = new Logger(GetType().Name);
        }

        public string Run(string command)
        {
            lastCommand = command;

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.StartInfo.Arguments = $"/C {command}";
                process.Start();
                process.WaitForExit();
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }

            if (error.Length > 0)
            {
                throw new CustomProcessException(error);
            }

            return output;
        }

        public void HandleError(string message)
        {
            log.Error($"An error occured during the command: {lastCommand}", message);
        }
    }
}