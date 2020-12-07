using System;
using System.IO;
using LibGit2Sharp;
using Pit.Logs;
using Pit.Process;

namespace Pit.Git
{
    public static class GitUtils
    {
        private static readonly Logger Log = new Logger("GitUtils");

        public static void CheckIfRepository()
        {
            try
            {
                using Repository repo = new Repository(Environment.CurrentDirectory);
            }
            catch (Exception e)
            {
                if (e is RepositoryNotFoundException)
                {
                    Log.Error(
                        "Not a Git repository!",
                        "Please navigate to a repository folder"
                    );
                    Environment.Exit(1);
                }
            }
        }

        public static void CheckOutBranch(Repository repo, Branch branch)
        {
            string currentPackageJson = ReadPackageJson();
            Commands.Checkout(repo, branch);
            Log.Green($"Changed to branch {branch.FriendlyName}");
            ComparePackageJsons(currentPackageJson);
        }

        public static void CheckOutBranch(string branchName)
        {
            string currentPackageJson = ReadPackageJson();
            ProcessRunner runner = new ProcessRunner();
            string coOutput = runner.RunWithDefault($"git checkout {branchName} 2>&1");
            CheckIfError(coOutput, "checkout branch");
            Log.Green($"Changed to branch {branchName}");
            Console.WriteLine(coOutput);
            ComparePackageJsons(currentPackageJson);
        }

        public static void CreateBranch(string branchName)
        {
            Log.Blue($"Creating branch...");
            ProcessRunner runner = new ProcessRunner();
            string coOutput = runner.RunWithDefault($"git checkout -b {branchName} 2>&1");
            CheckIfError(coOutput, "create branch");
            Console.WriteLine(coOutput);
        }

        private static string ReadPackageJson()
        {
            string pathToPackageJson = Path.Combine(Environment.CurrentDirectory, "package.json");
            return File.Exists(pathToPackageJson)
                ? File.ReadAllText(pathToPackageJson)
                : null;
        }

        private static void ComparePackageJsons(string previousPackageJson)
        {
            if (string.IsNullOrWhiteSpace(previousPackageJson)) return;
            string currentPackageJson = ReadPackageJson();
            if (
                string.IsNullOrWhiteSpace(currentPackageJson) ||
                string.Equals(previousPackageJson, currentPackageJson)
            ) return;
            Console.WriteLine();
            Log.Info("Package.json has changed!", "Don't forget to run npm ci...");
        }

        public static void ShowLatestCommit(Repository repo)
        {
            Commit latest = repo.Head.Tip;
            string sha = latest.Id.Sha.Substring(0, 7);
            string message = latest.MessageShort;
            string author = latest.Author.Name;
            Console.WriteLine();
            Log.Blue($"Latest commit: ({sha}) \"{message}\" by {author}");
        }

        public static void PerformFetch()
        {
            ProcessRunner runner = new ProcessRunner();
            Log.Blue("Fetching from remote 'origin'...\n");
            string fetchOutput = runner.RunWithDefault("git fetch 2>&1");
            CheckIfError(fetchOutput, "fetch");
            Console.WriteLine(fetchOutput);
        }

        public static void PerformPull(string branchName)
        {
            ProcessRunner runner = new ProcessRunner();
            Log.Blue($"Pulling from remote 'origin {branchName}'...\n");
            string pullOutput = runner.RunWithDefault($"git pull origin {branchName} 2>&1");
            CheckIfError(pullOutput, "pull");
            Console.WriteLine(pullOutput);
        }

        private static void CheckIfError(string output, string action)
        {
            if (output.Contains("fatal: Could not") || output.Contains("fatal: couldn't"))
            {
                Log.Error($"Couldn't {action}.", output);
                Environment.Exit(1);
            }
        }
    }
}