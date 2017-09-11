using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class NestedLoopsTests
    {
        [TestMethod]
        public void NestedLoops_Test()
        {
            var expected = new List<int>() { 0, 1, 2, 3, 4, 5 };
            var actual = new List<int>();

            var loopCounts = new List<int>() { 1, 2, 3 };
            var i = 0;
            Action action = () => actual.Add(i++);
            var actions = action.NestedLoops(loopCounts);
            actions.ForEach(a => a.Invoke());

            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
