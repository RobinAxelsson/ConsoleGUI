using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI
{
    public static class random
    {
        public static void Spots()
        {
            var random = new Random();
            ConsoleKey? key = null;
            Console.CursorVisible = false;

            while (key != ConsoleKey.Enter)
            {
                int Left = random.Next(0, 80);
                int Top = random.Next(0, 20);
                key = Console.ReadKey(true).Key;
                DrawBoard.At(Left, Top, ConsoleColor.Red);
            }
        }
    }
}
