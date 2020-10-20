using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI
{

    public class LayerHandler
    {
        public List<LayerObject> LayerObjects { set; get; }
        public ConsoleColor BackgroundColor = ConsoleColor.Black;

        public LayerHandler()
        {
            var layerObjects = new List<LayerObject>();
            LayerObjects = layerObjects;
        }

        public LayerObject CreateLayerObject(LayerObject.ShapeType shape, (int X, int Y) point1, (int X, int Y) point2, ConsoleColor color = ConsoleColor.White)
        {
            var newObject = new LayerObject(point1, point2, shape, color);

            LayerObjects.Add(newObject);
            return newObject;
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
