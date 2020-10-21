using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGUI
{
    class TextBody
    {
        public (int X, int Y) Point1;
        public string Prompt;
        public TextBody((int X, int Y) point1, string prompt)
        {
            Point1 = point1;
            Prompt = prompt;

            Console.CursorVisible = false;
            var cursorPos = DrawBoard.SaveCursor();
            var colorSave = DrawBoard.SaveColors();

            Console.SetCursorPosition(point1.X, point1.Y);
            Console.WriteLine(prompt);

            DrawBoard.ResetColors(colorSave);
            DrawBoard.ResetCursor(cursorPos);
            Console.CursorVisible = true;
        }
        public void Erase()
        {
            Console.CursorVisible = false;
            var cursorPos = DrawBoard.SaveCursor();
            var colorSave = DrawBoard.SaveColors();

            Console.SetCursorPosition(Point1.X, Point1.Y);
            foreach (char x in Prompt)
            {
                Console.Write(' ');
            }
            DrawBoard.ResetColors(colorSave);
            DrawBoard.ResetCursor(cursorPos);
            Console.CursorVisible = true;
        }
    }
}
