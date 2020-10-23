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
    }
}
