using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Pit.Help;
using Pit.Types;
using Pit.UI;

namespace Pit.Git
{
    public class BranchCleaner : PitAction, IPitActionSync
    {
        private readonly string[] defaultProtectedBranches = {"master", "main", "develop"};

        public BranchCleaner(string[] args) : base("BranchCleaner", args) { }

        public void Run()
        {
            if (Args.Length > 0 && (Args[0] == "-h" || Args[0] == "--help"))
            {
                ShowHelp();
                return;
            }

            GitUtils.CheckIfRepository();

            if (Args.Length == 0)
            {
                StartMultiSelectClean();
                return;
            }

            if (Args.Length == 1 && (Args[0] == "-a" || Args[0] == "--auto"))
            {
                StartAutoClean();
                return;
            }

            if (Args.Length == 3
                && (Args[0] == "-a" || Args[0] == "--auto")
                && (Args[1] == "-p" || Args[1] == "--protect")
                && Args[2].Length > 0)
            {
                StartAutoClean(Args[2]);
                return;
            }

            HandleUnknownParam();
        }

        private void StartMultiSelectClean()
        {
            using Repository repo = new Repository(Environment.CurrentDirectory);
            var deletableBranches = GetDeletableBranches(repo);

            var branchNames = deletableBranches.Select(b => b.FriendlyName).ToArray();
            const string description = "Select branches to delete:";

            var selectedForDelete = new MultiSelect(description, branchNames).Show();

            if (selectedForDelete.Count == 0)
            {
                Log.Info("Nothing is selected. Quiting...");
                return;
            }

            foreach (int index in selectedForDelete)
            {
                Branch branch = deletableBranches[index];
                repo.Branches.Remove(branch);
                Log.Green($"Deleted branch: {branch.FriendlyName}");
            }

            Log.Green("Done.");
        }

        private void StartAutoClean(string rawProtected = null)
        {
            string[] protectedBranches = { };
            if (rawProtected != null)
            {
                protectedBranches = rawProtected.Split(",");
            }

            using Repository repo = new Repository(Environment.CurrentDirectory);
            var deletableBranches = GetDeletableBranches(repo);

            foreach (Branch branch in deletableBranches)
            {
                if (protectedBranches.Length > 0
                    && protectedBranches.Any(n => branch.FriendlyName.Contains(n))
                )
                {
                    Log.Blue($"Protected branch: {branch.FriendlyName}");
                    continue;
                }

                repo.Branches.Remove(branch);
                Log.Green($"Deleted branch: {branch.FriendlyName}");
            }

            Log.Green("Done.");
        }

        private List<Branch> GetDeletableBranches(Repository repo)
        {
            var deletableBranches = repo.Branches.Where(
                b => !b.IsRemote && !b.IsCurrentRepositoryHead && !defaultProtectedBranches.Contains(b.FriendlyName)
            ).ToList();

            if (deletableBranches.Count == 0)
            {
                Log.Info("No available branches to delete.");
                Environment.Exit(0);
            }

            return deletableBranches;
        }

        public override void ShowHelp()
        {
            Log.Blue("Usage:");
            Console.WriteLine(HelpText.BranchCleaner);
        }
    }
}