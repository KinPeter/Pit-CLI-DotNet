using System;
using Pit.Types;

namespace Pit.Config
{
    public class ConfigFile : PitAction
    {
        public ConfigFile(string[] args) : base("Config", args) { }

        public override void Run()
        {
            string homePath = HomePath;
            Console.WriteLine($"home: {homePath}");
            
        }

        public override void ShowHelp()
        {
            throw new System.NotImplementedException();
        }

        private string HomePath
        {
            get
            {
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