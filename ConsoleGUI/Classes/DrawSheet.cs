using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Classes
{
    public static class Constants
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
        public List<IShape> PresentObjects;
        public List<char> PresentChars;
        public ConsoleColor ForegroundColor;
        public Pixel((int X, int Y) coord, IShape shapeObject, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var presentObjects = new List<IShape>();
            var presentChars = new List<char> { ' ' };
            Coord = coord;
            PresentChars = presentChars;
            PresentObjects = presentObjects;
            PresentObjects.Add(shapeObject);
            ForegroundColor = foregroundColor;
        }
        public Pixel((int X, int Y) coord, char character, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var presentObjects = new List<IShape>();
            var presentChars = new List<char> { ' ', character };
            Coord = coord;
            PresentChars = presentChars;
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
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ForegroundColor;
            }
                Console.Write(PresentChars[^1]);
            
        }
    }
    public class DrawSheet
    {

        public List<Pixel> Pixels;
        public List<IShape> SheetObjects;
        public IShape ActiveObject { get; set; }

        public DrawSheet()
        {
            var pixels = new List<Pixel>();
            var shapeObjects = new List<IShape>();

            Pixels = pixels;
            SheetObjects = shapeObjects;
        }

        public void Activate(IShape shapeObject)
        {
            ActiveObject = shapeObject;
            SheetObjects.Add(shapeObject);
            Console.BackgroundColor = shapeObject.Color;
            Console.CursorVisible = false;

        }
        public (ConsoleColor, ConsoleColor, int, int) SaveCurrent()
        {
            Console.CursorVisible = false;
            return (Console.BackgroundColor, Console.ForegroundColor, Console.CursorLeft, Console.CursorTop);
        }
        public void Restore((ConsoleColor background, ConsoleColor foreground, int cursorleft, int cursortop) settings)
        {
            Console.BackgroundColor = settings.background;
            Console.ForegroundColor = settings.foreground;
            Console.CursorLeft = settings.cursorleft;
            Console.CursorTop = settings.cursortop;
            Console.CursorVisible = true;
        }
        public void AddNew(IShape shapeObject)
        {
            var settings = SaveCurrent();

            ActiveObject = shapeObject;
            SheetObjects.Add(shapeObject);
            Console.BackgroundColor = shapeObject.Color;
            Console.CursorVisible = false;

            foreach (var coord in shapeObject.Coordinates)
            {
                DisplayAdd(coord);
            }

            Restore(settings);

        }

        public void DisplayAdd((int X, int Y) coord, char character = ' ')
        {
            var settings = SaveCurrent();
            bool pixelExists = Pixels.Select(p => p.Coord).ToList().Contains(coord);

            if (!pixelExists)
            {
                var pixel = new Pixel(coord, ActiveObject);
                Console.SetCursorPosition(coord.X, coord.Y);
                Console.Write(character);
                Pixels.Add(pixel);
                pixel.Refresh();
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
                }
                else
                {
                    bool activeObjectExists = pixel.PresentObjects.Exists(obj => obj == ActiveObject);
                    if (activeObjectExists) 
                    {
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
                Restore(settings);
            }        
        }
        public void DisplayRemove((int X, int Y) coord)
        {
           Console.BackgroundColor = ConsoleColor.Black;
           var pixel = Pixels.Find(p => p.Coord == coord);
            if (pixel.PresentChars.Count > 1)
            {
                pixel.PresentObjects.Remove(ActiveObject);
                pixel.Refresh();
            }
            else
            {
               pixel.PresentObjects.Clear();
               pixel.Refresh();
               Pixels.Remove(pixel);
            }
        
        }
        public void ToggleActiveObject(ConsoleKey key)
        {
            int index = SheetObjects.FindIndex(obj => obj == ActiveObject);
            if (key == ConsoleKey.Tab)
            {
                if (index != SheetObjects.Count-1)
                {
                    ActiveObject = SheetObjects[index + 1];
                }
                else
                {
                    ActiveObject = SheetObjects[0];
                }
            }
        }
        public void Move(IShape shape, ConsoleKey key)
        {
            var oldCoordinates = shape.Coordinates;
            var newCoordinates = new List<(int X, int Y)>();
            var keptCoordinates = new List<(int X, int Y)>();
            var removeCoordinates = new List<(int X, int Y)>();
            var drawCoordinates = new List<(int X, int Y)>();
            int xi = 0;
            int yi = 0;

            if (key == ConsoleKey.DownArrow) yi = +1;
            if (key == ConsoleKey.RightArrow) xi = +1;
            if (key == ConsoleKey.LeftArrow) xi = -1;
            if (key == ConsoleKey.UpArrow) yi = -1;

            foreach (var c in oldCoordinates)
            {
                newCoordinates.Add((c.X + xi, c.Y + yi));
            }
            shape.Coordinates = newCoordinates;

            keptCoordinates = oldCoordinates.Intersect(newCoordinates).ToList();
            drawCoordinates = newCoordinates.Except(keptCoordinates).ToList();
            removeCoordinates = oldCoordinates.Except(keptCoordinates).ToList();
            foreach (var p in removeCoordinates)
            {
                DisplayRemove(p);
            }
            foreach (var p in drawCoordinates)
            {
                DisplayAdd(p);
            }
            
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
        public static (int X, int Y) PointFromCursor(out ConsoleKey key, bool dynamic = false)
        {
            key = ConsoleKey.Spacebar;

            if (!dynamic)
            {
                while (key != ConsoleKey.Enter)
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                    if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                    if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                    if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
                }

            }
            else
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
            }
            return (Console.CursorLeft - 1, Console.CursorTop);
        }
        public static (int X, int Y) PointFromCursor(bool dynamic = false)
        {
            ConsoleKey? key = null;

            if (!dynamic)
            {
                while (key != ConsoleKey.Enter)
                {
                    key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                    if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                    if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                    if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
                }

            }
            else
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow && Console.CursorLeft >= 1) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Constants.XEnd) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop >= 1) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow && Console.CursorTop <= Constants.YEnd) Console.CursorTop++;
            }
            return (Console.CursorLeft - 1, Console.CursorTop);
        }
        public void KeyOptions()
        {
            var coloroptions = new List<string>
            {
                "Press Hotkeys[hotkey]",
                "-for all commands-",
                "Confirm command [Enter]",
                "",
                "Change Color:",
                "Default Black[d]",
                "White [w]",
                "Blue [b]",
                "Red [r]",
                "Yellow [y]",
                "Green [g]",
                "",
                "New Object:",
                "Circle [c]",
                "Rectangle [e]",
                "Line [l]",
                "",
                "Change Current Object",
                "",
                "Move [m]",
                "Fill/Unfill [f]",
                "Rescale [s]",
                "Move object up stack [+]",
                "Move object back [-]",
                "Name [n]",
                "",
                "Change Object[Tab]",
                "",
            };
        }
    }
}
