using System;

namespace Pit.Logs
{
    public class Logger
    {
        private readonly string module;

        public Logger(string module)
        {
            this.module = module;
        }

        public void Error(string title, string message = null)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[{module}] {title}");
            Console.ResetColor();
            
            if (message == null) return;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}