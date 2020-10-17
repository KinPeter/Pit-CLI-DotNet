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

        public void Red(string message)
        {
            WriteWithColor(message, ConsoleColor.Red);
        }

        public void Green(string message)
        {
            WriteWithColor(message, ConsoleColor.Green);
        }

        public void Blue(string message)
        {
            WriteWithColor(message, ConsoleColor.Blue);
        }

        private void WriteWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{module}] {message}");
            Console.ResetColor();
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