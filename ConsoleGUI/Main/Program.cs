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
    using static ConsoleColor;
    using static ConsoleKey;
    public class Program
    {      

        public static void Main()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.BufferHeight = Console.WindowHeight + 3;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.CursorVisible = false;
            var drawBoard = new DrawBoard();
            var handler = new LayerHandler();

            var prompt = new TextBody((40, 25), "Create your first object. Circle [c], Rectangle [e], Line [l]");

            ConsoleKey key = Console.ReadKey(true).Key;
            ConsoleKey objectChoiceKey = key;

            while (key != Escape && key != C && key != E && key != L)
            {
                key = Console.ReadKey(true).Key;
                objectChoiceKey = key;
            }

            prompt.Erase();

            Console.SetCursorPosition(75, 25);

            (int X, int Y) point1 = drawBoard.PointFromCursor();

            if (objectChoiceKey == C)
            {
                handler.CreateLayerObject(Circle, point1);
            }
            if (objectChoiceKey == E)
            {
                handler.CreateLayerObject(Rectangle, point1);
            }
            if (objectChoiceKey == L)
            {
                handler.CreateLayerObject(Line, point1);
            }
            handler.FilterDraw();

            //while (key != Escape)
            //{
            //    if (key == D) handler.ActiveObject.NewColor = Black;
            //    if (key == W) handler.ActiveObject.NewColor = White;
            //    if (key == B) handler.ActiveObject.NewColor = Blue;
            //    if (key == R) handler.ActiveObject.NewColor = Red;
            //    if (key == ConsoleKey.Y) handler.ActiveObject.NewColor = Yellow;
            //    if (key == G) handler.ActiveObject.NewColor = Green;

            //    handler.FilterDraw();
            //}
            Console.SetCursorPosition(150, 1);
            Console.ReadKey();
        }     


        //public static ConsoleColor? ColorChoice(ref ConsoleKey key)
        //{

        //    int X = Console.CursorLeft;
        //    int Y = Console.CursorTop;
        //    Console.CursorVisible = false;

            

        //    Console.CursorLeft = X;
        //    Console.CursorTop = Y;
        //    Console.CursorVisible = true;

        //    if (key == W) return White;
        //    if (key == D) return Black;
        //    if (key == B) return Blue;
        //    if (key == R) return Red;
        //    if (key == ConsoleKey.Y) return Yellow;
        //    if (key == G) return Green;

        //}
        //public static void TrackCursorNumbers()
        //{

        //    ConsoleKey? key = null;

        //    int left; int top;

        //    while (key != ConsoleKey.Enter)
        //    {
        //        left = Console.CursorLeft;
        //        top = Console.CursorTop;
        //        drawBoard.StringAt(2, 60, $"{left}, {top}");
        //        Console.SetCursorPosition(left, top);

        //        key = Console.ReadKey(true).Key;

        //        if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
        //        if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
        //        if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
        //        if (key == ConsoleKey.DownArrow) Console.CursorTop++;
        //    }
        //}
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

    
