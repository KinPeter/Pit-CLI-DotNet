using System;
using System.IO;
using System.Linq;
using Pit.Help;
using Pit.Process;
using Pit.Types;

namespace Pit.Debug
{
    public class RepoDebugger: PitAction
    {
        public RepoDebugger(string[] args) : base("RepoDebugger", args) {}
        
        public override void Run()
        {
            if (Args.Length == 0) HandleMissingParams();

            if (Args[0] == "-h" || Args[0] == "--help")
            {
                ShowHelp();
                return;
            }

            if (Args[0] == "create")
            {
                CreateDebugRepo();
                return;
            }

            if (Args[0] == "delete" || Args[0] == "remove")
            {
                DeleteDebugRepo();
                return;
            }

            if (Args[0] == "reset")
            {
                DeleteDebugRepo();
                CreateDebugRepo();
                return;
            }
            
            HandleUnknownParam();
        }

        public override void ShowHelp()
        {
            Log.Blue("Usage:");
            Console.WriteLine(HelpText.RepoDebugger);
        }
        
        private void CreateDebugRepo()
        {
            string repoPath = $"{Environment.CurrentDirectory}\\PitDebug";
            Log.Info($"Creating debug repo under {repoPath}\\...");
            if (Directory.Exists(repoPath))
            {
                Log.Error("Directory already exists.", "Use the \"reset\" or \"delete\" action first.");
                Environment.Exit(1);
            }
            Directory.CreateDirectory(repoPath);
            Directory.SetCurrentDirectory(repoPath);
            ProcessRunner runner = new ProcessRunner();
            runner.RunWithDefault("git init");
            runner.RunWithDefault("echo something > file1.txt");
            runner.RunWithDefault("git add --all && git commit -m \\\"Initial commit\\\"");
            Log.Green("Created first commit");
            /* Some git commands returns the success response to StdErr, redirection is required */
            runner.RunWithDefault("git checkout -b debug/PIT-111-add-some-files 2>&1");
            runner.RunWithDefault("echo something > file2.txt");
            runner.RunWithDefault("git add --all && git commit -m \\\"PIT-111: Add some files\\\"");
            Log.Green("Created second commit and branch");
            runner.RunWithDefault("git checkout master 2>&1");
            runner.RunWithDefault("git checkout -b debug/PIT-112-add-more-files 2>&1");
            runner.RunWithDefault("echo something > file3.txt");
            runner.RunWithDefault("git add --all && git commit -m \\\"PIT-112: Add more files\\\"");
            Log.Green("Created third commit and branch");
            runner.RunWithDefault("git checkout master 2>&1");
            runner.RunWithDefault("git merge debug/PIT-111-add-some-files");
            runner.RunWithDefault("git checkout -b debug/PIT-113-modify-a-file 2>&1");
            runner.RunWithDefault("echo something >> file2.txt");
            runner.RunWithDefault("git add --all && git commit -m \\\"PIT-113: Modify a file\\\"");
            Log.Green("Created fourth commit and branch");
            runner.RunWithDefault("git checkout master 2>&1");
            runner.RunWithDefault("git merge debug/PIT-113-modify-a-file");
            runner.RunWithDefault("git checkout -b debug/PIT-114-add-a-file 2>&1");
            runner.RunWithDefault("echo something > file4.txt");
            runner.RunWithDefault("git add --all && git commit -m \\\"PIT-114: Add a file\\\"");
            Log.Green("Created fifth commit and branch");
            runner.RunWithDefault("git checkout master 2>&1");
            Log.Green("Done.");
        }

        private void DeleteDebugRepo()
        {
            string repoPath = $"{Environment.CurrentDirectory}\\PitDebug";
            if (!Directory.Exists(repoPath))
            {
                Log.Error("Directory does not exist.");
                Environment.Exit(1);
            }
            
            Log.Info("Delete this directory?");
            Log.Blue(repoPath);
            Log.Blue("(Y/n)");
            string input = Console.ReadLine();
            string[] positive = {"y", "Y", ""};
            if (!positive.Contains(input)) Environment.Exit(0);

            ProcessRunner runner = new ProcessRunner();
            runner.RunWithDefault($"Remove-Item -Recurse -Force {repoPath}");

            Log.Green("Done.");
        }
    }
}