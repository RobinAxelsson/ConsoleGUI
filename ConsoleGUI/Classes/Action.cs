using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI
{
    public static class Action
    {
        public enum Shape
        {
            Box,
            Line,
            Circle
        }
        public static List<(int X, int Y)> DynShape(Shape shape, out ConsoleColor finalColor, ConsoleKey stopKey = ConsoleKey.L)
        {
            (int X, int Y) point1 = Point.FromUser();
            (int X, int Y) point2;
            Draw.At(point1);

            var oldPoints = new List<(int X, int Y)> { point1 };
            var newPoints = new List<(int X, int Y)>();

            ConsoleKey? key = null;
            ConsoleColor? color = ConsoleColor.White;

            while (key != stopKey)
            {
                key = Console.ReadKey(true).Key;
                if (ColorChoice((ConsoleKey)key) != null)
                {
                    color = ColorChoice((ConsoleKey)key);
                }

                point2 = CursorMove((ConsoleKey)key);

                if (shape == Shape.Box)
                {
                    newPoints = Geometry.RectanglePts(point1, point2);
                }
                if (shape == Shape.Line)
                {
                    newPoints = Geometry.LinePtPt(point1, point2);
                }
                if (shape == Shape.Circle)
                {
                    newPoints = Geometry.Circle(point1, point2);
                }
                Draw.Erase(newPoints, oldPoints);
                Draw.At(newPoints, (ConsoleColor)color);
                oldPoints = newPoints;
            }

            finalColor = (ConsoleColor)color;
            return newPoints;
        }

        public static List<(int X, int Y)> Fill(List<(int X, int Y)> points)
        {
            var Xs = Point.ToXs(points);
            var Ys = Point.ToYs(points);

            
            points = points.Distinct().ToList();

            return points;
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

                if (key == ConsoleKey.LeftArrow)
                {
                    Console.CursorLeft--;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Xs = Xs.Select(x => x - 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);

                }
                else if (key == ConsoleKey.RightArrow)
                {
                    Console.CursorLeft++;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Xs = Xs.Select(x => x + 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    Console.CursorTop--;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Ys = Ys.Select(y => y - 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    Console.CursorTop++;
                    Xs = Point.ToXs(oldPoints);
                    Ys = Point.ToYs(oldPoints);
                    Ys = Ys.Select(y => y + 1).ToList();
                    newPoints = Point.IntsToPts(Xs, Ys);
                }
                else { }

                Draw.Erase(newPoints, oldPoints, color);
                Draw.At(newPoints, color);
                oldPoints = newPoints;
            }
            return newPoints;
        }
        public static (int X, int Y) CursorMove(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 1) Console.CursorLeft--;
            if (key == ConsoleKey.RightArrow && Console.CursorLeft < Console.BufferWidth - 2) Console.CursorLeft++;
            if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
            if (key == ConsoleKey.DownArrow) Console.CursorTop++;

            return (Console.CursorLeft - 1, Console.CursorTop);
        }
        public static void TrackCursorNumbers()
        {

            ConsoleKey? key = null;

            int left; int top;

            while (key != ConsoleKey.Enter)
            {
                left = Console.CursorLeft;
                top = Console.CursorTop;
                Draw.StringAt(70, 0, $"{left}, {top}");
                Console.SetCursorPosition(left, top);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }
        }
        public static ConsoleColor? ColorChoice(ConsoleKey key)
        {

            int X = Console.CursorLeft;
            int Y = Console.CursorTop;
            Console.CursorVisible = false;

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

            Draw.LinesAt(70, 0, coloroptions);

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
        public static void DrawWithCursor()
        {
            ConsoleKey? key = null;
            ConsoleColor? color = ConsoleColor.White;

            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true).Key;
                if (ColorChoice((ConsoleKey)key) != null)
                {
                    color = ColorChoice((ConsoleKey)key);
                }

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;

                if (key == ConsoleKey.Spacebar) Draw.At(Console.CursorLeft, Console.CursorTop, (ConsoleColor)color);
            }
        }
    }
}
