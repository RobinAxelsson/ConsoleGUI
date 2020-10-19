using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConsoleGUI
{
    public static class DrawBoard
    {
        public static int XEnd = (int)Math.Round(Console.BufferWidth * 0.8);
        public static int YEnd = (int)Math.Round(Console.BufferHeight * 0.8);
        public static void Frame()
        {
            int Width = XEnd +1;
            int Height = YEnd +1;

            for (int i = 1; i <= Width; i++)
            {
                CharAt(i, 0, '─');
                CharAt(i, (Height), '─');
            }
            for (int i = 1; i <= (Height); i++)
            {
                CharAt(0, i, '│');
                CharAt((Width), i, '│');
            }

            CharAt(0, 0, '┌');
            CharAt((Width), 0, '┐');
            CharAt(0, (Height), '└');
            CharAt((Width), (Height), '┘');

            Console.CursorVisible = true;
        }
        public static bool IsInsideDrawboard((int X, int Y) point)
        {
            return (point.X >= 1 && point.Y >= 1 && point.X <= XEnd && point.Y <= YEnd) ? true : false;
        }
        public static void At(int X, int Y, ConsoleColor color = ConsoleColor.White)
        {
 
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            if (X >= Console.BufferWidth) X = Console.BufferWidth - 1;
            Console.SetCursorPosition(X, Y);
            Console.BackgroundColor = color;
            Console.Write(" ");
            Console.SetCursorPosition(X, Y);

            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;

        }
        public static void At((int X, int Y) point, ConsoleColor color = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            if (point.X >= Console.BufferWidth) point.X = Console.BufferWidth - 1;
            Console.SetCursorPosition(point.X, point.Y);
            Console.BackgroundColor = color;
            Console.Write(" ");

            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;
        }
        public static void At(List<(int X, int Y)> points, ConsoleColor color = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            foreach ((int X, int Y) point in points)
            {
                if (IsInsideDrawboard(point))
                {
                    Console.SetCursorPosition(point.X, point.Y);
                    Console.BackgroundColor = color;
                    Console.Write(" ");
                    
                }               
            }
            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;
        }
        public static void Erase(List<(int X, int Y)> newPoints, List<(int X, int Y)> oldPoints, ConsoleColor color = ConsoleColor.Black)
        {
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();
            Console.CursorVisible = false;

            var erasePoints = oldPoints.Except(newPoints).ToList();

            At(erasePoints, Console.BackgroundColor);

            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;
        }
        public static (ConsoleColor background, ConsoleColor foreground) SaveColors()
        {
            return (Console.BackgroundColor, Console.ForegroundColor);
        }
        public static void ResetColors((ConsoleColor background, ConsoleColor foreground) oldColors)
        {
            Console.ForegroundColor = oldColors.foreground;
            Console.BackgroundColor = oldColors.background;
        }
        
        public static void CharAt(int left, int top, char c = ' ', ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            Console.ForegroundColor = foregroundColor;            
            Console.SetCursorPosition(left, top);
            Console.Write(c);

            ResetColors(colorSave);
            ResetCursor(cursorPos);

        }
        public static void StringAt(int left, int top, string prompt)
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(left, top);
            Console.WriteLine("                  ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine(prompt);

            Console.CursorVisible = true;
        }
        public static void LinesAt(int left, int top, List<string> prompt)
        {
            Console.CursorVisible = false;
            for (int i = 0; i < prompt.Count; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.WriteLine(prompt[i]);
            }
        }
        public static (int X, int Y) SaveCursor()
        {
            int X = Console.CursorLeft;
            int Y = Console.CursorTop;
            return (X, Y);
        }
        public static void ResetCursor((int X, int Y) cursorPosition)
        {
            Console.CursorLeft = cursorPosition.X;
            Console.CursorTop = cursorPosition.Y;
        }
        
    }
}
