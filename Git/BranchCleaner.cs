using System;
using Pit.Logs;
using Action = Pit.Types.Action;

namespace Pit.Git
{
    public class BranchCleaner : Action
    {
        private readonly Logger log;
        private readonly string[] args;

        public BranchCleaner(string[] args)
        {
            this.args = args;
            log = new Logger(GetType().Name);
        }

        public void Run()
        {
            Console.WriteLine("Got params:");
            foreach (string s in args)
            {
                Console.WriteLine(s);
            }
        }
    }
}