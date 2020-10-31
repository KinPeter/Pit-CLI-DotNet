using System;

namespace Pit.UI
{
    public abstract class SelectMenu
    {
        private string Description { get; }
        protected string[] MenuItems { get; }
        protected int HoveredIndex { get; set; }
        protected bool SelectionDone { get; set; }
        
        protected SelectMenu(string description, string[] menuItems)
        {
            Description = description;
            MenuItems = menuItems;
            HoveredIndex = 0;
            SelectionDone = false;
        }
        
        protected void StartAndLoop()
        {
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            Console.CursorVisible = false;

            while (!SelectionDone)
            {
                PrepareConsole();

                for (var i = 0; i < MenuItems.Length; i++)
                {
                    WriteConsoleItem(i);
                }

                bottomOffset = Console.CursorTop;
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                HandleKeyPress(keyInfo.Key);
                
                Console.SetCursorPosition(0, topOffset);
            }
            
            Console.SetCursorPosition(0, Math.Min(bottomOffset + 1, Console.BufferHeight - 1));
            Console.CursorVisible = true;
        }
        
        private void PrepareConsole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{Description}\n");
            Console.ResetColor();
        }

        protected abstract void WriteConsoleItem(int itemIndex);
        protected abstract void HandleKeyPress(ConsoleKey key);
    }
}