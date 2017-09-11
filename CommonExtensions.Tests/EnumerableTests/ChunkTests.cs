using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions;
using CommonExtensions.EnumerableExtensions;

namespace CommonExtensions.Tests.EnumerableTests
{
    [TestClass]
    public class ChunkTests
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
        public void CanChunkString()
        {
            var expecteds = new List<string>() { "004","009","012","030","007","010" };
            var actuals = "004009012030007010".Chunk(3);
            Assert.IsTrue(expecteds.SequenceEqual(actuals));
        }

        [TestMethod]
        public void CanChunkEvenly()
        {
            Assert.IsTrue(letters.Chunk(13).Count() == 2);
            Assert.IsTrue(letters.Chunk(13).First().Count() == 13);
            Assert.IsTrue(letters.Chunk(2).Count() == 13);
            Assert.IsTrue(letters.Chunk(2).First().Count() == 2);
        }

        [TestMethod]
        public void CanChunkOrderedProperly()
        {
            var expecteds = new List<List<int>>() { new List<int>() { 1, 2, 3 }, new List<int>() { 4, 5, 6 }, new List<int>() { 7, 8, 9 }, new List<int>() { 10, 11, 12 }, new List<int>() { 13, 14, 15 }, new List<int>() { 16, 17, 18 }, new List<int>() { 19, 20, 21 }, new List<int>() { 22, 23, 24 }, new List<int>() { 25, 26 } };
            var actuals = numbers.Chunk(3).Select(x => x.ToList()).ToList();

            for (int i = 0; i < expecteds.Count; i++)
            {
                var expected = expecteds[i];
                var actual = actuals[i];
                for (int j = 0; j < expected.Count; j++)
                {
                    Assert.AreEqual(expected[j], actual[j]);
                }
            }
        }

        [TestMethod]
        public void CanChunkWithRemainder()
        {
            Assert.IsTrue(letters.Chunk(3).Count() == 9);
            Assert.IsTrue(letters.Chunk(3).Last().Count() == 2);
            Assert.IsTrue(letters.Chunk(25).Count() == 2);
            Assert.IsTrue(letters.Chunk(25).Last().Count() == 1);
        }

        [TestMethod]
        public void CanChunkEdgeCases()
        {
            Assert.IsTrue(letters.Chunk(26).Count() == 1);
            Assert.IsTrue(letters.Chunk(30).Count() == 1);
            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).Chunk(3));
            TestExt.AssertThrows<ArgumentOutOfRangeException>(() => letters.Chunk(0));
            TestExt.AssertThrows<ArgumentOutOfRangeException>(() => letters.Chunk(-1));
        }

        [TestMethod]
        public void CanChunkStreamEvenly()
        {
            Assert.IsTrue(letters.ChunkStream(13).Select(x => x.ToArray()).ToArray().Length == 2);
            Assert.IsTrue(letters.ChunkStream(13).Select(x => x.ToArray()).ToArray()[0].Length == 13);
            Assert.IsTrue(letters.ChunkStream(2).Select(x => x.ToArray()).ToArray().Length == 13);
            Assert.IsTrue(letters.ChunkStream(2).Select(x => x.ToArray()).ToArray()[0].Length == 2);
        }

        [TestMethod]
        public void CanChunkStreamOrderedProperly()
        {
            var expecteds = new List<List<int>>() { new List<int>() { 1, 2, 3 }, new List<int>() { 4, 5, 6 }, new List<int>() { 7, 8, 9 }, new List<int>() { 10, 11, 12 }, new List<int>() { 13, 14, 15 }, new List<int>() { 16, 17, 18 }, new List<int>() { 19, 20, 21 }, new List<int>() { 22, 23, 24 }, new List<int>() { 25, 26 } };
            var actuals = numbers.ChunkStream(3).Select(x => x.ToList()).ToList();

            for (int i = 0; i < expecteds.Count; i++)
            {
                var expected = expecteds[i];
                var actual = actuals[i];
                for (int j = 0; j < expected.Count; j++)
                {
                    Assert.AreEqual(expected[j], actual[j]);
                }
            }
        }

        [TestMethod]
        public void CanChunkStreamWithRemainder()
        {
            Assert.IsTrue(letters.ChunkStream(3).Select(x => x.ToArray()).ToArray().Length == 9);
            Assert.IsTrue(letters.ChunkStream(3).Select(x => x.ToArray()).ToArray()[8].Length == 2);
            Assert.IsTrue(letters.ChunkStream(25).Select(x => x.ToArray()).ToArray().Length == 2);
            Assert.IsTrue(letters.ChunkStream(25).Select(x => x.ToArray()).ToArray()[1].Length == 1);
        }

        [TestMethod]
        public void CanChunkStreamEdgeCases()
        {
            Assert.IsTrue(letters.ChunkStream(26).Select(x => x.ToArray()).ToArray().Length == 1);
            Assert.IsTrue(letters.ChunkStream(30).Select(x => x.ToArray()).ToArray().Length == 1);
            TestExt.AssertThrows<ArgumentNullException>(() => ((IEnumerable<string>)null).ChunkStream(3).Consume());
            TestExt.AssertThrows<ArgumentOutOfRangeException>(() => letters.ChunkStream(0).Select(x => x.ToArray()).ToArray());
            TestExt.AssertThrows<ArgumentOutOfRangeException>(() => letters.ChunkStream(-1).Select(x => x.ToArray()).ToArray());
        }
    }
}
