using System;
using Pit.Logs;

namespace Pit.Types
{
    public abstract class PitAction
    {
        protected Logger Log { get; }
        protected string[] Args { get; }
        protected PitAction(string module, string[] args)
        {
            Log = new Logger(module);
            Args = args;
        }

        public abstract void ShowHelp();

        protected void HandleMissingParams()
        {
            Log.Error(
                "Not enough parameters!", 
                "Please specify which action you want to execute. \nRun \"pit <command> --help\" for more info."
                );
            Environment.Exit(1);
        }

        protected void HandleUnknownParam()
        {
            Log.Error(
                "Unknown parameter!", 
                "Please use valid parameters. \nRun \"pit <command> --help\" for more info."
                );
            Environment.Exit(1);
        }
    }
}