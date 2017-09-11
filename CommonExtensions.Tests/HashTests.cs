using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions.HashExtensions;
using System.Collections.Generic;
using System.IO;

namespace CommonExtensions.Tests
{
    [TestClass]
    public class HashTests
    {
        [TestMethod]
        public void TestHashing()
        {
            var x = 0;
            var h1 = x.Hash();
            var h2 = x.Hash(1, 2, 3, 4, 5);
            Assert.AreNotEqual(x, h1);
            Assert.AreNotEqual(x, h2);
            Assert.AreNotEqual(h1, h2);
        }        

        [TestMethod]
        public void TestObjectHashCollisions()
        {
            var set = new Dictionary<int, string>();
            var controlSet = new Dictionary<int, string>();
            var collisions = new List<string>();
            var controlCollisions = new List<string>();

            var file = File.OpenText(@"..\..\SampleTexts\english-words.95");

            while (!file.EndOfStream)
            {
                var word = file.ReadLine();
                var hash = word.Hash();
                var controlHash = word.GetHashCode();
                
                if (set.ContainsKey(hash))
                    collisions.Add(set[hash] + " : " + word + " : " + hash.ToString());
                else
                    set.Add(hash, word);


                if (controlSet.ContainsKey(controlHash))
                    controlCollisions.Add(controlSet[controlHash] + " : " + word + " : " + controlHash.ToString());
                else
                    controlSet.Add(controlHash, word);
            }

            //I consider it a win if the custom hasher can beat the built in .NET hasher
            Assert.IsTrue(collisions.Count < controlCollisions.Count, "Count: " + collisions.Count + Environment.NewLine + string.Join(Environment.NewLine, collisions.ToArray()));
        }
    }
}
