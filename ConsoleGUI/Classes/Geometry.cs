using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConsoleGUI
{
    public static class Geometry
    {
        public static List<(int X, int Y)> RectanglePts((int X, int Y) point1, (int X, int Y) point2)
        {
            var points = new List<(int X, int Y)>();

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

            if (Xs.Count >= Ys.Count)
            {
                foreach (int X in Xs)
                {
                    foreach (int Y in Ys)
                    {
                        points.Add((X, Y));
                    }
                }
            }
            else
            {
                foreach (int Y in Ys)
                {
                    foreach (int X in Xs)
                    {
                        points.Add((X, Y));
                    }
                }
            }

            return points;
        }
        public static List<(int X, int Y)> LinePtPt((int X, int Y) point1, (int X, int Y) point2)
        {
            var Xs = Point.LineValsX(point1, point2);
            var Ys = Point.LineValsY(point1, point2);

            var points = new List<(int X, int Y)>();

            if (Xs.Count >= Ys.Count)
            {
                double yStep = Ys.Count / (double)Xs.Count;
                double yIndex = 0;
                int yRastorIndex = 0;


                foreach (int X in Xs)
                {
                    yRastorIndex = (int)yIndex;
                    points.Add((X, Ys[yRastorIndex]));
                    yIndex += yStep;
                }
            }
            else //More Ys then Xs
            {
                double xStep = Xs.Count / (double)Ys.Count;
                double xIndex = 0;
                int xRastorIndex = 0;


                foreach (int Y in Ys)
                {
                    xRastorIndex = (int)xIndex;
                    points.Add((Xs[xRastorIndex], Y));
                    xIndex += xStep;
                }
            }
            return points;

        }
        public static List<(int X, int Y)> Circle((int X, int Y) point1, (int X, int Y) point2)
        {

            double xDif = Math.Abs(point1.X - point2.X);
            double yDif = Math.Abs(point1.Y - point2.Y);

            if (xDif <2) xDif = 2;
            else xDif = Math.Abs(xDif);

            if (yDif <2) yDif = 2;
            else yDif = Math.Abs(yDif);

            double radius = Math.Sqrt(Math.Pow(xDif, 2) + Math.Pow(yDif, 2));

            radius = Math.Round(radius, 10);
            double Range = radius;
            var doubleXs = new List<double>();
            var doubleYs = new List<double>();

            double xIncr = 0.001;
            double sqrt;
            double scewRatio = 0.48;

            for (double i = -Range; i <= Range; i = i + xIncr)
            {
                sqrt = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(i, 2));
                doubleYs.Add(sqrt * scewRatio);
                doubleXs.Add(i);
            }

            for (double i = -Range; i <= Range; i = i + xIncr)
            {
                sqrt = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(i, 2));
                doubleYs.Add(-(sqrt*scewRatio)); //The negative curve
                doubleXs.Add(i);
            }

            var Xs = new List<int>();
            var Ys = new List<int>();

            foreach (double x in doubleXs)
            {
                Xs.Add((int)Math.Round(x) + point1.X);
            }
            foreach (double y in doubleYs)
            {               
                    Ys.Add((int)Math.Round(y) + point1.Y);
            }

            var points = new List<(int X, int Y)>();
            int length = Xs.Count;

            for (int i = 0; i < length; i++)
            {
                points.Add((Xs[i], Ys[i]));
            }

            points = points.Distinct().ToList();

            return points;
        }
        static public void Frame(int Width, int Height)
        {
            Console.CursorVisible = false;
            for (int i = 1; i <= Width; i++)
            {
                Draw.CharAt(i, 0, '─');
                Draw.CharAt(i, (Height + 1), '─');
            }
            for (int i = 1; i <= (Height); i++)
            {
                Draw.CharAt(0, i, '│');
                Draw.CharAt((Width + 1), i, '│');
            }

            Draw.CharAt(0, 0, '┌');
            Draw.CharAt((Width + 1), 0, '┐');
            Draw.CharAt(0, (Height + 1), '└');
            Draw.CharAt((Width + 1), (Height + 1), '┘');

            Console.CursorVisible = true;
        }
    }
}
