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
        public void TestParseNatural()
        {
            var input = Convert.ToString(TestContext.DataRow["Input"]);
            var culture = Convert.ToString(TestContext.DataRow["Culture"]);
            var expected = TimeSpan.ParseExact(Convert.ToString(TestContext.DataRow["Expected"]), "G", CultureInfo.InvariantCulture);
            var actual = TimeSpanUtility.ParseNatural(input, CultureInfo.GetCultureInfo(culture));
            Assert.AreEqual(expected, actual, "Input: \"{0}\"; Culture: {1}.", input, culture);
        }
    }
}
