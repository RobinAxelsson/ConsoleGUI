using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI
{
    public abstract class IShape
    {
        public (int X, int Y) Point1;
        public (int X, int Y) Point2;
        public List<(int X, int Y)> Coordinates;
        public ConsoleColor Color;
        public bool IsFilled;
        public abstract void Geometry();
    }
    public class Circle : IShape
    {
        public Circle((int X, int Y) point1, (int X, int Y) point2, ConsoleColor color, bool fill = false)
        {
            Point1 = point1;
            Point2 = point2;
            Color = color;
            IsFilled = false;
            Geometry();
        }
        public override void Geometry()
        {
            double radius = ConsoleGUI.Coordinates.RadiusFrom2Point(Point1, Point2, 10);
            var points = ConsoleGUI.Coordinates.CircleEdgePoints(radius);
            points = ConsoleGUI.Coordinates.SetCenterpoint(Point1, points);

            Coordinates = points;
        }
    }
    public class Rectangle : IShape
    {
        public Rectangle((int X, int Y) point1, (int X, int Y) point2, ConsoleColor color, bool fill = false)
        {
            Point1 = point1;
            Point2 = point2;
            Color = color;
            IsFilled = fill;
        }
        public override void Geometry()
        {
            var points = new List<(int X, int Y)>();

            var Xs = new List<int>();
            int x = Point1.X;
            int xEnd = Point2.X;

            if (Point1.X < Point2.X)
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
            int y = Point1.Y;
            int yEnd = Point2.Y;

            if (Point1.Y < Point2.Y)
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
            foreach (var X in Xs)
            {
                points.Add((X, Point1.Y));
                points.Add((X, Point2.Y));
            }
            foreach (var Y in Ys)
            {
                points.Add((Point1.X, Y));
                points.Add((Point2.X, Y));
            }
            points = points.Distinct().ToList();

            Coordinates = points;

        }
    }
    public static class Coordinates
    {
        public static bool CircleEdgeXYsDouble(out List<double> doubleXs, out List<double> doubleYs, double radius, double xIncr = 0.001, double scewRatio = 0.5)
        {
            double sqrt;
            doubleXs = new List<double>();
            doubleYs = new List<double>();

            for (double i = -radius; i <= radius; i = i + xIncr)
            {
                sqrt = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(i, 2));
                doubleYs.Add(sqrt * scewRatio);
                doubleXs.Add(i);
            }

            for (double i = -radius; i <= radius; i = i + xIncr)
            {
                sqrt = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(i, 2));
                doubleYs.Add(-(sqrt * scewRatio)); //The negative curve
                doubleXs.Add(i);
            }
            return doubleXs.Count == doubleYs.Count;
        }
        public static List<(int X, int Y)> CircleEdgePoints(double radius)
        {

            var doubleXs = new List<double>();
            var doubleYs = new List<double>();

            bool same = CircleEdgeXYsDouble(out doubleXs, out doubleYs, radius);

            var Xs = new List<int>();
            var Ys = new List<int>();

            bool equal = Point.doubleXYsToInt(doubleXs, doubleYs, out Xs, out Ys);
            var points = Point.IntsToPts(Xs, Ys);

            points = points.Distinct().ToList();

            return points;
        }
        public static List<(int X, int Y)> SetCenterpoint((int X, int Y) point1, List<(int X, int Y)> points)
        {
            var Xs = Point.ToXs(points);
            var Ys = Point.ToYs(points);

            var newpoints = new List<(int X, int Y)>();
            int X;
            int Y;
            for (int i = 0; i < Xs.Count; i++)
            {
                X = Xs[i] + point1.X;
                Y = Ys[i] + point1.Y;
                newpoints.Add((X, Y));
            }
            return newpoints;
        }
        public static double RadiusFrom2Point((int X, int Y) point1, (int X, int Y) point2, int decimals)
        {
            double xDif = Math.Abs(point1.X - point2.X);
            double yDif = Math.Abs(point1.Y - point2.Y);

            if (xDif < 2) xDif = 2;
            else xDif = Math.Abs(xDif);

            if (yDif < 2) yDif = 2;
            else yDif = Math.Abs(yDif);

            double radius = Math.Sqrt(Math.Pow(xDif, 2) + Math.Pow(yDif, 2));
            radius = Math.Round(radius, decimals);

            return radius;
        }

    }
}
