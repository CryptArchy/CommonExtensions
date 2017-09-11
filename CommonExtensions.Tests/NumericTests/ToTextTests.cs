using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CommonExtensions.NumericExtensions;

namespace CommonExtensions.Tests.NumericTests
{
    [TestClass]
    public class ToTextTests
    {        
        [TestMethod]
        public void ToText_Outputs_Correct_Strings()
        {
            var tests = new Dictionary<long, string>() { 
                {0, "Zero"}, 
                {1, "One"},
                {2, "Two"},
                {3, "Three"},
                {4, "Four"},
                {-5, "Negative Five"},
                {-6, "Negative Six"},
                {-7, "Negative Seven"},
                {-8, "Negative Eight"},
                {-9, "Negative Nine"},
                {10, "Ten"},
                {11, "Eleven"},
                {12, "Twelve"},
                {13, "Thirteen"},
                {14, "Fourteen"},
                {15, "Fifteen"},
                {16, "Sixteen"},
                {17, "Seventeen"},
                {18, "Eighteen"},
                {19, "Nineteen"},
                {20, "Twenty"},
                {100, "One Hundred"},
                {1000, "One Thousand"},
                {10000, "Ten Thousand"},
                {100000, "One Hundred Thousand"},
                {1000000, "One Million"},
                {10000000, "Ten Million"},
                {100000000, "One Hundred Million"},
                {1000000000, "One Billion"},
                {2345678967, "Two Billion Three Hundred and Forty-Five Million Six Hundred and Seventy-Eight Thousand Nine Hundred and Sixty-Seven"}
            };

            foreach (var test in tests)
            {
                Assert.AreEqual(test.Value, test.Key.ToText());
            }
        }

        [TestMethod]
        public void ToOrdinalText_Outputs_Correct_Strings()
        {
            var tests = new Dictionary<long, string>() { 
                {0, "Zeroth"}, 
                {1, "First"},
                {2, "Second"},
                {3, "Third"},
                {4, "Fourth"},
                {-5, "Negative Fifth"},
                {-6, "Negative Sixth"},
                {-7, "Negative Seventh"},
                {-8, "Negative Eighth"},
                {-9, "Negative Ninth"},
                {10, "Tenth"},
                {11, "Eleventh"},
                {12, "Twelfth"},
                {13, "Thirteenth"},
                {14, "Fourteenth"},
                {15, "Fifteenth"},
                {16, "Sixteenth"},
                {17, "Seventeenth"},
                {18, "Eighteenth"},
                {19, "Nineteenth"},
                {20, "Twentieth"},
                {100, "One Hundredth"},
                {101, "One Hundred and First"},
                {111, "One Hundred and Eleventh"},
                {202, "Two Hundred and Second"},
                {212, "Two Hundred and Twelfth"},
                {303, "Three Hundred and Third"},
                {313, "Three Hundred and Thirteenth"},
                {404, "Four Hundred and Fourth"},
                {414, "Four Hundred and Fourteenth"},
                {1000, "One Thousandth"},
                {10000, "Ten Thousandth"},
                {100000, "One Hundred Thousandth"},
                {1000000, "One Millionth"},
                {10000000, "Ten Millionth"},
                {100000000, "One Hundred Millionth"},
                {1000000000, "One Billionth"},
                {2345678967, "Two Billion Three Hundred and Forty-Five Million Six Hundred and Seventy-Eight Thousand Nine Hundred and Sixty-Seventh"}
            };

            foreach (var test in tests)
            {
                Assert.AreEqual(test.Value, test.Key.ToOrdinalText());
            }
        }

        [TestMethod]
        public void ToOrdinal_Outputs_Correct_Strings()
        {
            var tests = new Dictionary<long, string>() { 
                {0, "0th"}, 
                {1, "1st"},
                {2, "2nd"},
                {3, "3rd"},
                {4, "4th"},
                {-5, "-5th"},
                {-6, "-6th"},
                {-7, "-7th"},
                {-8, "-8th"},
                {-9, "-9th"},
                {10, "10th"},
                {11, "11th"},
                {12, "12th"},
                {13, "13th"},
                {14, "14th"},
                {15, "15th"},
                {16, "16th"},
                {17, "17th"},
                {18, "18th"},
                {19, "19th"},
                {20, "20th"},
                {100, "100th"},
                {101, "101st"},
                {111, "111th"},
                {202, "202nd"},
                {212, "212th"},
                {303, "303rd"},
                {313, "313th"},
                {404, "404th"},
                {414, "414th"},
                {2345678967, "2345678967th"}
            };

            foreach (var test in tests)
            {
                Assert.AreEqual(test.Value, test.Key.ToOrdinal());
            }
        }
    }
}
