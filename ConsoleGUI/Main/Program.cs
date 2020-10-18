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

namespace ConsoleGUI
{
    public class Program
    {
        public static void Main()
        {
            // We need this to make sure we can always use periods for decimal points.
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            
            ConsoleColor color;
            Console.WindowHeight = 50;
            Console.WindowWidth = Console.BufferWidth;
            Console.SetCursorPosition(Console.BufferWidth / 2, Console.WindowHeight / 2);

            while (true)
            {
            var points = Action.DynShape(Action.Shape.Circle, out color);
                points = Action.Fill(points);
            Action.Move(color, points);
            }
        }    
        
       
        
    }
}
