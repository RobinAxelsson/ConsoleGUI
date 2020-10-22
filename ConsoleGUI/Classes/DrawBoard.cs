using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace ConsoleGUI
{
    using static LayerObject.ShapeType;
    static class Constants
    {
        public const int YEnd = 50;
        public const int XEnd = 150;
    }
    public class DrawBoard
    {
        public (int X, int Y) StartPoint = (1, 1);
        public (int X, int Y) EndPoint = (Constants.XEnd, Constants.YEnd);
        public LayerObject Area;
        public static List<(int X, int Y)> AreaPoints;

        public DrawBoard()
        {           
            AreaFrame();
            BufferFrame();
            KeyOptions();          
            
        }
        public static (int X, int Y) PointFromCursor(out ConsoleKey key, bool dynamic = false)
        {
            key = ConsoleKey.Spacebar;

            if (!dynamic)
            {
                while (key != ConsoleKey.Enter)
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                    if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                    if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                    if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
                }
           
            }
            else
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
            }
            return (Console.CursorLeft - 1, Console.CursorTop);
        }
        public static (int X, int Y) PointFromCursor(bool dynamic = false)
        {
            ConsoleKey? key = null;

            if (!dynamic)
            {
                while (key != ConsoleKey.Enter)
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                    if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                    if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                    if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
                }

            }
            else
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
            }
            return (Console.CursorLeft - 1, Console.CursorTop);
        }
        public void AreaFrame()
        {

            int Width = EndPoint.X + 1;
            int Height = EndPoint.Y + 1;

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
        public void KeyOptions()
        {
            var coloroptions = new List<string>
            {
                "Press Hotkeys[hotkey]",
                "-for all commands-",
                "Confirm command [Enter]",
                "",
                "Change Color:",
                "Default Black[d]",
                "White [w]",
                "Blue [b]",
                "Red [r]",
                "Yellow [y]",
                "Green [g]",
                "",
                "New Object:",
                "Circle [c]",
                "Rectangle [e]",
                "Line [l]",
                "",
                "Change Current Object",
                "",
                "Move [m]",
                "Fill/Unfill [f]",
                "Rescale [s]",
                "Move object up stack [+]",
                "Move object back [-]",
                "Name [n]",
                "",
                "Change Object[Tab]",
                "",
            };

            LinesAt(159, 2, coloroptions);
        }

        public void BufferFrame()
        {

            int Width = Console.BufferWidth-5;
            int Height = Console.BufferHeight-5;

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
            return (point.X >= 1 && point.Y >= 1 && point.X <= Constants.XEnd && point.Y <= Constants.YEnd) ? true : false;
        }

        public static void DrawAt(int X, int Y, ConsoleColor color = ConsoleColor.White)
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
        public static void DrawAt((int X, int Y) point, ConsoleColor color = ConsoleColor.White)
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
        public static void DrawAt(List<(int X, int Y)> points, ConsoleColor color = ConsoleColor.White)
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

            DrawAt(erasePoints, Console.BackgroundColor);

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
        public void StringAt(int left, int top, string prompt)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            Console.SetCursorPosition(left, top);
            Console.WriteLine("                  ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine(prompt);

            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;
        }
        public static void LinesAt(int left, int top, List<string> prompt)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();
            for (int i = 0; i < prompt.Count; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.WriteLine(prompt[i]);
            }
            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;
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
