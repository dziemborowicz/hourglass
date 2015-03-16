// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanUtilityTest.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Test
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class for testing the <see cref="TimeSpanUtility"/> class.
    /// </summary>
    [TestClass]
    public class TimeSpanUtilityTest
    {
        /// <summary>
        /// Gets or sets the <see cref="TestContext"/> used to store information that is provided to unit tests.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Tests the <see cref="TimeSpanUtility.ParseNatural(string,IFormatProvider)"/> method.
        /// </summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\Resources\\TimeSpanUtilityParseNaturalTestData.csv", "TimeSpanUtilityParseNaturalTestData#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TestTimeSpanUtilityParseNatural()
        {
            string input = Convert.ToString(TestContext.DataRow["Input"]);
            string culture = Convert.ToString(TestContext.DataRow["Culture"]);
            string expected = Convert.ToString(TestContext.DataRow["Expected"]);
            TimeSpan expectedTime = expected.EndsWith("Exception") ? TimeSpan.MinValue : TimeSpan.ParseExact(expected, "G", CultureInfo.InvariantCulture);
            Type expectedException = expected.EndsWith("Exception") ? Type.GetType(expected) : null;

            if (expectedException == null)
            {
                TimeSpan actual = TimeSpanUtility.ParseNatural(input, CultureInfo.GetCultureInfo(culture));
                Assert.AreEqual(expectedTime, actual, @"Input: ""{0}""; Culture: {1}.", input, culture);
            }
            else
            {
                try
                {
                    TimeSpan actual = TimeSpanUtility.ParseNatural(input, CultureInfo.GetCultureInfo(culture));
                    Assert.Fail(@"Input: ""{0}""; Culture: {1}.; ExpectedException: {2}; Actual: {3}.", input, culture, expectedException, actual);
                }
                catch (Exception e)
                {
                    if (e is AssertFailedException)
                    {
                        throw;
                    }

                    Assert.AreEqual(expectedException, e.GetType(), @"Input: ""{0}""; Culture: {1}.", input, culture);
                }
            }
        }
    }
}
