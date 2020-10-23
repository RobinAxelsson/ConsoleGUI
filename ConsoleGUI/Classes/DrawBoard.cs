using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace ConsoleGUI
{
    //using static LayerObject.ShapeType;
    static class Constants //This is the drawingArea - the frame is bigger.
    {
        //Largest windowwidth = 200
        //Largest windowheight = 71

        public const int XStart = 5;
        public const int YStart = 3; 
        public const int XEnd = 150;
        public const int YEnd = 60;
    }
    public class CharPoint
    {
        public (int X, int Y) Point;
        public char Character;
        public ConsoleColor ForegroundColor = ConsoleColor.DarkGray;

        public CharPoint((int X, int Y) point, char character, ConsoleColor foregroundColor = ConsoleColor.DarkGray)
        {
            Point = point;
            Character = character;
            ForegroundColor = foregroundColor;
        }
        public void Draw(ConsoleColor backgroundcolor = ConsoleColor.Black)
        {
            DrawBoard.CharAt(Point, Character, ForegroundColor, backgroundcolor);
        }
    }


    public class DrawBoard
    {
        public (int X, int Y) StartPoint = (Constants.XStart, Constants.YStart);
        public (int X, int Y) EndPoint = (Constants.XEnd, Constants.YEnd);
        public List<CharPoint> CharPoints;

        public DrawBoard()
        {
            var charPoints = new List<CharPoint>();
            CharPoints = charPoints;
        }
        public void newCharPoint(List<(int X, int Y)> points, char character, ConsoleColor foregroundColor = ConsoleColor.DarkGray)
        {
            foreach (var p in points)
            {
            var charPoint = new CharPoint(p, character, foregroundColor);
            CharPoints.Add(charPoint);
            charPoint.Draw();
            }
        }
        public void newCharPoint((int X, int Y) point, char character, ConsoleColor foregroundColor = ConsoleColor.DarkGray)
        {
            var charPoint = new CharPoint(point, character, foregroundColor);
            CharPoints.Add(charPoint);
            charPoint.Draw();
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
        public void Grid(int rows, int columns, ConsoleColor color = ConsoleColor.DarkGray)        
        {
            int xOrigo = Constants.XStart - 1;
            int yOrigo = Constants.YStart - 1;
            (int X, int Y) frameOrigo = (xOrigo, yOrigo);         
                   
            int xLength = (Constants.XEnd + 1 - xOrigo);
            int yLength = (Constants.YEnd + 1 - yOrigo);

            int xSpacingForColumns = xLength / columns;
            int ySpacingForRows = yLength / rows;

            xLength = xSpacingForColumns * columns;
            yLength = ySpacingForRows * rows;

            (int X, int Y) frameEndpoint = (xLength+xOrigo, yLength+yOrigo);

            var allRowPoints = new List<(int X, int Y)>();
            var rowLines = new List<List<(int X, int Y)>>();

            for (int i = 0; i <= rows; i++)
            {
                var rowLine = new List<(int X, int Y)>();
                for (int x = frameOrigo.X; x <= frameEndpoint.X; x++)
                {
                    var rowPoint = (x, ySpacingForRows * i + yOrigo);
                    rowLine.Add(rowPoint);
                    allRowPoints.Add(rowPoint);
                }
                rowLines.Add(rowLine);
            }

            var allColumnPoints = new List<(int X, int Y)>();
            var columnLines = new List<List<(int X, int Y)>>();
            for (int i = 0; i <= columns; i++)
            {
                var columnLine = new List<(int X, int Y)>();
                for (int y = frameOrigo.Y; y <= frameEndpoint.Y; y++)
                {
                    var rowPoint = (xSpacingForColumns * i + xOrigo, y);
                    columnLine.Add(rowPoint);
                    allColumnPoints.Add(rowPoint);
                }
                columnLines.Add(columnLine);
            }


            var intersectingPoints = allRowPoints.Intersect(allColumnPoints).ToList();

            for (int i = 1; i < rowLines.Count-1; i++) //middle rows
            {
                for (int j = 0; j < rowLines[i].Count; j+=8)
                {
                    newCharPoint(rowLines[i][j], '─', color);                    
                }
            }
            for (int i = 1; i < columnLines.Count - 1; i++) //middle columns
            {
                for (int j = 0; j < columnLines[i].Count; j+=4)
                {
                    newCharPoint(columnLines[i][j], '│', color);
                }
            }

            newCharPoint(intersectingPoints, '\u0004');//diamonds
            newCharPoint(frameOrigo, '┌', color);
            newCharPoint((frameEndpoint.X, frameOrigo.Y), '┐', color);
            newCharPoint((frameOrigo.X, frameEndpoint.Y), '└', color);
            newCharPoint(frameEndpoint, '┘', color);
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
            return (point.X >= Constants.XStart && point.Y >= Constants.YStart && point.X <= Constants.XEnd && point.Y <= Constants.YEnd) ? true : false;
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
        public void DrawAt(List<(int X, int Y)> points, ConsoleColor color = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            var charPointValues = new List<(int X, int Y)>();

            if (CharPoints.Count>0)
            {
                foreach (var cp in CharPoints)
                {
                    charPointValues.Add(cp.Point);
                }
            }

            foreach ((int X, int Y) point in points)
            {
                if (charPointValues.Contains(point))
                {
                    int index = charPointValues.FindIndex(x => x == point);
                    CharPoints[index].Draw(color);
                }
                else
                {
                    if (IsInsideDrawboard((point)))
                    {
                        Console.SetCursorPosition(point.X, point.Y);
                        Console.BackgroundColor = color;
                        Console.Write(" ");                    
                    }            

                }
            }
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
        
        public static void CharAt(int left, int top, char c = ' ', ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)


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
        public static void CharAt((int X, int Y) point, char c = ' ', ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)


        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            Console.ForegroundColor = foregroundColor;
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write(c);

            ResetColors(colorSave);
            ResetCursor(cursorPos);

        }
        public static void CharAt(List<(int X, int Y)> points, char c = ' ', ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            var cursorPos = SaveCursor();
            var colorSave = SaveColors();

            Console.ForegroundColor = foregroundColor;
            foreach (var point in points)
            {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write(c);
            }

            ResetColors(colorSave);
            ResetCursor(cursorPos);
            Console.CursorVisible = true;

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
