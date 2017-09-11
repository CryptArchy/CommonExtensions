using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class PermutationsTests
    {
        [TestMethod]
        public void Permutations_Test()
        {
            var original = new List<int>() { 1, 2, 3 };
            var expected = new List<List<int>>() { 
                new List<int>() {1, 2, 3}, 
                new List<int>() {1, 3, 2},
                new List<int>() {2, 1, 3},
                new List<int>() {2, 3, 1},
                new List<int>() {3, 1, 2},
                new List<int>() {3, 2, 1}
            };
            var actual = original.Permutations().ToList();

            for (int i = 0; i < expected.Count; i++)
            {
                var exp = expected[i];
                var act = actual[i];
                Assert.IsTrue(exp.SequenceEqual(act));
            }
            
        }
    }
}
