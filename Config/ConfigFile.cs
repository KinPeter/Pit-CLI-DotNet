using System;
using System.IO;
using Pit.OS;
using Pit.Types;

namespace Pit.Config
{
    public class ConfigFile : PitAction, IPitActionSync
    {
        public ConfigFile(string[] args) : base("Config", args) { }
        
        public ConfigFile() : base("Config", null) { }

        public void Run()
        {
            PitConfig config = GetConfig();

            Log.Green($"Git user personal email: {config.GitUser.PersonalEmail}");
            Log.Green($"Git user work email: {config.GitUser.WorkEmail}");
        }

        public PitConfig GetConfig()
        {
            string homePath = HomePath;
            string configPath = Path.Combine(homePath, ".pitconfig");

            IniReader reader = new IniReader();
            var dictionary = reader.Read(configPath);
            
            return PitConfig.FromDictionary(dictionary);
        }
        
        public override void ShowHelp()
        {
            Log.Info("Help is not implemented for this module.");
        }

        private string HomePath
        {
            get
            {
                if (new Os().IsLinux())
                {
                    return Environment.GetEnvironmentVariable("HOME");
                }
                string homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
                string homeFolder = Environment.GetEnvironmentVariable("HOMEPATH");
                if (string.IsNullOrWhiteSpace(homeDrive) || string.IsNullOrWhiteSpace(homeFolder))
                {
                    Log.Error("Cannot find home directory path");
                    Environment.Exit(1);
                }

                return homeDrive + homeFolder;
            }
        }
    }
}