using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI
{
    public class LayerObject
    {
        public enum ShapeType
        {
            Rectangle,
            Line,
            Circle
        }

        public List<(int X, int Y)> DrawPoints;
        public ShapeType Shape { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        public ConsoleColor? NewColor { get; set; } = null;
        public (int X, int Y) Point1;
        public (int X, int Y) Point2;
        public bool FillTrue;

        public LayerObject((int X, int Y) point1, (int X, int Y) point2, ShapeType shape, ConsoleColor color = ConsoleColor.White)
        {
            Shape = shape;
            Point1 = point1;
            Point2 = point2;
            Color = color;
            FillTrue = false;
            DrawPoints = new List<(int X, int Y)>();
        }

        public List<(int X, int Y)> TruePoints()
        {
            var truePoints = new List<(int X, int Y)>();

            if (Shape == ShapeType.Rectangle) truePoints = Geometry.RectanglePts(Point1, Point2);
            if (Shape == ShapeType.Line) truePoints = Geometry.LinePtPt(Point1, Point2);
            if (Shape == ShapeType.Circle) truePoints = Geometry.CircleWithCenter(Point1, Point2);
            if (FillTrue) { Fill(ref truePoints); }

            return truePoints;
        }
        public List<(int X, int Y)> PointGen(ShapeType shape, (int X, int Y) point1, (int X, int Y) point2)
        {
            var points = new List<(int X, int Y)>();
            if (shape == ShapeType.Rectangle) points = Geometry.RectanglePts(point1, point2);
            if (shape == ShapeType.Line) points = Geometry.LinePtPt(point1, point2);
            if (shape == ShapeType.Circle) points = Geometry.CircleWithCenter(point1, point2);
            return points;
        }

        public void Fill(ref List<(int X, int Y)> truePoints)
        {
            var Ys = Point.ToYs(truePoints);

            int Ymin = Ys.Min();
            int Ymax = Ys.Max();

            var yLevels = new List<List<(int X, int Y)>>();

            for (int i = Ymin; i <= Ymax; i++)
            {
                var newList = new List<(int X, int Y)>();
                newList = truePoints.Where(p => p.Y == i).ToList();
                yLevels.Add(newList);
            }

            int Xmin;
            int Xmax;
            var pointsTemp = new List<(int X, int Y)>();
            var Xs = new List<int>();
            int y;

            foreach (var level in yLevels)
            {
                Xs = Point.ToXs(level);
                Xmin = Xs.Min();
                Xmax = Xs.Max();

                for (int x = Xmin; x < Xmax; x++)
                {
                    y = level[0].Y;
                    truePoints.Add((x, y));
                }
            }
            truePoints = truePoints.Distinct().ToList();
        }
        public void Fill()
        {

            var truePoints = TruePoints();
            var Ys = Point.ToYs(truePoints);

            int Ymin = Ys.Min();
            int Ymax = Ys.Max();

            var yLevels = new List<List<(int X, int Y)>>();

            for (int i = Ymin; i <= Ymax; i++)
            {
                var newList = new List<(int X, int Y)>();
                newList = truePoints.Where(p => p.Y == i).ToList();
                yLevels.Add(newList);
            }

            int Xmin;
            int Xmax;
            var pointsTemp = new List<(int X, int Y)>();
            var Xs = new List<int>();
            int y;

            foreach (var level in yLevels)
            {
                Xs = Point.ToXs(level);
                Xmin = Xs.Min();
                Xmax = Xs.Max();

                for (int x = Xmin; x < Xmax; x++)
                {
                    y = level[0].Y;
                    truePoints.Add((x, y));
                }
            }
            truePoints = truePoints.Distinct().ToList();
            FillTrue = true;
        }
        public List<(int X, int Y)> PointGen2(ShapeType shape, (int X, int Y) point1, (int X, int Y) point2)
        {
            var points = new List<(int X, int Y)>();
            if (shape == ShapeType.Rectangle) points = Geometry.RectanglePts(point1, point2);
            if (shape == ShapeType.Line) points = Geometry.LinePtPt(point1, point2);
            if (shape == ShapeType.Circle) points = Geometry.CircleWithCenter(point1, point2);
            return points;
        }


    }
}
