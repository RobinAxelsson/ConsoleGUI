using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleGUI
{
    [TestClass]

    public class ExampleTest
    {
        [TestInitialize]
        public void TestInit()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestMethod]
        public void DivideByZero()
        {
            try
            {
                int zero = 0;
                int result = 5 / zero;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Attempted to divide by zero.");
            }
        }
        [TestMethod]
        public void Circle_GetPoints()
        {
            int X1 = 0;
            int Y1 = 0;
            int X2 = 3;
            int Y2 = 4;

            (int X, int Y) point1 = (X1, Y1);
            (int X, int Y) point2 = (X2, Y2);

            var points = ShapeObject.CircleWithCenter(point1, point2);

        }
        [TestMethod]
        public void Fill_GetPointsBetween()
        {
            double radius = 10;
            var Xs = new List<double>();
            var Ys = new List<double>();
            ShapeObject.CircleEdgeXYsDouble(out Xs, out Ys, radius);


        }
    }
}
