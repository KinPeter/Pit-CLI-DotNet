using System;
using System.Diagnostics;
using System.IO;
using Pit.Logs;
using Pit.OS;

namespace Pit.Process
{
    public class ProcessRunner
    {
        private readonly Logger log;
        private readonly Os os;
        private readonly ProcessStartInfo startInfo;
        private string lastCommand;
        private string error;
        private string output;

        public ProcessRunner()
        {
            os = new Os();
            startInfo = new ProcessStartInfo
            {
                FileName = os.IsLinux() ? "/bin/bash" : "pwsh.exe",
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
            output = null;
            lastCommand = command;

            string escaped = command.Replace("\"", "\\\"");

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = startInfo;
                process.StartInfo.Arguments = os.IsLinux() ? $"-c \"{escaped}\"" : $"/C {command}";
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

        public string RunWithDefault(string command)
        {
            output = null;
            try
            {
                output = Run(command);
            }
            catch (Exception e)
            {
                if (e is CustomProcessException)
                {
                    HandleError(e.Message);
                    Environment.Exit(1);
                }

                throw;
            }

            return output;
        }

        public void RunMultiple(string[] commands)
        {
            if (os.IsLinux())
            {
                foreach (string command in commands)
                {
                    RunWithDefault(command);
                }

                return;
            }

            string batFileName = "";

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                batFileName = path + Guid.NewGuid() + ".bat";
                using (StreamWriter batFile = new StreamWriter(batFileName))
                {
                    foreach (string command in commands)
                    {
                        batFile.WriteLine(command);
                    }
                }

                RunWithDefault(batFileName);
            }
            finally
            {
                if (File.Exists(batFileName)) File.Delete(batFileName);
            }
        }

        public void HandleError(string message)
        {
            log.Error($"An error occured during the command: {lastCommand}", message);
        }
    }
}