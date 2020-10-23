using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGUI.Classes
{
    public static class Fixed
    {
        //Largest windowwidth = 200
        //Largest windowheight = 71

        public const int XStart = 5;
        public const int YStart = 3;
        public const int XEnd = 150;
        public const int YEnd = 60;
        public const ConsoleColor BackgroundColor = ConsoleColor.Black;
    }
  
    public class Pixel
    {
        public (int X, int Y) Coord;
        public List<ConsoleColor> Colors;
        public char Character = ' ';
        //public int ActiveColor;
        public Pixel((int X, int Y) coord, char character = ' ', ConsoleColor color = ConsoleColor.White)
        {
            var colors = new List<ConsoleColor>();
            Coord = coord;
            Character = character;
            Colors = colors;
            Colors.Add(color);
            //ActiveColor = 0;
        }
    }
    public class DrawSheet
    {
        public List<Pixel> Pixels;
        public List<ShapeObject> ShapeObjects;
        public List<ConsoleColor> LayerColors;
        public int ActiveIndex;

        public DrawSheet()
        { 
            var pixels = new List<Pixel>();
            var shapeObjects = new List<ShapeObject>();
            var layerColors = new List<ConsoleColor>();
            layerColors.Add(Fixed.BackgroundColor);

            Pixels = pixels;
            ShapeObjects = shapeObjects;
            LayerColors = layerColors;
            ActiveIndex = 0;
        }

        public void DisplayAdd((int X, int Y) coord, char character = ' ') //color sets from the drawhsheet
        {
            bool pixelExists = Pixels.Select(p => p.Coord).ToList().Contains(coord);

            if (!pixelExists)
            {
                var pixel = new Pixel(coord, character, LayerColors[ActiveIndex]);
                Console.SetCursorPosition(coord.X, coord.Y);
                Console.Write(character);
                Pixels.Add(pixel);
                return;
            }
            else
            {
                var pixel = Pixels.Find(p => p.Coord == coord);
                var pixelColor = pixel.Colors[^1];
                var layerColor = LayerColors[ActiveIndex];
                bool isColorEqual = pixelColor == layerColor;

                if (isColorEqual)
                {
                    return;
                }
                else
                {
                    int trueColorIndex = LayerColors.FindLastIndex(layerColor => layerColor == pixelColor);
                    bool isCurrentColorHigherIndex = ActiveIndex>=trueColorIndex;
                    if (!isCurrentColorHigherIndex)
                    {
                        return;
                    }
                    else
                    {
                        pixel.Colors.Add(layerColor);
                        

                    }
                }
            }
        
        }

        public void Grid(int rows, int columns, ConsoleColor color = ConsoleColor.DarkGray)
        {
            int xOrigo = Constants.XStart - 1;
            int yOrigo = Constants.YStart - 1;
            (int X, int Y) frameOrigo = (xOrigo, yOrigo);

            int xLength = (Constants.XEnd + 1 - xOrigo);
            int yLength = (Constants.YEnd + 1 - yOrigo);

            int xSpacingForColumns = xLength / columns;
            int ySpacingForRows = yLength / rows;

            xLength = xSpacingForColumns * columns;
            yLength = ySpacingForRows * rows;

            (int X, int Y) frameEndpoint = (xLength + xOrigo, yLength + yOrigo);

            var allRowPoints = new List<(int X, int Y)>();
            var rowLines = new List<List<(int X, int Y)>>();

            for (int i = 0; i <= rows; i++)
            {
                var rowLine = new List<(int X, int Y)>();
                for (int x = frameOrigo.X; x <= frameEndpoint.X; x++)
                {
                    var rowPoint = (x, ySpacingForRows * i + yOrigo);
                    rowLine.Add(rowPoint);
                    allRowPoints.Add(rowPoint);
                }
                rowLines.Add(rowLine);
            }

            var allColumnPoints = new List<(int X, int Y)>();
            var columnLines = new List<List<(int X, int Y)>>();
            for (int i = 0; i <= columns; i++)
            {
                var columnLine = new List<(int X, int Y)>();
                for (int y = frameOrigo.Y; y <= frameEndpoint.Y; y++)
                {
                    var rowPoint = (xSpacingForColumns * i + xOrigo, y);
                    columnLine.Add(rowPoint);
                    allColumnPoints.Add(rowPoint);
                }
                columnLines.Add(columnLine);
            }


            var intersectingPoints = allRowPoints.Intersect(allColumnPoints).ToList();

            for (int i = 1; i < rowLines.Count - 1; i++) //middle rows
            {
                for (int j = 0; j < rowLines[i].Count; j += 8)
                {
                    newCharPoint(rowLines[i][j], '─', color);
                }
            }
            for (int i = 1; i < columnLines.Count - 1; i++) //middle columns
            {
                for (int j = 0; j < columnLines[i].Count; j += 4)
                {
                    newCharPoint(columnLines[i][j], '│', color);
                }
            }

            newCharPoint(intersectingPoints, '\u0004');//diamonds
            newCharPoint(frameOrigo, '┌', color);
            newCharPoint((frameEndpoint.X, frameOrigo.Y), '┐', color);
            newCharPoint((frameOrigo.X, frameEndpoint.Y), '└', color);
            newCharPoint(frameEndpoint, '┘', color);
        }
    }
}
