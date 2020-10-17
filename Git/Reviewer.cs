using System;
using Pit.Help;
using Pit.Types;

namespace Pit.Git
{
    public class Reviewer : PitAction
    {
        public Reviewer(string[] args) : base("Reviewer", args) { }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override void ShowHelp()
        {
            Log.Info("Usage:");
            Console.WriteLine(HelpText.Reviewer);
        }
    }
}