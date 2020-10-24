using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<ShapeObject> PresentObjects;
        public char Character = ' ';
        public ConsoleColor ForegroundColor;
        public Pixel((int X, int Y) coord,  ShapeObject shapeObject, char character = ' ', ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var currentObjects = new List<ShapeObject>();
            Coord = coord;
            Character = character;
            PresentObjects = currentObjects;
            PresentObjects.Add(shapeObject);
            ForegroundColor = foregroundColor;
        }
        public Pixel((int X, int Y) coord, char character, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var presentObjects = new List<ShapeObject>();
            Coord = coord;
            Character = character;
            PresentObjects = presentObjects;
            ForegroundColor = foregroundColor;
        }
        public void Refresh()
        {
            Console.SetCursorPosition(Coord.X, Coord.Y);
            if (PresentObjects.Count != 0)
            {
                Console.BackgroundColor = PresentObjects[^1].Color;
            }
            if (Character != ' ')
            {
                Console.ForegroundColor = ForegroundColor;
            }
            Console.Write(Character);
        }
    }
    public class DrawSheet
    {
        public List<Pixel> Pixels;
        public List<ShapeObject> SheetObjects;
        private ShapeObject ActiveObject { get; set; }

        public DrawSheet()
        { 
            var pixels = new List<Pixel>();
            var shapeObjects = new List<ShapeObject>();
            var activeObject = new ShapeObject();

            Pixels = pixels;
            SheetObjects = shapeObjects;
            ActiveObject = activeObject;
        }

        public void Activate(ShapeObject shapeObject)
        {
            ActiveObject = shapeObject;
            SheetObjects.Add(shapeObject);
            Console.BackgroundColor = shapeObject.Color;
            Console.CursorVisible = false;
        }
        public void StringPixels((int X, int Y) coord, string prompt, ConsoleColor color, bool center)
        {
            if(center)
            {
                coord = ((coord.X - prompt.Length), coord.Y);
            }
            foreach (char c in prompt)
            {
                CharPixel(coord, c, color);
                coord = ((coord.X + 1), coord.Y);
            }
        }
        public void CharPixel((int X, int Y) coord, char character, ConsoleColor color)
        {
            var charPixel = new Pixel(coord, character, color);
            Pixels.Add(charPixel);
            charPixel.Refresh();
            
        }
        public void CharPixel(List<(int X, int Y)> coordinates, char character, ConsoleColor color)
        {
            foreach (var coord in coordinates)
            {
                var charPixel = new Pixel(coord, character, color);
                Pixels.Add(charPixel);
                charPixel.Refresh();
            }
        }
        public void DisplayAdd((int X, int Y) coord, char character = ' ')
        {
            bool pixelExists = Pixels.Select(p => p.Coord).ToList().Contains(coord);

            if (!pixelExists)
            {
                var pixel = new Pixel(coord, ActiveObject, character);
                Console.SetCursorPosition(coord.X, coord.Y);
                Console.Write(character);
                Pixels.Add(pixel);
                pixel.Refresh();
                return;
            }
            else
            {
                var pixel = Pixels.Find(p => p.Coord == coord);

                if (pixel.PresentObjects.Count == 0)
                {
                    pixel.PresentObjects.Add(ActiveObject);
                    pixel.Refresh();
                }

                var pixelObject = pixel.PresentObjects[^1];
                    
                bool isActiveObjectTop = pixelObject == ActiveObject;

                if (isActiveObjectTop)
                {
                    return;
                }
                else
                {
                    bool activeObjectExists = pixel.PresentObjects.Exists(obj => obj == ActiveObject);
                    if (activeObjectExists) 
                    {
                        return;
                    }
                    else //What index? Which order. if it not exists it needs to be ordered;
                    {
                        var indexHits = new List<int>();
                        int pixelIndex = 0;

                        for (int i = 0; i < SheetObjects.Count; i++)
                        {
                            if (SheetObjects[i] == pixel.PresentObjects[pixelIndex])
                            {
                                indexHits.Add(i);
                                pixelIndex++;
                            }
                        }

                        int activeObjectIndex = SheetObjects.FindIndex(obj => obj == ActiveObject);

                        bool isNewObjectAbove = indexHits.TrueForAll(i => i < activeObjectIndex);
                        if (isNewObjectAbove)
                        {
                            pixel.PresentObjects.Add(ActiveObject);
                            pixel.Refresh();
                            return;
                        }
                        else //insert object at index
                        {
                            bool isNewObjectLowest = activeObjectIndex == 0;
                            
                            if (isNewObjectLowest)
                            {
                                pixel.PresentObjects.Insert(0, ActiveObject);
                            }
                            else
                            {
                                int iInsert = indexHits.FindIndex(i => i > activeObjectIndex);
                                pixel.PresentObjects.Insert(iInsert, ActiveObject);
                            }                            
                        }
                    }
                }
            }        
        }
        public void DisplayRemove((int X, int Y) coord, char character = ' ')
        {
            var pixel = Pixels.Find(p => p.Coord == coord);
            pixel.PresentObjects.Remove(ActiveObject);
            pixel.Refresh();
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
                    CharPixel(rowLines[i][j], '─', color);
                }
            }
            for (int i = 1; i < columnLines.Count - 1; i++) //middle columns
            {
                for (int j = 0; j < columnLines[i].Count; j += 4)
                {
                    CharPixel(columnLines[i][j], '│', color);
                }
            }

            CharPixel(intersectingPoints, '\u0004', color);//diamonds
            CharPixel(frameOrigo, '┌', color);
            CharPixel((frameEndpoint.X, frameOrigo.Y), '┐', color);
            CharPixel((frameOrigo.X, frameEndpoint.Y), '└', color);
            CharPixel(frameEndpoint, '┘', color);
        }
    }
}
