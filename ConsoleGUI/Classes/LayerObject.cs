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

        public List<(int X, int Y)> GeometricalPoints;
        public List<(int X, int Y)> DrawnPoints;
        public ShapeType Shape { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        public ConsoleColor NewColor { get; set; }
        public (int X, int Y) Point1;
        public (int X, int Y) Point2;
        public bool FillTrue;

        public LayerObject((int X, int Y) point1, (int X, int Y) point2, ShapeType shape, ConsoleColor color = ConsoleColor.White)
        {
            Shape = shape;
            Point1 = point1;
            Point2 = point2;
            GeometricalPoints = PointGen(shape, point1, point2);
            NewColor = color;
            var drawPoints = new List<(int X, int Y)>();
            DrawnPoints = drawPoints;
        }

        public List<(int X, int Y)> PointGen(ShapeType shape, (int X, int Y) point1, (int X, int Y) point2)
        {
            var points = new List<(int X, int Y)>();
            if (shape == ShapeType.Rectangle) points = Geometry.RectanglePts(point1, point2);
            if (shape == ShapeType.Line) points = Geometry.LinePtPt(point1, point2);
            if (shape == ShapeType.Circle) points = Geometry.CircleWithCenter(point1, point2);
            return points;
        }

        public void UnFill()
        {
            GeometricalPoints = PointGen(Shape, Point1, Point2);
            FillTrue = false;
        }
        public void Fill()
        {
            var Ys = Point.ToYs(GeometricalPoints);

            int Ymin = Ys.Min();
            int Ymax = Ys.Max();

            var yLevels = new List<List<(int X, int Y)>>();

            for (int i = Ymin; i <= Ymax; i++)
            {
                var newList = new List<(int X, int Y)>();
                newList = GeometricalPoints.Where(p => p.Y == i).ToList();
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
                    GeometricalPoints.Add((x, y));
                }
            }
            GeometricalPoints = GeometricalPoints.Distinct().ToList();
            FillTrue = true;
        }

    }
}
