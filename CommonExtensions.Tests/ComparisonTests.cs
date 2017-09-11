using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions.NumericExtensions;

namespace CommonExtensions.Tests
{
    [TestClass]
    public class ComparisonTests
    {
        [TestMethod]
        public void BetweenTests()
        {
            Assert.IsTrue((5).Between(0, 10));
            Assert.IsTrue((0).Between(0, 10));
            Assert.IsTrue((10).Between(0, 10));
            Assert.IsTrue((0).Between(0, 0));
            Assert.IsFalse((15).Between(0, 10));
            Assert.IsFalse((-15).Between(0, 10));
        }

        [TestMethod]
        public void WithinTests()
        {
            Assert.IsTrue((5).Within(0, 10));
            Assert.IsFalse((0).Within(0, 10));
            Assert.IsFalse((10).Within(0, 10));
            Assert.IsFalse((0).Within(0, 0));
            Assert.IsFalse((15).Within(0, 10));
            Assert.IsFalse((-15).Within(0, 10));
        }

        [TestMethod]
        public void BetweenMinTests()
        {
            Assert.IsTrue((5).BetweenMin(0, 10));
            Assert.IsTrue((0).BetweenMin(0, 10));
            Assert.IsFalse((10).BetweenMin(0, 10));
            Assert.IsFalse((0).BetweenMin(0, 0));
            Assert.IsFalse((15).BetweenMin(0, 10));
            Assert.IsFalse((-15).BetweenMin(0, 10));
        }

        [TestMethod]
        public void BetweenMaxTests()
        {
            Assert.IsTrue((5).BetweenMax(0, 10));
            Assert.IsFalse((0).BetweenMax(0, 10));
            Assert.IsTrue((10).BetweenMax(0, 10));
            Assert.IsFalse((0).BetweenMax(0, 0));
            Assert.IsFalse((15).BetweenMax(0, 10));
            Assert.IsFalse((-15).BetweenMax(0, 10));
        }

        [TestMethod]
        public void LesserOfTests()
        {
            Assert.AreEqual(0, (0).LesserOf(1));
            Assert.AreEqual(1, (2).LesserOf(1));
            Assert.AreEqual(-1, (0).LesserOf(-1));
            Assert.AreEqual(-1, (-1).LesserOf(0));
            Assert.AreEqual(1.4, (1.4).LesserOf(1.41));
        }

        [TestMethod]
        public void GreaterOfTests()
        {
            Assert.AreEqual(1, (0).GreaterOf(1));
            Assert.AreEqual(2, (2).GreaterOf(1));
            Assert.AreEqual(0, (0).GreaterOf(-1));
            Assert.AreEqual(0, (-1).GreaterOf(0));
            Assert.AreEqual(1.41, (1.4).GreaterOf(1.41));
        }

        [TestMethod]
        public void TypedBetweenTests()
        {
            Assert.IsTrue(((sbyte)2).Between((sbyte)1, (sbyte)3));
            Assert.IsTrue(((short)2).Between((short)1, (short)3));
            Assert.IsTrue((2).Between(1, 3));
            Assert.IsTrue((2L).Between(1L, 3L));

            Assert.IsTrue(((byte)2).Between((byte)1, (byte)3));
            Assert.IsTrue(((ushort)2).Between((ushort)1, (ushort)3));
            Assert.IsTrue((2u).Between(1u, 3u));
            Assert.IsTrue((2UL).Between(1UL, 3UL));

            Assert.IsTrue((2.1).Between(2.0, 2.2));
            Assert.IsTrue((2.1f).Between(2.0f, 2.2f));
            Assert.IsTrue((2.1M).Between(2.0M, 2.2M));

            Assert.IsTrue(('B').Between('A', 'C'));
            Assert.IsTrue(("B").Between("A", "C"));
        }
    }
}
