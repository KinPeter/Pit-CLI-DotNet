using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Help;
using Pit.Types;
using Pit.UI;

namespace Pit.Git
{
    public class Checkout : PitAction, IPitActionSync
    {
        public Checkout(string[] args) : base("Checkout", args) { }

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
                branch = repo.Branches.First(b => !b.IsRemote && IsRequiredBranch(action, b));
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
            GitUtils.ShowLatestCommit(repo);

            if (Args.Length == 1 && Args[0] == "-r")
            {
                GitUtils.PerformPull(branch.FriendlyName);
                GitUtils.ShowLatestCommit(repo);
                return;
            }

            if (Args.Length > 0) HandleUnknownParam();
        }

        private void StartCheckout(string arg)
        {
            using Repository repo = new Repository(Environment.CurrentDirectory);
            Branch branch;

            try
            {
                branch = repo.Branches
                    .OrderBy(b => b.IsRemote)
                    .First(b => b.FriendlyName.Contains(arg));
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

            if (branch.IsRemote)
            {
                CheckoutRemote(repo, branch);
                return;
            }

            GitUtils.CheckOutBranch(repo, branch);
            GitUtils.ShowLatestCommit(repo);
        }

        private void StartSelectCheckout()
        {
            const string selectDescription = "Select a branch to check out";

            using Repository repo = new Repository(Environment.CurrentDirectory);

            var availableBranches = repo.Branches.OrderBy(b => b.IsRemote).ToArray();

            if (availableBranches.Length < 1)
            {
                Log.Error("No branches found.");
                Environment.Exit(1);
            }

            string[] branchNames = availableBranches.Select(b => b.FriendlyName).ToArray();

            int selectedIndex = new SimpleSelect(selectDescription, branchNames).Show();

            if (selectedIndex < 0) return;

            if (availableBranches[selectedIndex].IsRemote)
            {
                CheckoutRemote(repo, availableBranches[selectedIndex]);
                return;
            }

            GitUtils.CheckOutBranch(repo, availableBranches[selectedIndex]);
            GitUtils.ShowLatestCommit(repo);
        }

        private static bool IsRequiredBranch(string action, Branch b)
        {
            return action == "com"
                ? (b.FriendlyName.Equals("master") || b.FriendlyName.Equals("main"))
                : b.FriendlyName.Equals("develop");
        }

        private void CheckoutRemote(Repository repo, Branch branch)
        {
            Log.Blue("Only remote found.");
            GitUtils.PerformFetch();
            Console.WriteLine();
            GitUtils.CheckOutBranch(GetLocalName(branch));
            GitUtils.ShowLatestCommit(repo);
        }

        private static string GetLocalName(Branch originBranch)
        {
            return string.Join(
                '/',
                originBranch.FriendlyName
                    .Split('/')
                    .Skip(1)
                    .ToArray()
            );
        }

        public override void ShowHelp()
        {
            Log.Info("Usage:");
            Console.WriteLine(HelpText.Checkout);
        }
    }
}