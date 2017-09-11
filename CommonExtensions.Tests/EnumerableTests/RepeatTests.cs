using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class RepeatTests
    {
        [TestMethod]
        public void Repeat_Test()
        {
            var original = new List<int>() { 1, 2, 3 };
            var expected = new List<int>() { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };
            var actual = original.Repeat(4);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void Repeat_Buffering_Test()
        {
            var a = 0;
            var original = new List<int>() {++a, ++a, ++a };
            var expected = new List<int>() { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };
            var actual = original.Repeat(4);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
