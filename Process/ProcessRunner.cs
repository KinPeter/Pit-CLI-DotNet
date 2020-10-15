using System;
using System.Diagnostics;

namespace pit.Process
{
    public class ProcessRunner
    {
        private readonly ProcessStartInfo startInfo;
        private string error;
        private string output;
        
        public ProcessRunner(string command)
        {
            startInfo = new ProcessStartInfo
            {
                FileName = "pwsh.exe",
                Arguments = $"/C {command}",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
        }

        public string Run()
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }

            if (error.Length > 0)
            {
                throw new Exception(error);
            }

            return output;
        }
    }
}