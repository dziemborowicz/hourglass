// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeUtilityTest.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Test
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class for testing the <see cref="DateTimeUtility"/> class.
    /// </summary>
    [TestClass]
    public class DateTimeUtilityTest
    {
        /// <summary>
        /// Gets or sets the <see cref="TestContext"/> used to store information that is provided to unit tests.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Tests the <see cref="DateTimeUtility.ParseNatural(string,DateTime,IFormatProvider)"/> method.
        /// </summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\Resources\\DateTimeUtilityParseNaturalTestData.csv", "DateTimeUtilityParseNaturalTestData#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TestDateTimeUtilityParseNatural()
        {
            string input = Convert.ToString(TestContext.DataRow["Input"]);
            DateTime referenceDate = Convert.ToDateTime(TestContext.DataRow["ReferenceDate"], CultureInfo.InvariantCulture);
            string culture = Convert.ToString(TestContext.DataRow["Culture"]);
            string expected = Convert.ToString(TestContext.DataRow["Expected"]);
            DateTime expectedDate = expected.EndsWith("Exception") ? DateTime.MinValue : Convert.ToDateTime(expected, CultureInfo.InvariantCulture);
            Type expectedException = expected.EndsWith("Exception") ? Type.GetType(expected) : null;

            if (expectedException == null)
            {
                DateTime actual = DateTimeUtility.ParseNatural(input, referenceDate, CultureInfo.GetCultureInfo(culture));
                Assert.AreEqual(expectedDate, actual, @"Input: ""{0}""; Reference Date: {1}; Culture: {2}.", input, referenceDate, culture);
            }
            else
            {
                try
                {
                    DateTime actual = DateTimeUtility.ParseNatural(input, referenceDate, CultureInfo.GetCultureInfo(culture));
                    Assert.Fail(@"Input: ""{0}""; Reference Date: {1}; Culture: {2}; ExpectedException: {3}; Actual: {4}.", input, referenceDate, culture, expectedException, actual);
                }
                catch (Exception e)
                {
                    if (e is AssertFailedException)
                    {
                        throw;
                    }

                    Assert.AreEqual(expectedException, e.GetType(), @"Input: ""{0}""; Reference Date: {1}; Culture: {2}.", input, referenceDate, culture);
                }
            }
        }
    }
}
