using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class TailsTests
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
        public void CanGenerateTails()
        {
            var expecteds = new List<List<string>>() { 
                new List<string>() { "A", "B", "C", "D" }, 
                new List<string>() { "B", "C", "D" }, 
                new List<string>() { "C", "D" }, 
                new List<string>() { "D" } };

            var actuals = letters.Take(4).Tails().Select(x => x.ToList()).ToList();
            for (int i = 0; i < expecteds.Count; i++)
            {
                Assert.IsTrue(expecteds[i].SequenceEqual(actuals[i]));
            }
        }
    }
}
