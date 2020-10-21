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
            (int X, int Y) point1;
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

            point1 = DrawBoard.PointFromCursor();


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

            while (key != Enter)
            {

            handler.ScaleActiveObject(DrawBoard.PointFromCursor(out key, true));
            }
            key = Spacebar;

            while (key != Enter)
            {
                handler.MoveActiveObject(out key);
            }
            key = Spacebar;

            while (key != Enter)
            { 

                if (key == D) handler.ActiveObject.NewColor = Black;
                if (key == W) handler.ActiveObject.NewColor = White;
                if (key == B) handler.ActiveObject.NewColor = Blue;
                if (key == R) handler.ActiveObject.NewColor = Red;
                if (key == ConsoleKey.Y) handler.ActiveObject.NewColor = Yellow;
                if (key == G) handler.ActiveObject.NewColor = Green;

                handler.ActiveObject.Fill();
                handler.FilterDraw();
                key = Console.ReadKey(true).Key;
            }
            Console.SetCursorPosition(150, 1);

            var prompt2 = new TextBody((40, 50), "Create second. Circle [c], Rectangle [e], Line [l]");

            while (key != Escape && key != C && key != E && key != L)
            {
                key = Console.ReadKey(true).Key;
                objectChoiceKey = key;
            }

            prompt2.Erase();

            Console.SetCursorPosition(75, 25);

            point1 = DrawBoard.PointFromCursor();


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

            while (key != Enter)
            {

                handler.ScaleActiveObject(DrawBoard.PointFromCursor(out key, true));
            }
            key = Spacebar;

            while (key != Enter)
            {
                handler.MoveActiveObject(out key);
            }
            key = Spacebar;

            while (key != Enter)
            {

                if (key == D) handler.ActiveObject.NewColor = Black;
                if (key == W) handler.ActiveObject.NewColor = White;
                if (key == B) handler.ActiveObject.NewColor = Blue;
                if (key == R) handler.ActiveObject.NewColor = Red;
                if (key == ConsoleKey.Y) handler.ActiveObject.NewColor = Yellow;
                if (key == G) handler.ActiveObject.NewColor = Green;

                handler.ActiveObject.Fill();
                handler.FilterDraw();
                key = Console.ReadKey(true).Key;
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

    
