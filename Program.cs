using System;
using pit.Process;

namespace pit
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            ProcessRunner runner = new ProcessRunner("git branch && git remote -v");

            string output;
            
            try
            {
                output = runner.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



            var lines = output.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"{i}, {lines[i]}");
            }


        }
    }
}