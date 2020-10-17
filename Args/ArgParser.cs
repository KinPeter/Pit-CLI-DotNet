using System;
using System.Linq;
using Pit.Git;
using Pit.Logs;

namespace Pit.Args
{
    public class ArgParser
    {
        private readonly Logger log;

        public ArgParser()
        {
            log = new Logger(GetType().Name);
        }

        public void Parse(string[] args)
        {
            CheckArgs(args);

            string action = args[0];
            string[] parameters = args.Skip(1).ToArray();

            if (action == "clean")
            {
                new BranchCleaner(parameters).Run();
                return;
            }

            NoActionFound();
        }

        private void CheckArgs(string[] args)
        {
            if (args.Length != 0) return;
            log.Error("Not enough arguments!", "Please specify an action");
            Environment.Exit(1);
        }

        private void NoActionFound()
        {
            log.Error("Invalid action.");
            Environment.Exit(1);
        }
    }
}