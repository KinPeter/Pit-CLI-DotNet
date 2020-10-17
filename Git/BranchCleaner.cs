using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Help;
using Pit.Types;

namespace Pit.Git
{
    public class BranchCleaner : PitAction
    {
        public BranchCleaner(string[] args) : base("BranchCleaner", args) {}

        public override void Run()
        {
            if (Args.Length == 0) HandleMissingParams();

            // Log.Info("Got params:");
            // foreach (string s in Args)
            // {
            //     Console.WriteLine(s);
            // }

            if (Args[0] == "-h" || Args[0] == "--help")
            {
                ShowHelp();
                return;
            }
            
            GitHelper.CheckIfRepository();

            using Repository repo = new Repository(Environment.CurrentDirectory);
            foreach (Branch b in repo.Branches.Where(b => !b.IsRemote))
            {
                Console.WriteLine($"{(b.IsCurrentRepositoryHead ? "*" : " ")}{b.FriendlyName}");
            }
        }

        public override void ShowHelp()
        {
            Log.Blue("Usage:");
            Console.WriteLine(HelpText.BranchCleaner);
        }
    }
}