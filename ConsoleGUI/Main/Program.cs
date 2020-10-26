using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using ConsoleGUI.Classes;

namespace ConsoleGUI
{
    //using static LayerObject.ShapeType;
    using static ConsoleColor;
    using static ConsoleKey;
    public class Program
    {
        public static void Main()
        {
            Settings.SmallWindow();
            //Settings.FullsizeWindow();

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.CursorVisible = false;
            var sheet = new DrawSheet();
            sheet.Grid(4,4);

     

            var redDots = new FreeForm((10, 10), (11, 10), Red);

            sheet.AddNew(redDots);

            var blueDots = new FreeForm((12, 10), (13, 10), Blue);

            sheet.AddNew(blueDots);

            var greenDots = new FreeForm((12, 10), (13, 10), Green);

            sheet.AddNew(greenDots);

            var circle = new Circle((12, 10), (15, 15), Yellow);

            sheet.AddNew(circle);

            ConsoleKey key = Spacebar;

            while (key != Enter)
            {
                sheet.Move(key);
                sheet.ShiftLayer(key);
                sheet.ToggleActiveObject(key);
                sheet.Scale(key);

                key = Console.ReadKey(true).Key;
            }

        }



        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

    }
}

    
