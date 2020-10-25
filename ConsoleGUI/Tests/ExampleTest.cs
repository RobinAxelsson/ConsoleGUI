using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleGUI
{
    class Shape
    {
        public int X;
        int Y;
    }
    class Triangle : Shape
    {

    }

    [TestClass]

    public class ExampleTest
    {

        [TestMethod]
        public void TestingClasses()
        {
            var shape = new Shape();


        }
    }
}
