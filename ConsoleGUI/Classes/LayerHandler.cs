using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI
{
    using static LayerObject.ShapeType;
    public class LayerHandler
    {
        public List<LayerObject> LayerObjects { set; get; }
        public ConsoleColor BackgroundColor = ConsoleColor.Black;
        public LayerObject ActiveObject { get; set; }
        public LayerHandler()
        {
            var layerObjects = new List<LayerObject>();
            LayerObjects = layerObjects;
        }

        public LayerObject CreateLayerObject(LayerObject.ShapeType shape, (int X, int Y) point1, ConsoleColor color = ConsoleColor.White)
        {
            var point2 = (point1.X + 5, point1.Y + 3);
            var newObject = new LayerObject(point1, point2, shape, color);
            LayerObjects.Add(newObject);
            ActiveObject = newObject;
            return newObject;
        }

        public void MoveActiveObject(out ConsoleKey key)
        {
            var shape = ActiveObject.Shape;
            int x1 = ActiveObject.Point1.X;
            int y1 = ActiveObject.Point1.Y;
            int x2 = ActiveObject.Point2.X;
            int y2 = ActiveObject.Point2.Y;

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1)
            { Console.CursorLeft--; x1--; x2--; }

            if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd)
            { Console.CursorLeft++; x1++; x2++; }

            if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1)
            { Console.CursorTop--; y1--; y2--; }

            if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd)
            { Console.CursorTop++; y1++; y2++; }

            var newGeometricalPoints = new List<(int X, int Y)>();
            (int X, int Y) point1 = (x1, y1);
            (int X, int Y) point2 = (x2, y2);

            if (shape == Rectangle) newGeometricalPoints = Geometry.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = Geometry.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = Geometry.CircleWithCenter(point1, point2);

            DrawBoard.Erase(newGeometricalPoints, ActiveObject.GeometricalPoints);
            DrawBoard.DrawAt(newGeometricalPoints);
            ActiveObject.GeometricalPoints = newGeometricalPoints;
            ActiveObject.Point1 = point1;
            ActiveObject.Point2 = point2;
        }
        public void ScaleActiveObject((int X, int Y) point2)
        {
            var shape = ActiveObject.Shape;
            (int X, int Y) point1 = ActiveObject.Point1;

            var newGeometricalPoints = new List<(int X, int Y)>();

            if (shape == Rectangle) newGeometricalPoints = Geometry.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = Geometry.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = Geometry.CircleWithCenter(point1, point2);
            DrawBoard.Erase(newGeometricalPoints, ActiveObject.GeometricalPoints);
            DrawBoard.DrawAt(newGeometricalPoints);
            ActiveObject.GeometricalPoints = newGeometricalPoints;
            ActiveObject.Point1 = point1;
            ActiveObject.Point2 = point2;
        }
        public void FillActiveObject()
        {
            var Ys = Point.ToYs(ActiveObject.GeometricalPoints);

            int Ymin = Ys.Min();
            int Ymax = Ys.Max();

            var yLevels = new List<List<(int X, int Y)>>();

            for (int i = Ymin; i <= Ymax; i++)
            {
                var newList = new List<(int X, int Y)>();
                newList = ActiveObject.GeometricalPoints.Where(p => p.Y == i).ToList();
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
                    ActiveObject.GeometricalPoints.Add((x, y));
                }
            }
            ActiveObject.GeometricalPoints = ActiveObject.GeometricalPoints.Distinct().ToList();
            ActiveObject.FillTrue = true;
        }
        public void FilterDraw()
        {
            var tempLayers = new List<List<(int X, int Y)>>();
            var exceptLayer = new List<(int X, int Y)>();
            int next = 0;



            for (int i = 0; i < LayerObjects.Count - 1; i++)
            {
                var tempLayer = new List<(int X, int Y)>();
                tempLayers.Add(tempLayer);
                tempLayer.AddRange(LayerObjects[i].GeometricalPoints); //Adding the points as they were without interferance.

                for (int j = 1 + next; j < LayerObjects.Count; j++)
                {
                    exceptLayer.AddRange(LayerObjects[j].GeometricalPoints); //Removes all the layers under other layers.
                }
                tempLayer = LayerObjects[i].GeometricalPoints.Except(exceptLayer).ToList();
                exceptLayer.Clear();
                next++;
            }

            tempLayers.Add(LayerObjects[^1].GeometricalPoints); //adds the final layer.


            bool DrawPointsExists = LayerObjects.TrueForAll(x => x.DrawnPoints.Count != 0);

            if (DrawPointsExists)
            {
                var oldAllPoints = new List<(int X, int Y)>();
                var newAllPoints = new List<(int X, int Y)>();
                foreach (var layer in LayerObjects)
                {
                    oldAllPoints.AddRange(layer.DrawnPoints);
                }
                foreach (var tempLayer in tempLayers)
                {
                    newAllPoints.AddRange(tempLayer);
                }
                var newBackgroundPoints = oldAllPoints.Except(newAllPoints).ToList(); //added background;
                if (newBackgroundPoints.Count > 0)
                {
                    DrawBoard.DrawAt(newBackgroundPoints, BackgroundColor); //draw new background
                }
            }

            for (int i = 0; i < LayerObjects.Count; i++)
            {
                if (LayerObjects[i].DrawnPoints.Count == 0 || LayerObjects[i].Color != LayerObjects[i].NewColor)
                {
                    DrawBoard.DrawAt(tempLayers[i], LayerObjects[i].NewColor);
                    LayerObjects[i].Color = LayerObjects[i].NewColor;
                    LayerObjects[i].DrawnPoints = tempLayers[i];
                }
                else
                {
                    tempLayers[i] = tempLayers[i].Except(LayerObjects[i].DrawnPoints).ToList();
                    DrawBoard.DrawAt(tempLayers[i], LayerObjects[i].NewColor);
                }
            }

        }

    }
}
