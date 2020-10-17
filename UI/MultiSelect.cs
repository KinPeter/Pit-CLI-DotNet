using System;
using System.Collections.Generic;

namespace Pit.UI
{
    public class MultiSelect
    {
        private readonly string description;
        private readonly string[] menuItems;
        private int hoveredIndex = 0;
        private readonly HashSet<int> selectedItems = new HashSet<int>();
        private bool selectionDone = false;

        public MultiSelect(string description, string[] menuItems)
        {
            this.description = description;
            this.menuItems = menuItems;
        }

        public HashSet<int> Show()
        {
            StartAndLoop();
            return selectedItems;
        }

        private void StartAndLoop()
        {
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            Console.CursorVisible = false;

            while (!selectionDone)
            {
                PrepareConsole();

                for (var i = 0; i < menuItems.Length; i++)
                {
                    WriteConsoleItem(i);
                }

                bottomOffset = Console.CursorTop;
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                HandleKeyPress(keyInfo.Key);
                
                Console.SetCursorPosition(0, topOffset);
            }
            
            Console.SetCursorPosition(0, bottomOffset + 1);
            Console.CursorVisible = true;
        }

        private void PrepareConsole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{description}\n");
            Console.ResetColor();
        }

        private void HandleKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    hoveredIndex = hoveredIndex == 0 ? menuItems.Length - 1 : hoveredIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    hoveredIndex = hoveredIndex == menuItems.Length - 1 ? 0 : hoveredIndex + 1;
                    break;
                case ConsoleKey.Spacebar:
                    ToggleSelectItem();
                    break;
                case ConsoleKey.Escape:
                    selectedItems.Clear();
                    selectionDone = true;
                    break;
                case ConsoleKey.Enter:
                    selectionDone = true;
                    break;
            }
        }

        private void ToggleSelectItem()
        {
            if (selectedItems.Contains(hoveredIndex))
            {
                selectedItems.Remove(hoveredIndex);
            }
            else
            {
                selectedItems.Add(hoveredIndex);
            }
        }

        private void WriteConsoleItem(int itemIndex)
        {
            if (selectedItems.Contains(itemIndex))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            if (hoveredIndex == itemIndex)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            string cursor = hoveredIndex == itemIndex ? ">> " : "   ";
            string selector = selectedItems.Contains(itemIndex) ? " (*) " : " ( ) ";
            Console.WriteLine($"{cursor}{selector} {menuItems[itemIndex]}");
            Console.ResetColor();
        }
    }
}