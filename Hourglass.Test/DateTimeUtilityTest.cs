using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hourglass.Test
{
    [TestClass]
    public class DateTimeUtilityTest
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\Resources\\DateTimeUtilityParseNaturalTestData.csv", "DateTimeUtilityParseNaturalTestData#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void Test_DateTimeUtility_ParseNatural()
        {
            var input = Convert.ToString(TestContext.DataRow["Input"]);
            var referenceDate = Convert.ToDateTime(TestContext.DataRow["ReferenceDate"], CultureInfo.InvariantCulture);
            var culture = Convert.ToString(TestContext.DataRow["Culture"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var expectedDate = expected.EndsWith("Exception") ? DateTime.MinValue : Convert.ToDateTime(expected, CultureInfo.InvariantCulture);
            var expectedException = expected.EndsWith("Exception") ? Type.GetType(expected) : null;

            if (expectedException == null)
            {
                var actual = DateTimeUtility.ParseNatural(input, referenceDate, CultureInfo.GetCultureInfo(culture));
                Assert.AreEqual(expectedDate, actual, @"Input: ""{0}""; Reference Date: {1}; Culture: {2}.", input, referenceDate, culture);
            }
            else
            {
                try
                {
                    var actual = DateTimeUtility.ParseNatural(input, referenceDate, CultureInfo.GetCultureInfo(culture));
                    Assert.Fail(@"Input: ""{0}""; Reference Date: {1}; Culture: {2}; ExpectedException: {3}; Actual: {4}.", input, referenceDate, culture, expectedException, actual);
                }
                catch (Exception e)
                {
                    if (e is AssertFailedException)
                        throw;

                    Assert.AreEqual(expectedException, e.GetType(), @"Input: ""{0}""; Reference Date: {1}; Culture: {2}.", input, referenceDate, culture);
                }
            }
        }
    }
}
