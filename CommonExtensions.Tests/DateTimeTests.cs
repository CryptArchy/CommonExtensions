using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions.DateTimeExtensions;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommonExtensions.Tests
{
    [TestClass]
    public class DateTimeTests
    {
        private class TestContext
        {
            public string Name;
            public DateTime BirthDate;
            public DateTime CurrentDate;
            public int ExpectedAge;
        }

        private List<TestContext> TestsToRun_ForNaturalAge;
        private List<TestContext> TestsToRun_ForIntegerAge;

        [TestInitialize]
        public void InitializeDateTimeTests()
        {
            TestsToRun_ForNaturalAge = new List<TestContext>()
            {
                new TestContext() {Name = "Currently standard year the day before; Born on leap year before leap day", 
                    BirthDate = new DateTime(2000, 2, 28), CurrentDate = new DateTime(2011, 2, 27), ExpectedAge = 10},
                new TestContext() {Name = "Currently standard year the day of; Born on leap year before leap day", 
                    BirthDate = new DateTime(2000, 2, 28), CurrentDate = new DateTime(2011, 2, 28), ExpectedAge = 11},
                new TestContext() {Name = "Currently standard year the day after; Born on leap year before leap day", 
                    BirthDate = new DateTime(2000, 2, 28), CurrentDate = new DateTime(2011, 3, 1), ExpectedAge = 11},

                new TestContext() {Name = "Currently standard year the day before; Born on leap day", 
                    BirthDate = new DateTime(2000, 2, 29), CurrentDate = new DateTime(2011, 2, 27), ExpectedAge = 10},
                new TestContext() {Name = "Currently standard year the day of; Born on leap day", 
                    BirthDate = new DateTime(2000, 2, 29), CurrentDate = new DateTime(2011, 2, 28), ExpectedAge = 10},
                new TestContext() {Name = "Currently standard year the day after; Born on leap day", 
                    BirthDate = new DateTime(2000, 2, 29), CurrentDate = new DateTime(2011, 3, 1), ExpectedAge = 11},

                new TestContext() {Name = "Currently standard year two days before; Born on leap year after leap day", 
                    BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2011, 2, 27), ExpectedAge = 10},
                new TestContext() {Name = "Currently standard year the day before; Born on leap year after leap day", 
                    BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2011, 2, 28), ExpectedAge = 10},
                new TestContext() {Name = "Currently standard year the day of; Born on leap year after leap day", 
                    BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2011, 3, 1), ExpectedAge = 11},

                new TestContext() {Name = "Currently leap year the day before; Born on leap year before leap day", 
                    BirthDate = new DateTime(2000, 2, 28), CurrentDate = new DateTime(2012, 2, 28), ExpectedAge = 12},
                new TestContext() {Name = "Currently leap year the day of; Born on leap year before leap day", 
                    BirthDate = new DateTime(2000, 2, 28), CurrentDate = new DateTime(2012, 2, 29), ExpectedAge = 12},
                new TestContext() {Name = "Currently leap year the day after; Born on leap year before leap day", 
                    BirthDate = new DateTime(2000, 2, 28), CurrentDate = new DateTime(2012, 3, 1), ExpectedAge = 12},

                new TestContext() {Name = "Currently leap year the day before; Born on leap day", 
                    BirthDate = new DateTime(2000, 2, 29), CurrentDate = new DateTime(2012, 2, 28), ExpectedAge = 11},
                new TestContext() {Name = "Currently leap year the day of; Born on leap day", 
                    BirthDate = new DateTime(2000, 2, 29), CurrentDate = new DateTime(2012, 2, 29), ExpectedAge = 12},
                new TestContext() {Name = "Currently leap year the day after; Born on leap day", 
                    BirthDate = new DateTime(2000, 2, 29), CurrentDate = new DateTime(2012, 3, 1), ExpectedAge = 12},

                new TestContext() {Name = "Currently leap year two days before; Born on leap year after leap day", 
                    BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2012, 2, 28), ExpectedAge = 11},
                new TestContext() {Name = "Currently leap year the day before; Born on leap year after leap day", 
                    BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2012, 2, 29), ExpectedAge = 11},
                new TestContext() {Name = "Currently leap year the day of; Born on leap year after leap day", 
                    BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2012, 3, 1), ExpectedAge = 12},
            };

            TestsToRun_ForIntegerAge = new List<TestContext>(TestsToRun_ForNaturalAge);

            TestsToRun_ForNaturalAge.Add(new TestContext() { Name = "Born this year, a day in the future", 
                BirthDate = new DateTime(2000, 2, 2), CurrentDate = new DateTime(2000, 2, 1), ExpectedAge = 0 });
            TestsToRun_ForNaturalAge.Add(new TestContext() { Name = "Born this year, a month in the future", 
                BirthDate = new DateTime(2000, 2, 1), CurrentDate = new DateTime(2000, 1, 1), ExpectedAge = 0 });
            TestsToRun_ForNaturalAge.Add(new TestContext() { Name = "Born this day, 5 years in the future", 
                BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(1995, 3, 1), ExpectedAge = 0 });

            TestsToRun_ForIntegerAge.Add(new TestContext() { Name = "Born this year, a day in the future", 
                BirthDate = new DateTime(2000, 2, 3), CurrentDate = new DateTime(2000, 2, 1), ExpectedAge = -1 }); //Born days in the future
            TestsToRun_ForIntegerAge.Add(new TestContext() { Name = "Born this year, a month in the future", 
                BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(2000, 1, 1), ExpectedAge = -1 }); //Born months in the future
            TestsToRun_ForIntegerAge.Add(new TestContext() { Name = "Born this day, 5 years in the future", 
                BirthDate = new DateTime(2000, 3, 1), CurrentDate = new DateTime(1995, 3, 1), ExpectedAge = -5 }); //Born years in the future
        }

        [TestMethod]
        public void ToIntegerAge_Extension_Method_Returns_Correct_Values()
        {
            foreach (var test in TestsToRun_ForIntegerAge)
            {
                var actualAge = test.BirthDate.ToIntegerAge(test.CurrentDate);
                Assert.AreEqual(test.ExpectedAge, actualAge, "Integer Age. " + test.Name);
            }
        }

        [TestMethod]
        public void ToNaturalAge_Extension_Method_Returns_Correct_Values()
        {
            foreach (var test in TestsToRun_ForNaturalAge)
            {
                var actualAge = test.BirthDate.ToNaturalAge(test.CurrentDate);
                Assert.AreEqual(test.ExpectedAge, actualAge, "Natural Age. " + test.Name);
            }
        }

         [TestMethod]
        public void Performance_Test_Alternative_Methods()
        {
            // Use the commented out value for trials to get a good performance test. Use the "trials = 1" value so running all tests doesn't take a really long time.
            //var trials = 100000;
            var trials = 1;
            var funcs = new List<Func<DateTime, DateTime, int>>() { AgeByCompareMonthsAndDays, AgeByTimeSpanDiff, AgeByBigDateDiff };

            var funcCount = funcs.Count;
            var testCount = TestsToRun_ForNaturalAge.Count;

            var messages = new List<string>(funcCount * testCount + funcCount);

            var sw = new Stopwatch();
            var results = new int[funcCount, testCount];
            var times = new long[funcCount];

            for (int t = 0; t < funcCount; t++)
            {
                sw.Start();
                for (int x = 0; x < trials; x++)
                    for (int i = 0; i < TestsToRun_ForNaturalAge.Count; i++)
                        results[t, i] = funcs[t](TestsToRun_ForNaturalAge[i].BirthDate, TestsToRun_ForNaturalAge[i].CurrentDate);
                times[t] = sw.ElapsedMilliseconds;
                sw.Reset();

                messages.Add(string.Format("Test {0} ran in {1}ms.", t + 1, times[t]));
                for (int i = 0; i < testCount; i++)
                {
                    messages.Add(string.Format("{0}: Expected {1} Actual {2}; {3} : {4}",
                        TestsToRun_ForNaturalAge[i].ExpectedAge == results[t, i] ? "PASS" : "FAIL",
                        TestsToRun_ForNaturalAge[i].ExpectedAge, results[t, i],
                        TestsToRun_ForNaturalAge[i].BirthDate.ToShortDateString(),
                        TestsToRun_ForNaturalAge[i].CurrentDate.ToShortDateString()));
                    //Assert.AreEqual(TestsToRun[i].ExpectedAge, 
                    //    results[t, i], 
                    //    string.Format("{0} | {1}", 
                    //    TestsToRun[i].BirthDate.ToShortDateString(), 
                    //    TestsToRun[i].CurrentDate.ToShortDateString()));
                }
            }
        }

        // This is the same as the method currently used for the ToAge extension.  It is correct, but relatively slow.
        public int AgeByCompareMonthsAndDays(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }

        // This method seems to be correct but is only marginally faster
        public int AgeByBigDateDiff(DateTime birthdate, DateTime current)
        {
            var b = birthdate.Year * 1000 + birthdate.Month * 100 + birthdate.Day;
            var n = current.Year * 1000 + current.Month * 100 + current.Day;

            return (n - b) / 1000;
        }

        
        // This method calculates age by getting the TimeSpan difference between the two dates.  It is not quite accurate, but is very fast.
        public int AgeByTimeSpanDiff(DateTime birthDate, DateTime now)
        {
            // TimeSpans can only output the difference in, at best, a quantity of days
            // But trying to convert Days to Years accurately is a challenge, and there always seem to be edge cases where it won't be accurate.

            double daysInAYear = 365.2425;
            var xbirthDate = birthDate.Date;//.AddMinutes(1439);
            var xnow = now.Date;//.AddMinutes(1439);//.AddHours(23);

            var diff = xnow.Subtract(xbirthDate);
            var rawDays = diff.TotalDays;
            var rawYears = rawDays / daysInAYear;
            var roundYears = Math.Floor(rawYears);//Math.Round(rawYears, 2, MidpointRounding.AwayFromZero); //Math.Floor(rawYears);
            var intYears = (int)roundYears;
            return intYears;

            //return (int)Math.Floor(now.Subtract(birthDate).TotalDays / daysInAYear);
        }
    }
}
