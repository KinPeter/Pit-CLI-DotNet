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
            WriteWithTwoColors(title, message, ConsoleColor.DarkRed, ConsoleColor.Red);
        }

        public void Info(string title, string message = null)
        {
            WriteWithTwoColors(title, message, ConsoleColor.DarkBlue, ConsoleColor.Blue);
        }

        private void WriteWithTwoColors(string title, string message, ConsoleColor bgColor,
            ConsoleColor textColor)
        {
            Console.BackgroundColor = bgColor;
            Console.WriteLine($"[{module}] {title}");
            Console.ResetColor();
            
            if (message == null) return;

            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}