using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Types;

namespace Pit.Git
{
    public class BranchCleaner : PitAction
    {
        public BranchCleaner(string[] args) : base("BranchCleaner", args) {}

        public override void Run()
        {
            GitHelper.CheckIfRepository();

            Log.Info("Got params:");
            foreach (string s in Args)
            {
                Console.WriteLine(s);
            }

            using Repository repo = new Repository(Environment.CurrentDirectory);
            foreach (Branch b in repo.Branches.Where(b => !b.IsRemote))
            {
                Console.WriteLine($"{(b.IsCurrentRepositoryHead ? "*" : " ")}{b.FriendlyName}");
            }
        }
    }
}