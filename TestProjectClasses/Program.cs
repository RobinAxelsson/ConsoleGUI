using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjectClasses
{
    public class Program
    {
        public static void Main()
        {
        }
    }
    class Shape
    {
        public int X;
        public int Y;

        public virtual string String()
        {
            return "Shape!";
        }
    }
    class Triangle : Shape
    {
        public int Side;

        public override string String()
        {
            return "Triangle!";
        }
    }
    abstract class IShape
    {
        public int X = 1;
        public int Y = 1;

        abstract public string String();
    }
    class Square : IShape
    {
        public override string String() { return "Square!"; }
    }
    class Tree : IShape
    {
        public override string String() { return "Tree!"; }
    }
    [TestClass]

    public class ExampleTest
    {
        [TestMethod]
        public void TestingAbstract()
        {
            var square = new Square() {X = 1, Y = 1 };
            var tree = new Tree();

            var iShapes = new List<IShape> { square, tree };
            var strings = new List<string>();

            string a = square.String();
            string b = tree.String();

            foreach (var s in iShapes)
            {
                strings.Add(s.String());
            }

        }
        [TestMethod]
        public void TestingClasses()
        {
            var shape = new Shape()
            {
                X = 1,
                Y = 2
            };
            shape.X = 5;
            int a = shape.Y;

            var triangle = new Triangle() { X = 1, Y = 5, Side = 2 };

            var shapes = new List<Shape> { shape, triangle };
            var strings = new List<string>();

            foreach (var s in shapes)
            {
                strings.Add(s.String());
            }

        }
    }
}
