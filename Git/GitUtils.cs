using System;
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
            Log.Green($"Changing to branch {branch.FriendlyName}");
            Commands.Checkout(repo, branch);
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
            if (fetchOutput.Contains("fatal: Could not"))
            {
                Log.Error("Couldn't fetch.", fetchOutput);
                Environment.Exit(1);
            }
            Console.WriteLine(fetchOutput);
        }
    }
}