using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions.StringExtensions;

namespace CommonExtensions.Tests
{
    [TestClass]
    public class StringTests
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string ellipsis = "\u2026";
        private const string rightangle = ">";
        private const string leftangle = "<";

        [TestMethod]
        public void FromLeftTests()
        {
            Assert.AreEqual("abc", alphabet.FromLeft(3));
            Assert.AreEqual("abcdefghijklmnopqrstuvwxy", alphabet.FromLeft(25));
            Assert.AreEqual("", alphabet.FromLeft(0));
            Assert.AreEqual(alphabet, alphabet.FromLeft(26));
            Assert.AreEqual(alphabet, alphabet.FromLeft(100));
            Assert.AreEqual("xyz", alphabet.FromLeft(-3));
        }

        [TestMethod]
        public void FromRightTests()
        {
            Assert.AreEqual("xyz", alphabet.FromRight(3));
            Assert.AreEqual("bcdefghijklmnopqrstuvwxyz", alphabet.FromRight(25));
            Assert.AreEqual("", alphabet.FromRight(0));
            Assert.AreEqual(alphabet, alphabet.FromRight(26));
            Assert.AreEqual(alphabet, alphabet.FromRight(100));
            Assert.AreEqual("abc", alphabet.FromRight(-3));
        }

        [TestMethod]
        public void FromCenterTests()
        {
            Assert.AreEqual("lmn", alphabet.FromCenter(3));
            Assert.AreEqual("lmno", alphabet.FromCenter(4));
            Assert.AreEqual("klmno", alphabet.FromCenter(5));
            Assert.AreEqual("klmnop", alphabet.FromCenter(6));
            Assert.AreEqual("abcdefghijklmnopqrstuvwxy", alphabet.FromCenter(25));
            Assert.AreEqual("", alphabet.FromCenter(0));
            Assert.AreEqual(alphabet, alphabet.FromCenter(26));
            Assert.AreEqual(alphabet, alphabet.FromCenter(100));
            Assert.AreEqual("ayz", alphabet.FromCenter(-3));
        }

        [TestMethod]
        public void FromOutsideTests()
        {
            Assert.AreEqual("ayz", alphabet.FromOutside(3));
            Assert.AreEqual("abyz", alphabet.FromOutside(4));
            Assert.AreEqual("abxyz", alphabet.FromOutside(5));
            Assert.AreEqual("abcxyz", alphabet.FromOutside(6));
            Assert.AreEqual("abcdefghijklnopqrstuvwxyz", alphabet.FromOutside(25));
            Assert.AreEqual("", alphabet.FromOutside(0));
            Assert.AreEqual(alphabet, alphabet.FromOutside(26));
            Assert.AreEqual(alphabet, alphabet.FromOutside(100));
            Assert.AreEqual("lmn", alphabet.FromOutside(-3));
        }

        [TestMethod]
        public void FromLeftEllipsisTests()
        {
            Assert.AreEqual("...", alphabet.FromLeftWithEllipsis(3));
            Assert.AreEqual("abc...", alphabet.FromLeftWithEllipsis(6));
            Assert.AreEqual("abcdefghijklmnopqrstuv...", alphabet.FromLeftWithEllipsis(25));
            Assert.AreEqual("", alphabet.FromLeftWithEllipsis(0));
            Assert.AreEqual("", alphabet.FromLeftWithEllipsis(1));
            Assert.AreEqual(alphabet, alphabet.FromLeftWithEllipsis(26));
            Assert.AreEqual(alphabet, alphabet.FromLeftWithEllipsis(100));
            Assert.AreEqual("...", alphabet.FromLeftWithEllipsis(-3));
            Assert.AreEqual("", alphabet.FromLeftWithEllipsis(-2));
            Assert.AreEqual("...xyz", alphabet.FromLeftWithEllipsis(-6));
        }

        [TestMethod]
        public void FromRightEllipsisTests()
        {
            Assert.AreEqual("...", alphabet.FromRightWithEllipsis(3));
            Assert.AreEqual("...xyz", alphabet.FromRightWithEllipsis(6));
            Assert.AreEqual("...efghijklmnopqrstuvwxyz", alphabet.FromRightWithEllipsis(25));
            Assert.AreEqual("", alphabet.FromRightWithEllipsis(0));
            Assert.AreEqual("", alphabet.FromRightWithEllipsis(1));
            Assert.AreEqual(alphabet, alphabet.FromRightWithEllipsis(26));
            Assert.AreEqual(alphabet, alphabet.FromRightWithEllipsis(100));
            Assert.AreEqual("...", alphabet.FromRightWithEllipsis(-3));
            Assert.AreEqual("", alphabet.FromRightWithEllipsis(-2));
            Assert.AreEqual("abc...", alphabet.FromRightWithEllipsis(-6));
        }

        [TestMethod]
        public void FromCenterEllipsisTests()
        {
            Assert.AreEqual("", alphabet.FromCenterWithEllipsis(3));
            Assert.AreEqual("......", alphabet.FromCenterWithEllipsis(6));
            Assert.AreEqual("...m...", alphabet.FromCenterWithEllipsis(7));
            Assert.AreEqual("...mn...", alphabet.FromCenterWithEllipsis(8));
            Assert.AreEqual("...lmn...", alphabet.FromCenterWithEllipsis(9));
            Assert.AreEqual("...defghijklmnopqrstuv...", alphabet.FromCenterWithEllipsis(25));
            Assert.AreEqual("", alphabet.FromCenterWithEllipsis(0));
            Assert.AreEqual("", alphabet.FromCenterWithEllipsis(1));
            Assert.AreEqual(alphabet, alphabet.FromCenterWithEllipsis(26));
            Assert.AreEqual(alphabet, alphabet.FromCenterWithEllipsis(100));
            Assert.AreEqual("...", alphabet.FromCenterWithEllipsis(-3));
            Assert.AreEqual("", alphabet.FromCenterWithEllipsis(-2));
            Assert.AreEqual("a...yz", alphabet.FromCenterWithEllipsis(-6));
        }

        [TestMethod]
        public void FromOutsideEllipsisTests()
        {
            Assert.AreEqual("...", alphabet.FromOutsideWithEllipsis(3));
            Assert.AreEqual("a...yz", alphabet.FromOutsideWithEllipsis(6));
            Assert.AreEqual("...z", alphabet.FromOutsideWithEllipsis(4));
            Assert.AreEqual("abcdefghijk...pqrstuvwxyz", alphabet.FromOutsideWithEllipsis(25));
            Assert.AreEqual("", alphabet.FromOutsideWithEllipsis(0));
            Assert.AreEqual("", alphabet.FromOutsideWithEllipsis(1));
            Assert.AreEqual(alphabet, alphabet.FromOutsideWithEllipsis(26));
            Assert.AreEqual(alphabet, alphabet.FromOutsideWithEllipsis(100));
            Assert.AreEqual("", alphabet.FromOutsideWithEllipsis(-3));
            Assert.AreEqual("", alphabet.FromOutsideWithEllipsis(-2));
            Assert.AreEqual("......", alphabet.FromOutsideWithEllipsis(-6));
            Assert.AreEqual("...lmn...", alphabet.FromOutsideWithEllipsis(-9));
        }

        [TestMethod]
        public void FromLeftWithTests()
        {
            Assert.AreEqual("ab" + ellipsis, alphabet.FromLeftWith(3,ellipsis));
            Assert.AreEqual("abcde" + ellipsis, alphabet.FromLeftWith(6, ellipsis));
            Assert.AreEqual("abcdefghijklmnopqrstuvwx" + ellipsis, alphabet.FromLeftWith(25, ellipsis));
            Assert.AreEqual("", alphabet.FromLeftWith(0,ellipsis));
            Assert.AreEqual(ellipsis, alphabet.FromLeftWith(1, ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromLeftWith(26,ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromLeftWith(100,ellipsis));
            Assert.AreEqual(ellipsis + "yz", alphabet.FromLeftWith(-3,ellipsis));
            Assert.AreEqual(ellipsis + "z", alphabet.FromLeftWith(-2,ellipsis));
            Assert.AreEqual(ellipsis + "vwxyz", alphabet.FromLeftWith(-6,ellipsis));
        }

        [TestMethod]
        public void FromRightWithTests()
        {
            Assert.AreEqual(ellipsis + "yz", alphabet.FromRightWith(3, ellipsis));
            Assert.AreEqual(ellipsis + "vwxyz", alphabet.FromRightWith(6,ellipsis));
            Assert.AreEqual(ellipsis + "cdefghijklmnopqrstuvwxyz", alphabet.FromRightWith(25,ellipsis));
            Assert.AreEqual("", alphabet.FromRightWith(0,ellipsis));
            Assert.AreEqual(ellipsis, alphabet.FromRightWith(1, ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromRightWith(26,ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromRightWith(100,ellipsis));
            Assert.AreEqual("ab" + ellipsis, alphabet.FromRightWith(-3, ellipsis));
            Assert.AreEqual("a" + ellipsis, alphabet.FromRightWith(-2, ellipsis));
            Assert.AreEqual("abcde" + ellipsis, alphabet.FromRightWith(-6, ellipsis));
        }

        [TestMethod]
        public void FromCenterWithTests()
        {
            Assert.AreEqual(ellipsis + "m" + ellipsis, alphabet.FromCenterWith(3, ellipsis));
            Assert.AreEqual(ellipsis + ellipsis, alphabet.FromCenterWith(2, ellipsis));
            Assert.AreEqual(ellipsis + "bcdefghijklmnopqrstuvwx" + ellipsis, alphabet.FromCenterWith(25, ellipsis));
            Assert.AreEqual("", alphabet.FromCenterWith(0,ellipsis));
            Assert.AreEqual("", alphabet.FromCenterWith(1, ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromCenterWith(26,ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromCenterWith(100,ellipsis));
            Assert.AreEqual("a" + ellipsis + "z", alphabet.FromCenterWith(-3, ellipsis));
            Assert.AreEqual(ellipsis + "z", alphabet.FromCenterWith(-2,ellipsis));
            Assert.AreEqual("ab" + ellipsis + "xyz", alphabet.FromCenterWith(-6, ellipsis));

            Assert.AreEqual(leftangle + "m" + rightangle, alphabet.FromCenterWith(3, leftangle, rightangle));
            Assert.AreEqual(leftangle + rightangle, alphabet.FromCenterWith(2, leftangle, rightangle));
            Assert.AreEqual(leftangle + "bcdefghijklmnopqrstuvwx" + rightangle, alphabet.FromCenterWith(25, leftangle, rightangle));
            Assert.AreEqual("", alphabet.FromCenterWith(0, leftangle, rightangle));
            Assert.AreEqual("", alphabet.FromCenterWith(1, leftangle, rightangle));
            Assert.AreEqual(alphabet, alphabet.FromCenterWith(26, leftangle, rightangle));
            Assert.AreEqual(alphabet, alphabet.FromCenterWith(100, leftangle, rightangle));
            Assert.AreEqual(leftangle + rightangle + "z", alphabet.FromCenterWith(-3, leftangle, rightangle));
            Assert.AreEqual(leftangle + rightangle, alphabet.FromCenterWith(-2, leftangle, rightangle));
            Assert.AreEqual("ab" + leftangle + rightangle + "yz", alphabet.FromCenterWith(-6, leftangle, rightangle));
        }

        [TestMethod]
        public void FromOutsideWithTests()
        {
            Assert.AreEqual("a" + ellipsis + "z", alphabet.FromOutsideWith(3,ellipsis));
            Assert.AreEqual(ellipsis + "z", alphabet.FromOutsideWith(2, ellipsis));
            Assert.AreEqual("ab" + ellipsis + "xyz", alphabet.FromOutsideWith(6,ellipsis));
            Assert.AreEqual("abcdefghijkl" + ellipsis + "opqrstuvwxyz", alphabet.FromOutsideWith(25,ellipsis));
            Assert.AreEqual("", alphabet.FromOutsideWith(0,ellipsis));
            Assert.AreEqual(ellipsis, alphabet.FromOutsideWith(1,ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromOutsideWith(26,ellipsis));
            Assert.AreEqual(alphabet, alphabet.FromOutsideWith(100,ellipsis));
            Assert.AreEqual(ellipsis + "m" + ellipsis, alphabet.FromOutsideWith(-3, ellipsis));
            Assert.AreEqual(ellipsis + ellipsis, alphabet.FromOutsideWith(-2, ellipsis));
        }
    }
}
