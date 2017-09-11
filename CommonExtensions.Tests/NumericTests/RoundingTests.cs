using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions.NumericExtensions;

namespace CommonExtensions.Tests.NumericTests
{
    [TestClass]
    public class RoundingTests
    {
        [TestMethod]
        public void RoundHigherMultipleTests()
        {
            Assert.AreEqual(0, (0).RoundHigherMultiple(3));
            Assert.AreEqual(3, (1).RoundHigherMultiple(3));
            Assert.AreEqual(3, (2).RoundNearestMultiple(3));
            Assert.AreEqual(3, (3).RoundNearestMultiple(3));
            Assert.AreEqual(6, (4).RoundHigherMultiple(3));

            Assert.AreEqual((sbyte)24, ((sbyte)13).RoundHigherMultiple(12));
            //Assert.AreEqual((short)38, ((short)37).RoundHigherMultiple(2));
            Assert.AreEqual(150, (139).RoundHigherMultiple(50));
            Assert.AreEqual(200L, (139L).RoundHigherMultiple(100L));

            Assert.AreEqual(150, (139.6).RoundHigherMultiple(50));
            Assert.AreEqual(150, (139.6F).RoundHigherMultiple(50));
            Assert.AreEqual(150, (139.6M).RoundHigherMultiple(50));

            Assert.AreEqual((byte)24, ((byte)13).RoundHigherMultiple(12));
            Assert.AreEqual((ushort)38, ((ushort)37).RoundHigherMultiple(2));
            Assert.AreEqual(150u, (139u).RoundHigherMultiple(50u));
            Assert.AreEqual(200uL, (139uL).RoundHigherMultiple(100uL));

            Assert.AreEqual(0, (-4).RoundHigherMultiple(50));
            Assert.AreEqual(-10, (-19).RoundHigherMultiple(10));
        }

        [TestMethod]
        public void RoundLowerMultipleTests()
        {
            Assert.AreEqual(0, (0).RoundLowerMultiple(3));
            Assert.AreEqual(0, (1).RoundLowerMultiple(3));
            Assert.AreEqual(0, (2).RoundLowerMultiple(3));
            Assert.AreEqual(3, (3).RoundLowerMultiple(3));
            Assert.AreEqual(3, (4).RoundLowerMultiple(3));

            Assert.AreEqual((sbyte)12, ((sbyte)13).RoundLowerMultiple(12));
            //Assert.AreEqual((short)36, ((short)37).RoundLowerMultiple(2));
            Assert.AreEqual(100, (139).RoundLowerMultiple(50));
            Assert.AreEqual(100L, (139L).RoundLowerMultiple(100L));

            Assert.AreEqual(100, (139.6).RoundLowerMultiple(50));
            Assert.AreEqual(100, (139.6F).RoundLowerMultiple(50));
            Assert.AreEqual(100, (139.6M).RoundLowerMultiple(50));

            Assert.AreEqual((byte)12, ((byte)13).RoundLowerMultiple(12));
            Assert.AreEqual((ushort)36, ((ushort)37).RoundLowerMultiple(2));
            Assert.AreEqual(100u, (139u).RoundLowerMultiple(50u));
            Assert.AreEqual(100uL, (139uL).RoundLowerMultiple(100uL));

            Assert.AreEqual(-50, (-4).RoundLowerMultiple(50));
            Assert.AreEqual(-20, (-19).RoundLowerMultiple(10));
        }

        [TestMethod]
        public void RoundNearestMultipleTests()
        {
            Assert.AreEqual(0, (0).RoundNearestMultiple(3));
            Assert.AreEqual(0, (1).RoundNearestMultiple(3));
            Assert.AreEqual(3, (2).RoundNearestMultiple(3));
            Assert.AreEqual(3, (3).RoundNearestMultiple(3));
            Assert.AreEqual(3, (4).RoundNearestMultiple(3));
            
            Assert.AreEqual((sbyte)12, ((sbyte)13).RoundNearestMultiple(12));
            //Assert.AreEqual((short)38, ((short)37).RoundNearestMultiple(2));
            Assert.AreEqual(150, (139).RoundNearestMultiple(50));
            Assert.AreEqual(100L, (139L).RoundNearestMultiple(100L));

            Assert.AreEqual(150, (139.6).RoundNearestMultiple(50));
            Assert.AreEqual(150, (139.6F).RoundNearestMultiple(50));
            Assert.AreEqual(150, (139.6M).RoundNearestMultiple(50));

            Assert.AreEqual((byte)12, ((byte)13).RoundNearestMultiple(12));
            Assert.AreEqual((ushort)38, ((ushort)37).RoundNearestMultiple(2));
            Assert.AreEqual(150u, (139u).RoundNearestMultiple(50u));
            Assert.AreEqual(100uL, (139uL).RoundNearestMultiple(100uL));

            Assert.AreEqual(0, (-4).RoundNearestMultiple(50));
            Assert.AreEqual(-20, (-19).RoundNearestMultiple(10));
        }

        [TestMethod]
        public void RoundToTests()
        {
            Assert.AreEqual(0, (0D).RoundTo(5));
            Assert.AreEqual(0M, (0M).RoundTo(5));

            Assert.AreEqual(3.12346, (3.123456D).RoundTo(5));
            Assert.AreEqual(3.12346M, (3.123456M).RoundTo(5));
            Assert.AreEqual(3.65432, (3.654321D).RoundTo(5));
            Assert.AreEqual(3.65432M, (3.654321M).RoundTo(5));

            Assert.AreEqual(3.123, (3.123456D).RoundTo(3));
            Assert.AreEqual(3.123M, (3.123456M).RoundTo(3));
            Assert.AreEqual(3.654, (3.654321D).RoundTo(3));
            Assert.AreEqual(3.654M, (3.654321M).RoundTo(3));

            Assert.AreEqual(3.1, (3.123456D).RoundTo(1));
            Assert.AreEqual(3.1M, (3.123456M).RoundTo(1));
            Assert.AreEqual(3.7, (3.654321D).RoundTo(1));
            Assert.AreEqual(3.7M, (3.654321M).RoundTo(1));

            Assert.AreEqual(3.0, (3.123456D).RoundTo(0));
            Assert.AreEqual(3.0M, (3.123456M).RoundTo(0));
            Assert.AreEqual(4.0, (3.654321D).RoundTo(0));
            Assert.AreEqual(4.0M, (3.654321M).RoundTo(0));

            Assert.AreEqual(1, (0.5D).RoundTo(0));
            Assert.AreEqual(1M, (0.5M).RoundTo(0));

            Assert.AreEqual(-3.1, (-3.123456D).RoundTo(1));
            Assert.AreEqual(-3.1M, (-3.123456M).RoundTo(1));
            Assert.AreEqual(-3.7, (-3.654321D).RoundTo(1));
            Assert.AreEqual(-3.7M, (-3.654321M).RoundTo(1));
            
            Assert.AreEqual("0.00000", (0D).RoundTo(5).ToString("N5"));
            Assert.AreEqual("0.123460000", (0.123456789D).RoundTo(5).ToString("N9"));
        }
    }
}
