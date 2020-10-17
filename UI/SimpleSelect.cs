using System;

namespace Pit.UI
{
    public class SimpleSelect : SelectMenu
    {
        private int selectedItemIndex = 0;
        
        public SimpleSelect(string description, string[] menuItems) : base(description, menuItems) { }
        
        public int Show()
        {
            StartAndLoop();
            return selectedItemIndex;
        }
        
        protected override void WriteConsoleItem(int itemIndex)
        {
            if (HoveredIndex == itemIndex)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            string cursor = HoveredIndex == itemIndex ? ">> " : "   ";
            Console.WriteLine($"{cursor}  {MenuItems[itemIndex]}");
            Console.ResetColor();
        }

        protected override void HandleKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    HoveredIndex = HoveredIndex == 0 ? MenuItems.Length - 1 : HoveredIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    HoveredIndex = HoveredIndex == MenuItems.Length - 1 ? 0 : HoveredIndex + 1;
                    break;
                case ConsoleKey.Escape:
                    selectedItemIndex = -1;
                    SelectionDone = true;
                    break;
                case ConsoleKey.Enter:
                    selectedItemIndex = HoveredIndex;
                    SelectionDone = true;
                    break;
            }
        }
    }
}