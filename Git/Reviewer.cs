using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Help;
using Pit.Types;
using Pit.UI;

namespace Pit.Git
{
    public class Reviewer : PitAction, IPitActionSync
    {
        public Reviewer(string[] args) : base("Reviewer", args) { }

        public void Run()
        {
            if (Args.Length == 1 && (Args[0] == "-h" || Args[0] == "--help"))
            {
                ShowHelp();
                return;
            }

            GitUtils.CheckIfRepository();

            if (Args.Length == 0)
            {
                StartSelectReview();
                return;
            }

            if (Args.Length == 1)
            {
                StartReview(Args[0]);
                return;
            }
            
            HandleUnknownParam();
        }

        private void StartReview(string arg)
        {
            GitUtils.PerformFetch();
            
            using Repository repo = new Repository(Environment.CurrentDirectory);
            var results = repo.Branches
                .Where(b => b.IsRemote && b.FriendlyName.Contains(arg))
                .ToArray();
            
            if (results.Length == 0)
            {
                Log.Error("No such branch found.");
                return;
            }
            if (results.Length > 1)
            {
                StartSelectReview(results);
                return;
            }

            GitUtils.CheckOutBranch(repo, results[0]);
            GitUtils.ShowLatestCommit(repo);
        }

        private void StartSelectReview(Branch[] options = null)
        {
            var selectDescription = "There were more than one results. Select a branch to review:";
            
            if (options == null)
            {
                selectDescription = "Select a branch to review";
                GitUtils.PerformFetch();
            }
            using Repository repo = new Repository(Environment.CurrentDirectory);

            var availableBranches = options ?? repo.Branches.Where(b => b.IsRemote).ToArray();

            if (availableBranches.Length < 1)
            {
                Log.Error("No remote branches found.");
                Environment.Exit(1);
            }

            string[] branchNames = availableBranches.Select(b => b.FriendlyName).ToArray();
            
            int selectedIndex = new SimpleSelect(selectDescription, branchNames).Show();

            if (selectedIndex < 0) return;

            GitUtils.CheckOutBranch(repo, availableBranches[selectedIndex]);
            GitUtils.ShowLatestCommit(repo);
        }

        public override void ShowHelp()
        {
            Log.Info("Usage:");
            Console.WriteLine(HelpText.Reviewer);
        }
    }
}