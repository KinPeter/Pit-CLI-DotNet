using System;
using Pit.Args;
using Pit.Process;
using Pit.UI;

namespace Pit
{
    class Program
    {
        public static void Main(string[] args)
        {
            new ArgParser().Parse(args);
            // string[] menuItems = {"First item", "Second item", "Third item", "Fourth item"};
            // MultiSelect select = new MultiSelect("Select branches to delete:", menuItems);
            // var menuSelection = select.Show();
            //
            // Console.WriteLine($"Selection ({menuSelection.Count}):");
            // foreach (int i in menuSelection)
            // {
            //     Console.WriteLine(i);
            // }

            // ProcessRunner runner = new ProcessRunner();
            //
            // string output;
            //
            // try
            // {
            //     output = runner.Run("git " + string.Join(" ", args));
            // }
            // catch (Exception e)
            // {
            //     if (e is CustomProcessException)
            //     {
            //         runner.HandleError(e.Message);
            //         Environment.Exit(0);
            //     }
            //
            //     throw;
            // }
            //
            // Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            //
            // var lines = output.Split('\n');
            //
            // for (var i = 0; i < lines.Length; i++)
            // {
            //     Console.WriteLine($"{i}, {lines[i]}");
            // }


        }
    }
}