using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using System.Reflection;
using System.Data;
using System.Linq;

namespace ConsoleGUI
{
    public class Layerobject
    {
        public enum Type
        {
            Box,
            Line,
            Circle
        }

        public List<(int X, int Y)> Points;
        public Type Shape { get; }
        public ConsoleColor color { get; set; } = ConsoleColor.White;
        public (int X, int Y) Point1;
        public (int X, int Y) Point2;
        public bool FillTrue;
        
        public int GetLayerIndex()
        {
            return Interface.Layers.FindIndex(l => l == Points);
        }
        public Layerobject((int X, int Y) point1, (int X, int Y) point2, Type shape)
        {
            Shape = shape;
            Point1 = point1;
            Point2 = point2;
            Points = PointGen(shape, point1, point2);
            Interface.Layers.Add(Points);            
        }

        public List<(int X, int Y)> PointGen(Type shape, (int X, int Y)point1, (int X, int Y) point2)
        {
        var points = new List<(int X, int Y)>();
        if (shape == Type.Box) points = Geometry.RectanglePts(point1, point2);
        if (shape == Type.Line) points = Geometry.LinePtPt(point1, point2);
        if (shape == Type.Circle) points = Geometry.CircleWithCenter(point1, point2);
            return points;
        }

        public void UnFill()
        {
            Points = PointGen(Shape, Point1, Point2);
            FillTrue = false;
        }
        public void Fill()
        {
            var Ys = Point.ToYs(Points);

            int Ymin = Ys.Min();
            int Ymax = Ys.Max();

            var yLevels = new List<List<(int X, int Y)>>();

            for (int i = Ymin; i <= Ymax; i++)
            {
                var newList = new List<(int X, int Y)>();
                newList = Points.Where(p => p.Y == i).ToList();
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
                    Points.Add((x, y));
                }
            }
            Points = Points.Distinct().ToList();
            FillTrue = true;
        }

    }
    public class Interface
    {
        public static List<List<(int X, int Y)>> Layers;
        public List<int> Priority;
        
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        public Interface()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.BufferHeight = Console.WindowHeight + 3;
            Console.SetCursorPosition(DrawBoard.XEnd / 2, DrawBoard.YEnd / 2);

            DrawBoard.Frame();
        }

        public static ConsoleColor? ColorChoice(ConsoleKey key)
        {

            int X = Console.CursorLeft;
            int Y = Console.CursorTop;
            Console.CursorVisible = false;

            var coloroptions = new List<string>
            {
                "Press key to change color,",
                "Draw with spacebar",
                "Default Black[d]",
                "White [w]",
                "Blue [b]",
                "Red [r]",
                "Yellow [y]",
                "Green [g]"
            };

            DrawBoard.LinesAt(70, 0, coloroptions);

            Console.CursorLeft = X;
            Console.CursorTop = Y;
            Console.CursorVisible = true;

            if (key == ConsoleKey.W) return ConsoleColor.White;
            if (key == ConsoleKey.D) return ConsoleColor.Black;
            if (key == ConsoleKey.B) return ConsoleColor.Blue;
            if (key == ConsoleKey.R) return ConsoleColor.Red;
            if (key == ConsoleKey.Y) return ConsoleColor.Yellow;
            if (key == ConsoleKey.G) return ConsoleColor.Green;
            else return null;
        }

        public void TrackCursorNumbers()
        {

            ConsoleKey? key = null;

            int left; int top;

            while (key != ConsoleKey.Enter)
            {
                left = Console.CursorLeft;
                top = Console.CursorTop;
                DrawBoard.StringAt(2, 60, $"{left}, {top}");
                Console.SetCursorPosition(left, top);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && Console.CursorLeft > 0) Console.CursorLeft--;
                if (key == ConsoleKey.RightArrow && Console.CursorLeft <= Console.BufferWidth - 1) Console.CursorLeft++;
                if (key == ConsoleKey.UpArrow && Console.CursorTop > 0) Console.CursorTop--;
                if (key == ConsoleKey.DownArrow) Console.CursorTop++;
            }
        }
    }
}
