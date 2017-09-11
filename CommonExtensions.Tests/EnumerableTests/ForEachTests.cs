using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class ForEachTests
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
        public void ForEach_Test()
        {
            var expected = new List<string>(26);
            foreach (var s in letters) { expected.Add(s.ToLower()); }
            var actual = new List<string>(26); 
            letters.ForEach(s => actual.Add(s.ToLower()));
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
