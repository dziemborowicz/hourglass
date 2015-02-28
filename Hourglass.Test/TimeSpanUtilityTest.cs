using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hourglass.Test
{
    [TestClass]
    public class TimeSpanUtilityTest
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\Resources\\TimeSpanUtilityParseNaturalTestData.csv", "TimeSpanUtilityParseNaturalTestData#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void Test_TimeSpanUtility_ParseNatural()
        {
            var input = Convert.ToString(TestContext.DataRow["Input"]);
            var culture = Convert.ToString(TestContext.DataRow["Culture"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var expectedTime = expected.EndsWith("Exception") ? TimeSpan.MinValue : TimeSpan.ParseExact(expected, "G", CultureInfo.InvariantCulture);
            var expectedException = expected.EndsWith("Exception") ? Type.GetType(expected) : null;

            if (expectedException == null)
            {
                var actual = TimeSpanUtility.ParseNatural(input, CultureInfo.GetCultureInfo(culture));
                Assert.AreEqual(expectedTime, actual, @"Input: ""{0}""; Culture: {1}.", input, culture);
            }
            else
            {
                try
                {
                    var actual = TimeSpanUtility.ParseNatural(input, CultureInfo.GetCultureInfo(culture));
                    Assert.Fail(@"Input: ""{0}""; Culture: {1}.; ExpectedException: {2}; Actual: {3}.", input, culture, expectedException, actual);
                }
                catch (Exception e)
                {
                    if (e is AssertFailedException)
                        throw;

                    Assert.AreEqual(expectedException, e.GetType(), @"Input: ""{0}""; Culture: {1}.", input, culture);
                }
            }
        }
    }
}
