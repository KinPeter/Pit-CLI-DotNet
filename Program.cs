using System;
using System.Diagnostics;

namespace pit
{
    class Program
    {
        static void Main(string[] args)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "pwsh.exe",
                    Arguments = "/C git branch && git remote -v && git co update-to-angular10",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            var lines = output.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"{i}, {lines[i]}");
            }


        }
    }
}