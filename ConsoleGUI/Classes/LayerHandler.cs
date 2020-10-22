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
            Refresh(newObject.TruePoints());
            return newObject;
        }
        public LayerObject CreateLayerObject(LayerObject.ShapeType shape, (int X, int Y) point1, (int X, int Y) point2, ConsoleColor color = ConsoleColor.White)
        {
            var newObject = new LayerObject(point1, point2, shape, color);
            LayerObjects.Add(newObject);
            ActiveObject = newObject;
            Refresh(newObject.TruePoints());
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

            if (shape == Rectangle) newGeometricalPoints = Geometry.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = Geometry.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = Geometry.CircleWithCenter(point1, point2);

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

            if (shape == Rectangle) newGeometricalPoints = Geometry.RectanglePts(point1, point2);
            if (shape == Line) newGeometricalPoints = Geometry.LinePtPt(point1, point2);
            if (shape == Circle) newGeometricalPoints = Geometry.CircleWithCenter(point1, point2);

            Refresh(newGeometricalPoints);

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

            Refresh(newGeometricalPoints);
            ActiveObject.Point1 = point1;
            ActiveObject.Point2 = point2;
        }

        public void Refresh(List<(int X, int Y)> newTruePoints)
        {
            var newTrueOverwritePoints = new List<(int X, int Y)>();
            var addPoints = new List<(int X, int Y)>();
            var overwritePoints = new List<(int X, int Y)>();
            var coverPoints = new List<(int X, int Y)>();
            addPoints = newTruePoints.Except(ActiveObject.DrawPoints).ToList(); //Points to draw and the potentially covered points. filter last!!

            newTrueOverwritePoints = ActiveObject.DrawPoints.Except(newTruePoints).ToList(); //points to overwrite by others if not covered.
            overwritePoints = newTrueOverwritePoints;

            int activeIndex = LayerObjects.FindIndex(x => x == ActiveObject); //Layer of the active object
            bool nothingToDraw = false;
            bool topObject = activeIndex == LayerObjects.Count-1;
            bool onlyOneObject = LayerObjects.Count == 1;
            bool isLowestObject = activeIndex == 0;
            bool nothingToOverwrite = false;

            if (onlyOneObject)
            {
                DrawBoard.DrawAt(addPoints, ActiveObject.Color);
                ActiveObject.DrawPoints.AddRange(addPoints);
                DrawBoard.DrawAt(newTrueOverwritePoints, BackgroundColor);
                ActiveObject.DrawPoints = ActiveObject.DrawPoints.Except(newTrueOverwritePoints).ToList();
                return;
            }
            
            else //look for cover
            { 
                for (int i = LayerObjects.Count - 1; i > activeIndex; i--) //Looks for cover starting at the top;
                {
                    addPoints = addPoints.Except(LayerObjects[i].DrawPoints).ToList(); //Removes cover

                    if (addPoints.Count == 0)
                    {
                        nothingToDraw = true; //all new points are covered
                        break;
                    }
                }

                if (!nothingToDraw) //some new points are not covered
                {
                    DrawBoard.DrawAt(addPoints, ActiveObject.Color);
                    ActiveObject.DrawPoints.AddRange(addPoints);

                    if (!isLowestObject)
                    {

                        for (int i = activeIndex-1; i >= 0; i--) //remove point from overdrawn layers
                        {
                            coverPoints = addPoints.Intersect(LayerObjects[i].TruePoints()).ToList();

                            if (coverPoints.Count != 0)
                            {
                                LayerObjects[i].DrawPoints = LayerObjects[i].DrawPoints.Except(coverPoints).ToList();
                                addPoints = addPoints.Except(coverPoints).ToList();
                                coverPoints.Clear();
                            }
                            if (addPoints.Count == 0)
                            {
                                break;
                            }
                        }
                    }

                }
            } //All drawing done.



            //Draw and change points in drawpoints - plus and minus.

            if (!topObject)
            {
                for (int i = LayerObjects.Count - 1; i > activeIndex; i--) //Looks for cover starting at the top;
                {
                    overwritePoints = overwritePoints.Except(LayerObjects[i].DrawPoints).ToList(); //Removes cover

                    if (overwritePoints.Count == 0)
                    {
                        nothingToOverwrite = true; //all overwritePoints are covered
                        break;
                    }
                }
            }

            var intersectedPoints = new List<(int X, int Y)>();
            bool nothingMoreToOverwrite = false;


            if (!nothingToOverwrite)
            {
                if (activeIndex == 0) //Our object cant cover anything if it is the lowest layer. But have to check for cover.
                {
                    DrawBoard.DrawAt(overwritePoints, BackgroundColor);
                }
                else
                {
                    for (int i = activeIndex-1; i >= 0; i--) //The intersecting field is drawn until background [0] takes the last
                    {
                        intersectedPoints = overwritePoints.Intersect(LayerObjects[i].TruePoints()).ToList();

                        if (intersectedPoints.Count != 0)
                        {
                        DrawBoard.DrawAt(intersectedPoints, LayerObjects[i].Color);
                        LayerObjects[i].DrawPoints.AddRange(intersectedPoints);
                        overwritePoints = overwritePoints.Except(intersectedPoints).ToList();
                        }
                        intersectedPoints.Clear();

                        if (overwritePoints.Count == 0)
                        {
                            nothingMoreToOverwrite = true;
                            break;
                        }
                    }
                    if (!nothingMoreToOverwrite)
                    {
                        DrawBoard.DrawAt(overwritePoints, BackgroundColor);
                    }
                }
            }

            ActiveObject.DrawPoints.Except(newTrueOverwritePoints).ToList();
        }

        public void FillActiveObject()
        {
            var truePoints = ActiveObject.TruePoints();
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
            var Xs = new List<int>();
            int y;
            var pointsTemp = new List<(int X, int Y)>();

            foreach (var level in yLevels)
            {
                Xs = Point.ToXs(level);
                Xmin = Xs.Min();
                Xmax = Xs.Max();

                for (int x = Xmin; x < Xmax; x++)
                {
                    y = level[0].Y;
                    pointsTemp.Add((x, y));
                }
            }
            truePoints = pointsTemp.Distinct().ToList();
            Refresh(truePoints);
            ActiveObject.FillTrue = true;
        }
        //public void FilterDraw()
        //{
        //    var tempLayers = new List<List<(int X, int Y)>>();
        //    var exceptLayer = new List<(int X, int Y)>();
        //    int next = 0;



        //    for (int i = 0; i < LayerObjects.Count - 1; i++)
        //    {
        //        var tempLayer = new List<(int X, int Y)>();
        //        tempLayers.Add(tempLayer);
        //        tempLayer.AddRange(LayerObjects[i].TruePoints()); //Adding the points as they were without interferance.

        //        for (int j = 1 + next; j < LayerObjects.Count; j++)
        //        {
        //            exceptLayer.AddRange(LayerObjects[j].TruePoints()); //Removes all the layers under other layers.
        //        }
        //        tempLayer = LayerObjects[i].GeometricalPoints.Except(exceptLayer).ToList();
        //        exceptLayer.Clear();
        //        next++;
        //    }

        //    tempLayers.Add(LayerObjects[^1].GeometricalPoints); //adds the final layer.


        //    bool DrawPointsExists = LayerObjects.TrueForAll(x => x.DrawnPoints.Count != 0);

        //    if (DrawPointsExists)
        //    {
        //        var oldAllPoints = new List<(int X, int Y)>();
        //        var newAllPoints = new List<(int X, int Y)>();
        //        foreach (var layer in LayerObjects)
        //        {
        //            oldAllPoints.AddRange(layer.DrawnPoints);
        //        }
        //        foreach (var tempLayer in tempLayers)
        //        {
        //            newAllPoints.AddRange(tempLayer);
        //        }
        //        var newBackgroundPoints = oldAllPoints.Except(newAllPoints).ToList(); //added background;
        //        if (newBackgroundPoints.Count > 0)
        //        {
        //            DrawBoard.DrawAt(newBackgroundPoints, BackgroundColor); //draw new background
        //        }
        //    }

        //    for (int i = 0; i < LayerObjects.Count; i++)
        //    {
        //        if (LayerObjects[i].DrawnPoints.Count == 0 || LayerObjects[i].Color != LayerObjects[i].NewColor)
        //        {
        //            DrawBoard.DrawAt(tempLayers[i], LayerObjects[i].NewColor);
        //            LayerObjects[i].Color = LayerObjects[i].NewColor;
        //            LayerObjects[i].DrawnPoints = tempLayers[i];
        //        }
        //        else
        //        {
        //            tempLayers[i] = tempLayers[i].Except(LayerObjects[i].DrawnPoints).ToList();
        //            DrawBoard.DrawAt(tempLayers[i], LayerObjects[i].NewColor);
        //        }
        //    }

        //}

    }
}
