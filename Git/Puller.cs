using System;
using LibGit2Sharp;
using Pit.Help;
using Pit.Types;

namespace Pit.Git
{
    public class Puller : PitAction, IPitActionSync
    {
        public Puller(string[] args) : base("Puller", args) { }

        public void Run()
        {
            if (Args.Length == 1 && (Args[0] == "-h" || Args[0] == "--help"))
            {
                ShowHelp();
                return;
            }

            if (Args.Length != 0)
            {
                HandleUnknownParam();
            }

            GitUtils.CheckIfRepository();
            PullOriginHead();
        }

        private void PullOriginHead()
        {
            using Repository repo = new Repository(Environment.CurrentDirectory);
            string currentHead = repo.Head.FriendlyName;
            GitUtils.PerformPull(currentHead);
            GitUtils.ShowLatestCommit(repo);
        }

        public override void ShowHelp()
        {
            Log.Info("Usage:");
            Console.WriteLine(HelpText.Puller);
        }
    }
}