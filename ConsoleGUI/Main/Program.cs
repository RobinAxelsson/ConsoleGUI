using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace ConsoleGUI
{
    using static LayerObject.ShapeType;
    public class Program
    {      

        public static void Main()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.BufferHeight = Console.WindowHeight + 3;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;           
           
            var drawBoard = new DrawBoard();
            var handler = new LayerHandler();


            ConsoleKey? key = null;

            while (key != ConsoleKey.Escape)
            {
                TrackCursorNumbers();
            }
            Console.SetCursorPosition(150, 1);
        }     


        public static ConsoleColor? ColorChoice(ConsoleKey key)
        {

            int X = Console.CursorLeft;
            int Y = Console.CursorTop;
            Console.CursorVisible = false;

            

            Console.CursorLeft = X;
            Console.CursorTop = Y;
            Console.CursorVisible = true;

            if (key == ConsoleKey.W) return ConsoleColor.White;
            if (key == ConsoleKey.D) return ConsoleColor.Black;
            if (key == ConsoleKey.B) return ConsoleColor.Blue;
            if (key == ConsoleKey.R) return ConsoleColor.Red;
            if (key == ConsoleKey.Y) return ConsoleColor.Yellow;
            if (key == ConsoleKey.G) return ConsoleColor.Green;
            else return null;
        }
        public static void TrackCursorNumbers()
        {

            ConsoleKey? key = null;

            int left; int top;

            while (key != ConsoleKey.Enter)
            {
                left = Console.CursorLeft;
                top = Console.CursorTop;
                DrawBoard.StringAt(2, 60, $"{left}, {top}");
                Console.SetCursorPosition(left, top);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }
        }
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

    }
}

    
