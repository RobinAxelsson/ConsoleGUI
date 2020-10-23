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
        //public List<(int X, int Y)> DrawnBackground;
        public LayerObject ActiveObject { get; set; }
        public DrawBoard DrawBoard;
        public LayerHandler()
        {
            var layerObjects = new List<LayerObject>();
            LayerObjects = layerObjects;
            var Background = new LayerObject(ConsoleColor.Black);
            LayerObjects.Add(Background);
            //ActiveObject = Background;

        }

        public LayerObject CreateLayerObject(LayerObject.ShapeType shape, (int X, int Y) point1, (int X, int Y) point2, ConsoleColor color = ConsoleColor.White, bool isFillTrue = false)
        {
            var newObject = new LayerObject(point1, point2, shape, color, isFillTrue);
            LayerObjects.Add(newObject);
            ActiveObject = newObject;
            Refresh(newObject.TruePoints(isFillTrue));
            return newObject;
        }
        public void MoveActiveObject(int moveX, int moveY)
        {
            var shape = ActiveObject.Shape;
            int x1 = ActiveObject.Point1.X + moveX;
            int y1 = ActiveObject.Point1.Y + moveY;
            int x2 = ActiveObject.Point2.X + moveX;
            int y2 = ActiveObject.Point2.Y + moveY;

            var newGeometricalPoints = new List<(int X, int Y)>();
            (int X, int Y) point1 = (x1, y1);
            (int X, int Y) point2 = (x2, y2);

            if (shape == Rectangle) newGeometricalPoints = ShapeObject.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = ShapeObject.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = ShapeObject.CircleWithCenter(point1, point2);

            Refresh(newGeometricalPoints);

            ActiveObject.Point1 = point1;
            ActiveObject.Point2 = point2;
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

            if (shape == Rectangle) newGeometricalPoints = ShapeObject.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = ShapeObject.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = ShapeObject.CircleWithCenter(point1, point2);

            if (ActiveObject.FillTrue)
            {
                LayerObject.Fill(ref newGeometricalPoints);
            }

            Refresh(newGeometricalPoints);

            ActiveObject.Point1 = point1;
            ActiveObject.Point2 = point2;
        }
        public void ScaleActiveObject((int X, int Y) point2)
        {
            var shape = ActiveObject.Shape;
            (int X, int Y) point1 = ActiveObject.Point1;
            bool filltrue = ActiveObject.FillTrue;

            var newGeometricalPoints = new List<(int X, int Y)>();

            if (shape == Rectangle) newGeometricalPoints = ShapeObject.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = ShapeObject.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = ShapeObject.CircleWithCenter(point1, point2);

            if (filltrue)
            {
                LayerObject.Fill(ref newGeometricalPoints);
            }

            Refresh(newGeometricalPoints);
            ActiveObject.Point1 = point1;
            ActiveObject.Point2 = point2;
        }
    
        public void Refresh(List<(int X, int Y)> newInput)
        {
            int activeIndex = LayerObjects.FindIndex(x => x == ActiveObject);
            var truePointsOld = new List<List<(int X, int Y)>>();
            
            foreach (var obj in LayerObjects)
            {
                truePointsOld.Add(obj.TruePoints());
            }
            
            var drawPointsOld = new List<List<(int X, int Y)>>();
            foreach (var obj in LayerObjects)
            {
                drawPointsOld.Add(obj.DrawPoints);
            }
            
            var truePointsNew = truePointsOld;
            truePointsNew.Insert(activeIndex, newInput);
            truePointsNew.RemoveAt(activeIndex + 1);
            var drawPointsNew = new List<List<(int X, int Y)>>();

            int next = 0;

            for (int i = 0; i < truePointsNew.Count; i++) //New true points as List
            {
                drawPointsNew.Add(truePointsNew[i]);

                for (int j = 1+next; j < truePointsNew.Count; j++)
                {
                    drawPointsNew[i] = drawPointsNew[i].Except(truePointsNew[j]).ToList();
                }
                next++;
            }

            for (int i = 0; i < LayerObjects.Count; i++)
            {
                LayerObjects[i].DrawPoints = drawPointsNew[i];
            }

            for (int i = 0; i < truePointsNew.Count; i++) //New true points as List
            {
                for (int j = 0 + next; j < truePointsNew.Count; j++)
                {
                    drawPointsNew[i] = drawPointsNew[i].Except(drawPointsOld[j]).ToList();
                }
                next++;
            }
            for (int i = drawPointsNew.Count-1; i >= 0; i--)
            {
                DrawBoard.DrawAt(drawPointsNew[i], LayerObjects[i].Color);
            }

        }

      

    }
}
