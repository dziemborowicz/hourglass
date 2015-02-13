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
        public void TestParseNatural()
        {
            var input = Convert.ToString(TestContext.DataRow["Input"]);
            var referenceDate = Convert.ToDateTime(TestContext.DataRow["ReferenceDate"], CultureInfo.InvariantCulture);
            var culture = Convert.ToString(TestContext.DataRow["Culture"]);
            var expected = Convert.ToDateTime(TestContext.DataRow["Expected"], CultureInfo.InvariantCulture);
            var actual = DateTimeUtility.ParseNatural(input, referenceDate, CultureInfo.GetCultureInfo(culture));
            Assert.AreEqual(expected, actual, "Input: \"{0}\"; Reference Date: {1}; Culture: {2}.", input, referenceDate, culture);
        }
    }
}
