using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI
{
    public static class Action
    {

        public static List<(int X, int Y)> DynShape(Layerobject.Type shape, out ConsoleColor finalColor, ConsoleKey stopKey = ConsoleKey.L)
        {
            (int X, int Y) point1 = Point.FromUser();
            (int X, int Y) point2;
            DrawBoard.At(point1);

            var oldPoints = new List<(int X, int Y)> { point1 };
            var newPoints = new List<(int X, int Y)>();

            ConsoleKey? key = null;
            ConsoleColor? color = ConsoleColor.White;

            while (key != stopKey)
            {
                key = Console.ReadKey(true).Key;
                if (Interface.ColorChoice((ConsoleKey)key) != null)
                {
                    color = Interface.ColorChoice((ConsoleKey)key);
                }

                point2 = CursorMoveInsideDrawingboard((ConsoleKey)key);

                if (shape == Layerobject.Type.Box)
                {
                    newPoints = Geometry.RectanglePts(point1, point2);
                }
                if (shape == Layerobject.Type.Line)
                {
                    newPoints = Geometry.LinePtPt(point1, point2);
                }
                if (shape == Layerobject.Type.Circle)
                {
                    newPoints = Geometry.CircleWithCenter(point1, point2);
                }
                DrawBoard.Erase(newPoints, oldPoints);
                DrawBoard.At(newPoints, (ConsoleColor)color);
                oldPoints = newPoints;
            }

            finalColor = (ConsoleColor)color;
            return newPoints;
        }


        public static List<(int X, int Y)> Move(ConsoleColor color, List<(int X, int Y)> oldPoints, ConsoleKey stopKey = ConsoleKey.Enter)
        {
            var newPoints = new List<(int X, int Y)>();
            var Xs = new List<int>();
            var Ys = new List<int>();

            ConsoleKey? key = null;

            while (key != stopKey)
            {

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1)
                {
                    Console.CursorLeft--;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Xs = Xs.Select(x => x - 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);

                }
                else if (key == ConsoleKey.RightArrow && Console.CursorLeft <= DrawBoard.XEnd)
                {
                    Console.CursorLeft++;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Xs = Xs.Select(x => x + 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);
                }
                else if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1)
                {
                    Console.CursorTop--;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Ys = Ys.Select(y => y - 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);
                }
                else if (key == ConsoleKey.DownArrow && Console.CursorTop <= DrawBoard.YEnd)
                {
                    Console.CursorTop++;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Ys = Ys.Select(y => y + 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);
                }
                else { }

                DrawBoard.Erase(newPoints, oldPoints, color);
                DrawBoard.At(newPoints, color);
                oldPoints = newPoints;
            }
            return newPoints;
        }
        public static (int X, int Y) CursorMoveInsideDrawingboard(ConsoleKey key)
        {

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 3) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= DrawBoard.XEnd) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow && Console.CursorTop <= DrawBoard.YEnd-1) Console.CursorTop++;

                return (Console.CursorLeft - 1, Console.CursorTop);
        }
    
    public static void DrawWithCursor()
    {
        ConsoleKey? key = null;
        ConsoleColor? color = ConsoleColor.White;

        while (key != ConsoleKey.Enter)
        {
            key = Console.ReadKey(true).Key;
            if (Interface.ColorChoice((ConsoleKey)key) != null)
            {
                color = Interface.ColorChoice((ConsoleKey)key);
            }

            if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
            if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
            if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
            if (key == ConsoleKey.DownArrow) Console.CursorTop++;

            if (key == ConsoleKey.Spacebar) DrawBoard.At(Console.CursorLeft, Console.CursorTop, (ConsoleColor)color);
        }
    }


}
}
