using System;
using System.Linq;
using System.Threading.Tasks;
using Pit.Config;
using Pit.Debug;
using Pit.Git;
using Pit.Help;
using Pit.Logs;
using Pit.Jira;

namespace Pit.Args
{
    public class ArgParser
    {
        private readonly Logger log;

        public ArgParser()
        {
            log = new Logger(GetType().Name);
        }

        public async Task Parse(string[] args)
        {
            CheckIfHasArgs(args);

            string action = args[0];
            string[] parameters = args.Skip(1).ToArray();

            if (action == "-h" || action == "--help" || action == "help")
            {
                ShowHelp();
                return;
            }

            if (action == "checkout" || action == "co")
            {
                new Checkout(parameters).Run();
                return;
            }

            if (action == "com" || action == "cod")
            {
                new Checkout(parameters).RunShortcut(action);
                return;
            }

            if (action == "review" || action == "rv")
            {
                new Reviewer(parameters).Run();
                return;
            }

            if (action == "clean" || action == "cl")
            {
                new BranchCleaner(parameters).Run();
                return;
            }

            if (action == "pulo")
            {
                new Puller(parameters).Run();
                return;
            }

            if (action == "user")
            {
                new User(parameters).Run();
                return;
            }

            if (action == "jira")
            { 
                await new JiraClient(parameters).RunAsync();
                return;
            }

            if (action == "debug")
            {
                new RepoDebugger(parameters).Run();
                return;
            }

            if (action == "config")
            {
                new ConfigFile(parameters).Run();
                return;
            }

            NoActionFound();
        }

        private void ShowHelp()
        {
            new Logger("Pit").Blue("Usage:");
            Console.WriteLine(HelpText.Main);
        }

        private void CheckIfHasArgs(string[] args)
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