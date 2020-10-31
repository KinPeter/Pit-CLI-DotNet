using System;
using System.Linq;
using LibGit2Sharp;
using Pit.Config;
using Pit.Help;
using Pit.Types;

namespace Pit.Git
{
    public enum UserConfig
    {
        Personal,
        Work
    }

    public class User : PitAction, IPitActionSync
    {
        private readonly string personalEmail;
        private readonly string workEmail;

        public User(string[] args) : base("User", args)
        {
            PitConfig config = new ConfigFile().GetConfig();
            personalEmail = config.GitUser.PersonalEmail;
            workEmail = config.GitUser.WorkEmail;
        }

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
                ShowUser(isGlobal: false);
                return;
            }

            if (Args.Length == 1 && (Args[0] == "-g" || Args[0] == "--global"))
            {
                ShowUser(isGlobal: true);
                return;
            }

            if (Args.Length == 1 && (Args[0] == "-p" || Args[0] == "--personal"))
            {
                SetUser(isGlobal: false, config: UserConfig.Personal);
                return;
            }

            if (Args.Length == 1 && (Args[0] == "-w" || Args[0] == "--work"))
            {
                SetUser(isGlobal: false, config: UserConfig.Work);
                return;
            }

            if (
                Args.Length == 2 && 
                (Args.Contains("-g") || Args.Contains("--global")) &&
                (Args.Contains("-p") || Args.Contains("--personal"))
            )
            {
                SetUser(isGlobal: true, config: UserConfig.Personal);
                return;
            }

            if (
                Args.Length == 2 &&
                (Args.Contains("-g") || Args.Contains("--global")) &&
                (Args.Contains("-w") || Args.Contains("--work"))
            )
            {
                SetUser(isGlobal: true, config: UserConfig.Work);
                return;
            }

            HandleUnknownParam();
        }

        private void ShowUser(bool isGlobal)
        {
            ConfigurationLevel level = isGlobal ? ConfigurationLevel.Global : ConfigurationLevel.Local;

            using Repository repo = new Repository(Environment.CurrentDirectory);

            ConfigurationEntry<string> emailEntry = repo.Config.Get<string>("user.email", level);

            if (emailEntry == null)
            {
                Log.Red($"{level.ToString()} user is not set.");
                return;
            }

            Log.Green($"The {level.ToString()} user is set to {emailEntry.Value}");
        }

        private void SetUser(bool isGlobal, UserConfig config)
        {
            CheckEmailVariables();

            ConfigurationLevel level = isGlobal ? ConfigurationLevel.Global : ConfigurationLevel.Local;

            using Repository repo = new Repository(Environment.CurrentDirectory);

            var emailToSet = "";

            switch (config)
            {
                case UserConfig.Personal:
                    emailToSet = personalEmail;
                    break;
                case UserConfig.Work:
                    emailToSet = workEmail;
                    break;
            }

            repo.Config.Set("user.email", emailToSet, level);
            
            ShowUser(isGlobal);
        }

        private void CheckEmailVariables()
        {
            if (string.IsNullOrEmpty(personalEmail) || string.IsNullOrEmpty(workEmail))
            {
                Log.Error("Cannot find personal or work email", "Please set up the required config.");
                Environment.Exit(1);
            }
        }

        public override void ShowHelp()
        {
            Log.Info("Usage:");
            Console.WriteLine(HelpText.User);
        }
    }
}