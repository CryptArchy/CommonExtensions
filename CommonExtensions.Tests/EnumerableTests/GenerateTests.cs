using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class GenerateTests
    {
        [TestMethod]
        public void Generate_Test()
        {
            var actual = EnumerableExt.Generate(0, x => x + 2).Take(6);
            var expected = new List<int>() { 0, 2, 4, 6, 8, 10 };
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
