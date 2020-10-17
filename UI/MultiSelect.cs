using System;
using System.Collections.Generic;

namespace Pit.UI
{
    public class MultiSelect : SelectMenu
    {
        private readonly HashSet<int> selectedItems = new HashSet<int>();

        public MultiSelect(string description, string[] menuItems) : base(description, menuItems) {}

        public HashSet<int> Show()
        {
            StartAndLoop();
            return selectedItems;
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
                case ConsoleKey.Spacebar:
                    ToggleSelectItem();
                    break;
                case ConsoleKey.Escape:
                    selectedItems.Clear();
                    SelectionDone = true;
                    break;
                case ConsoleKey.Enter:
                    SelectionDone = true;
                    break;
            }
        }

        private void ToggleSelectItem()
        {
            if (selectedItems.Contains(HoveredIndex))
            {
                selectedItems.Remove(HoveredIndex);
            }
            else
            {
                selectedItems.Add(HoveredIndex);
            }
        }

        protected override void WriteConsoleItem(int itemIndex)
        {
            if (selectedItems.Contains(itemIndex))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            if (HoveredIndex == itemIndex)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            string cursor = HoveredIndex == itemIndex ? ">> " : "   ";
            string selector = selectedItems.Contains(itemIndex) ? " (*) " : " ( ) ";
            Console.WriteLine($"{cursor}{selector} {MenuItems[itemIndex]}");
            Console.ResetColor();
        }
    }
}