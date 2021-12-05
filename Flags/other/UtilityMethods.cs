using System;
using System.Collections.Generic;

namespace Flags
{
    public static class UtilityMethods
    {
        public static List<int> RandomInts(int min = 0, int max = 10, int length = 10)
        {
            var random = new Random();
            var ints = new List<int>();

            for (int i = 0; i < length; i++)
            {
                ints.Add(random.Next(min, max));
            }
            return ints;
        }
        public static void DragLine()
        {
            ConsoleKey? key = null;

            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }
            (int, int)LeftTop1 = (Console.CursorLeft, Console.CursorTop);

            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }

            (int, int)LeftTop2 = (Console.CursorLeft, Console.CursorTop);


        }
        public static void DrawAt(ConsoleColor color, int left, int top)
        {
            if (left >= Console.BufferWidth) left = Console.BufferWidth-1;
            Console.SetCursorPosition(left, top);
            Console.BackgroundColor = color;
            Console.Write(" ");
            Console.ResetColor();
        }
        public static void WriteAt(int left, int top, List<string> prompt)
        {
            for (int i = 0; i < prompt.Count; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.WriteLine(prompt[i]);
            }
        }
        public static void WriteAt(int left, int top, string prompt)
        {
           
            Console.SetCursorPosition(left, top);
            Console.WriteLine("                      ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine(prompt);
        }
        public static void TrackCursor()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleKey? key = null;

            int left; int top;

            while (key != ConsoleKey.Enter)
            {
                left = Console.CursorLeft;
                top = Console.CursorTop;
                WriteAt(70, 0, $"{left}, {top}");
                Console.SetCursorPosition(left, top);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }
        }
        public static void DrawWithCursor()
        {
            var coloroptions = new List<string>
            {
                "Press key to change color,",
                "Draw with spacebar",
                "Default Black[d]",
                "White [w]",
                "Blue [b]",
                "Red [r]",
                "Yellow [y]",
                "Green [g]"
            };

            WriteAt(70, 0, coloroptions);

            Console.SetCursorPosition(0, 0);
            ConsoleKey? key = null;


            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft<=Console.BufferWidth-1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0)Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;

                if (key == ConsoleKey.W) Console.BackgroundColor = ConsoleColor.White;
                if (key == ConsoleKey.D) Console.BackgroundColor = ConsoleColor.Black;
                if (key == ConsoleKey.B) Console.BackgroundColor = ConsoleColor.Blue;
                if (key == ConsoleKey.R) Console.BackgroundColor = ConsoleColor.Red;
                if (key == ConsoleKey.Y) Console.BackgroundColor = ConsoleColor.Yellow;
                if (key == ConsoleKey.G) Console.BackgroundColor = ConsoleColor.Green;

                if (key == ConsoleKey.Spacebar) Console.Write(" ");
            }
        }
        public static void MoveCursor()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft >= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }
        }
        public static int ShowMenu(string prompt, string[] options)
        {
            if (options == null || options.Length == 0)
            {
                throw new ArgumentException("Cannot show a menu for an empty array of options.");
            }

            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write("- " + option);
                    Console.ResetColor();
                    Console.WriteLine();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and return the selected option.
            Console.CursorVisible = true;
            return selected;
        }
    }
}
