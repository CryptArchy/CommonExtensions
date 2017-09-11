using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class ConcatTests
    {
        private IEnumerable<string> letters;
        private IEnumerable<int> numbers;

        [TestInitialize]
        public void SetUp()
        {
            letters = new List<String>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            numbers = Enumerable.Range(1, 26);
        }

        [TestMethod]
        public void Prepend_Test()
        {
            var original = new List<int>() { 1, 2, 3, 4, 5 };
            var expected = new List<int>() { 9, 1, 2, 3, 4, 5 };
            var actual = original.Prepend(9);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void Append_Test()
        {
            var original = new List<int>() { 1, 2, 3, 4, 5 };
            var expected = new List<int>() { 1, 2, 3, 4, 5, 9 };
            var actual = original.Append(9);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
