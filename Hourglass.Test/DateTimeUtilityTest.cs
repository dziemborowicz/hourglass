using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Hourglass;

namespace Hourglass.Test
{
    [TestClass]
    public class DateTimeUtilityTest
    {
        [TestMethod]
        public void TestParseNatural()
        {
            var tests = GetParseNaturalTests();
            foreach (var test in tests.Where((t) => !t.Ignore))
                RunParseNaturalTest(test);
        }

        private struct ParseNaturalTestInfo
        {
            public DateTime Now;
            public CultureInfo Culture;
            public string Input;
            public DateTime ExpectedOutput;
            public Type ExpectedException;
            public bool Ignore;
        }

        private IEnumerable<ParseNaturalTestInfo> GetParseNaturalTests()
        {
            return new ParseNaturalTestInfo[] {
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = null, ExpectedException = typeof(ArgumentNullException) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = string.Empty, ExpectedException = typeof(FormatException) },

                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "13/01/2000", ExpectedException = typeof(FormatException) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/32/2000", ExpectedException = typeof(FormatException) },

                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000", ExpectedOutput = new DateTime(2000, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/02/2000", ExpectedOutput = new DateTime(2000, 1, 2) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "02/01/2000", ExpectedOutput = new DateTime(2000, 2, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12/01/2000", ExpectedOutput = new DateTime(2000, 12, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/31/2000", ExpectedOutput = new DateTime(2000, 1, 31) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00:00noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00:00midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00:00midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00:00 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00:00 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00:00 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },

                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 am", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12 pm", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2 am", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2 pm", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 am", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00 pm", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34 am", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34 pm", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56 am", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56 pm", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 0:00", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 12:00", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 14:34", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 2:34:56", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "01/01/2000 14:34:56", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "midnight", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12:00 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12:00 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34:56 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34:56 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "noon", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "midday", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "midnight", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12 PM", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2 AM", ExpectedOutput = new DateTime(2000, 1, 2, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2 PM", ExpectedOutput = new DateTime(2000, 1, 2, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12:00 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "12:00 PM", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34 AM", ExpectedOutput = new DateTime(2000, 1, 2, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34 PM", ExpectedOutput = new DateTime(2000, 1, 2, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34:56 AM", ExpectedOutput = new DateTime(2000, 1, 2, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "2:34:56 PM", ExpectedOutput = new DateTime(2000, 1, 2, 14, 34, 56) },


                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/13/2000", ExpectedException = typeof(FormatException) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "32/01/2000", ExpectedException = typeof(FormatException) },

                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000", ExpectedOutput = new DateTime(2000, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/02/2000", ExpectedOutput = new DateTime(2000, 2, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "02/01/2000", ExpectedOutput = new DateTime(2000, 1, 2) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/12/2000", ExpectedOutput = new DateTime(2000, 12, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "31/01/2000", ExpectedOutput = new DateTime(2000, 1, 31) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00:00noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00:00midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00:00midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00:00 noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00:00 midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00:00 midnight", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },

                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00AM", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 am", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12 pm", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2 am", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2 pm", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 am", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00 pm", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34 am", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34 pm", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56 am", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56 pm", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 0:00", ExpectedOutput = new DateTime(2000, 1, 1, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 12:00", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 14:34", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 2:34:56", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "01/01/2000 14:34:56", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "noon", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "midday", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "midnight", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12:00 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12:00 PM", ExpectedOutput = new DateTime(2000, 1, 1, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34:56 AM", ExpectedOutput = new DateTime(2000, 1, 1, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34:56 PM", ExpectedOutput = new DateTime(2000, 1, 1, 14, 34, 56) },
                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "noon", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "midday", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "midnight", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12 PM", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2 AM", ExpectedOutput = new DateTime(2000, 1, 2, 2, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2 PM", ExpectedOutput = new DateTime(2000, 1, 2, 14, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12:00 AM", ExpectedOutput = new DateTime(2000, 1, 2, 0, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "12:00 PM", ExpectedOutput = new DateTime(2000, 1, 2, 12, 0, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34 AM", ExpectedOutput = new DateTime(2000, 1, 2, 2, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34 PM", ExpectedOutput = new DateTime(2000, 1, 2, 14, 34, 0) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34:56 AM", ExpectedOutput = new DateTime(2000, 1, 2, 2, 34, 56) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 1, 1, 23, 59, 59), Culture = CultureInfo.GetCultureInfo("en-AU"), Input = "2:34:56 PM", ExpectedOutput = new DateTime(2000, 1, 2, 14, 34, 56) },

                
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "ny", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "nye", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "new year", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "new years", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "new year's", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "new year eve", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "new years eve", ExpectedOutput = new DateTime(2001, 1, 1) },
                new ParseNaturalTestInfo() { Now = new DateTime(2000, 12, 31), Culture = CultureInfo.GetCultureInfo("en-US"), Input = "new year's eve", ExpectedOutput = new DateTime(2001, 1, 1) }
            };
        }

        private void RunParseNaturalTest(ParseNaturalTestInfo test)
        {
            try
            {
                DateTime actualOutput = DateTimeUtility.ParseNatural(test.Input, test.Now, test.Culture);
                if (test.ExpectedException != null)
                    Assert.Fail(string.Format("Parsing \"{0}\" at {1} with {2} culture failed. Expected {3}.", test.Input, test.Now, test.Culture.Name, test.ExpectedException));
                else
                    Assert.AreEqual(test.ExpectedOutput, actualOutput, string.Format("Parsing \"{0}\" at {1} with {2} culture failed.", test.Input, test.Now, test.Culture.Name));
            }
            catch (Exception e)
            {
                if (test.ExpectedException == null || e.GetType() != test.ExpectedException)
                    Assert.Fail(string.Format("Parsing \"{0}\" at {1} with {2} culture failed. Unexpected exception: {3}.", test.Input, test.Now, test.Culture.Name, e));
            }
        }
    }
}
