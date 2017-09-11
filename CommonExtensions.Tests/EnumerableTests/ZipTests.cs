using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class ZipTests
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
        public void CanZipShortest()
        {
            var actuals = letters.ZipShortest(Enumerable.Range(1, 4), (s, n) => s + n);
            var expecteds = new List<string>() { "A1", "B2", "C3", "D4" };
            Assert.IsTrue(expecteds.SequenceEqual(actuals));
                        
            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ZipShortest(letters, (z, n) => z + n));

            expecteds = new List<string>() { "AAAA", "BBBB", "CCCC", "DDDD" };
            actuals = letters.ZipShortest(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d) => a + b + c + d);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));
        }

        [TestMethod]
        public void CanZipShortestIndexed()
        {
            var actuals = letters.ZipShortest(Enumerable.Range(1, 4), (s, n, i) => s + n + i);
            var expecteds = new List<string>() { "A10", "B21", "C32", "D43" };
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ZipShortest(letters, (z, n, i) => z + n));

            expecteds = new List<string>() { "AAAA0", "BBBB1", "CCCC2", "DDDD3" };
            actuals = letters.ZipShortest(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d, i) => a + b + c + d + i);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));
        }

        [TestMethod]
        public void CanZipLongest()
        {
            var actuals = letters.Take(4).ZipLongest(Enumerable.Range(1, 3), (s, n) => s + n);
            var expecteds = new List<string>() { "A1", "B2", "C3", "D0" };
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ZipLongest(letters, (z, n) => z + n));

            expecteds = new List<string>() { "AAAA", "BBBB", "CCCC", "DDDD", "E" };
            actuals = letters.Take(5).ZipLongest(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d) => a + b + c + d);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));
        }

        [TestMethod]
        public void CanZipLongestIndexed()
        {
            var actuals = letters.Take(4).ZipLongest(Enumerable.Range(1, 3), (s, n, i) => s + n + i);
            var expecteds = new List<string>() { "A10", "B21", "C32", "D03" };
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ZipLongest(letters, (z, n, i) => z + n));

            expecteds = new List<string>() { "AAAA0", "BBBB1", "CCCC2", "DDDD3", "E4" };
            actuals = letters.Take(5).ZipLongest(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d, i) => a + b + c + d + i);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));
        }

        [TestMethod]
        public void CanZipEven()
        {
            var actuals = letters.Take(4).ZipEven(Enumerable.Range(1, 4), (s, n) => s + n);
            var expecteds = new List<string>() { "A1", "B2", "C3", "D4" };
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<InvalidOperationException>(() => letters.Take(4).ZipEven(Enumerable.Range(1, 2), (s, n) => s + n).Consume());
            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ZipShortest(letters, (z, n) => z + n));

            expecteds = new List<string>() { "AAAA", "BBBB", "CCCC", "DDDD" };
            actuals = letters.Take(4).ZipEven(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d) => a + b + c + d);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<InvalidOperationException>(() => letters.Take(5).ZipEven(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d) => a + b + c + d).Consume());
        }

        [TestMethod]
        public void CanZipEvenIndexed()
        {
            var actuals = letters.Take(4).ZipEven(Enumerable.Range(1, 4), (s, n, i) => s + n + i);
            var expecteds = new List<string>() { "A10", "B21", "C32", "D43" };
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<InvalidOperationException>(() => letters.Take(4).ZipEven(Enumerable.Range(1, 2), (s, n, i) => s + n).Consume());
            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ZipEven(letters, (z, n, i) => z + n));

            expecteds = new List<string>() { "AAAA0", "BBBB1", "CCCC2", "DDDD3" };
            actuals = letters.Take(4).ZipEven(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d, i) => a + b + c + d + i);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));

            TestExt.AssertThrows<InvalidOperationException>(() => letters.Take(5).ZipEven(letters.Take(4), letters.Take(4), letters.Take(4), (a, b, c, d, i) => a + b + c + d + i).Consume());
        }
    }
}
