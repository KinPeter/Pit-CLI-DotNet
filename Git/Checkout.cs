using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Help;
using Pit.Types;
using Pit.UI;

namespace Pit.Git
{
    public class Checkout : PitAction
    {
        public Checkout(string[] args) : base("Checkout", args) { }

        public override void Run()
        {
            if (Args.Length == 1 && (Args[0] == "-h" || Args[0] == "--help"))
            {
                ShowHelp();
                return;
            }

            GitUtils.CheckIfRepository();

            if (Args.Length == 0)
            {
                StartSelectCheckout();
                return;
            }

            if (Args.Length == 1)
            {
                StartCheckout(Args[0]);
                return;
            }
            
            HandleUnknownParam();
        }

        public void RunShortcut(string action)
        {
            GitUtils.CheckIfRepository();
            using Repository repo = new Repository(Environment.CurrentDirectory);

            Branch branch;

            try
            {
                if (action == "com")
                {
                    branch = repo.Branches.First(b =>
                        !b.IsRemote && (b.FriendlyName.Equals("master") || b.FriendlyName.Equals("main")));
                }
                else
                {
                    branch = repo.Branches.First(b =>
                        !b.IsRemote && (b.FriendlyName.Equals("develop")));
                }
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException) branch = null;
                else throw;
            }

            if (branch == null)
            {
                Log.Error("No such branch found.");
                return;
            }
            
            GitUtils.CheckOutBranch(repo, branch);

            if (Args.Length == 1 && Args[0] == "-r")
            {
                GitUtils.PerformPull(branch.FriendlyName);
                return;
            }
            if (Args.Length > 0) HandleUnknownParam();
        }

        private void StartCheckout(string arg)
        {
            GitUtils.PerformFetch();
            
            using Repository repo = new Repository(Environment.CurrentDirectory);
            var results = repo.Branches
                .Where(b => b.FriendlyName.Contains(arg))
                .ToArray();
            
            if (results.Length == 0)
            {
                Log.Error("No such branch found.");
                return;
            }
            if (results.Length > 1)
            {
                StartSelectCheckout(results);
                return;
            }

            GitUtils.CheckOutBranch(repo, results[0]);
            GitUtils.ShowLatestCommit(repo);
        }

        private void StartSelectCheckout(Branch[] options = null)
        {
            var selectDescription = "There were more than one results. Select a branch to check out:";
            
            if (options == null)
            {
                selectDescription = "Select a branch to check out";
                GitUtils.PerformFetch();
            }
            using Repository repo = new Repository(Environment.CurrentDirectory);

            var availableBranches = options ?? repo.Branches.Where(b => b.IsRemote).ToArray();

            if (availableBranches.Length < 1)
            {
                Log.Error("No remote branches found.");
                Environment.Exit(0);
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
            Console.WriteLine(HelpText.Checkout);
        }
    }
}