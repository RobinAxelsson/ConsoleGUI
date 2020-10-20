using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI
{
    public static class Point
    {
       
        public static List<int> ToXs(List<(int X, int Y)> points)
        {
            return points.Select(p => p.X).ToList();
        }
        public static List<int> ToYs(List<(int X, int Y)> points)
        {
            return points.Select(p => p.Y).ToList();
        }
        public static List<(int X, int Y)> IntsToPts(List<int> Xs, List<int> Ys)
        {
            var points = new List<(int X, int Y)>();


            for (int i = 0; i < Xs.Count; i++)
            {
                points.Add((Xs[i], Ys[i]));
            }
            return points;
        }
        public static List<int> LineValsX((int X, int Y) point1, (int X, int Y) point2)
        {
            var Xs = new List<int>();
            int x = point1.X;
            int xEnd = point2.X;

            if (point1.X < point2.X)
            {
                while (x <= xEnd)
                {
                    Xs.Add(x);
                    x++;
                }
            }
            else
            {
                while (x >= xEnd)
                {
                    Xs.Add(x);
                    x--;
                }
            }
            return Xs;
        }
        public static List<int> LineValsY((int X, int Y) point1, (int X, int Y) point2)
        {
            var Ys = new List<int>();
            int y = point1.Y;
            int yEnd = point2.Y;

            if (point1.Y < point2.Y)
            {
                while (y <= yEnd)
                {
                    Ys.Add(y);
                    y++;
                }
            }
            else
            {
                while (y >= yEnd)
                {
                    Ys.Add(y);
                    y--;
                }
            }
            return Ys;
        }
        public static bool doubleXYsToInt(List<double> doubleXs, List<double> doubleYs, out List<int> Xs, out List<int> Ys)
        {

            Xs = new List<int>();
            Ys = new List<int>();

            foreach (double x in doubleXs)
            {
                Xs.Add((int)Math.Round(x));
            }
            foreach (double y in doubleYs)
            {
                Ys.Add((int)Math.Round(y));
            }
            return Xs.Count == Ys.Count;
        }
        public static List<(int X, int Y)> GetAddedPoints(List<(int X, int Y)> oldPoints, List<(int X, int Y)> totalPoints)
        {
            var newPoints = new List<(int X, int Y)>();
            //var Xs = new List<int>();
            //var Ys = new List<int>();

            //        Xs = Point.ToXs(oldPoints);
            //        Ys = Point.ToYs(oldPoints);
            //        Xs = Xs.Select(x => x - 1).ToList();
            //        newPoints = Point.IntsToPts(Xs, Ys);

            return newPoints;            
        }
        public static (int X, int Y) FromUser()
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
            return (Console.CursorLeft, Console.CursorTop);
        }
    }
}
