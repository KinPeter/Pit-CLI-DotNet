using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Logs;
using Pit.Types;

namespace Pit.Git
{
    public class BranchCleaner : PitAction
    {
        private readonly Logger log;
        private readonly string[] args;

        public BranchCleaner(string[] args)
        {
            this.args = args;
            log = new Logger(GetType().Name);
        }

        public override void Run()
        {
            Console.WriteLine("Got params:");
            foreach (string s in args)
            {
                Console.WriteLine(s);
            }

            using Repository repo = new Repository(Environment.CurrentDirectory);
            Console.WriteLine(repo.Info);
            foreach(Branch b in repo.Branches.Where(b => !b.IsRemote))
            {
                Console.WriteLine($"{(b.IsCurrentRepositoryHead ? "*" : " ")}{b.FriendlyName}");
            }
        }
    }
}