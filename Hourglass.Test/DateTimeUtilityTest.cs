using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Hourglass;

namespace Hourglass.Test
{
    [TestClass]
    public class DateTimeUtilityTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseNatural000()
        {
            DateTime actual = DateTimeUtility.ParseNatural(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural001()
        {
            DateTime actual = DateTimeUtility.ParseNatural("");
        }

        [TestMethod]
        public void TestParseNatural002()
        {
            DateTime actual = DateTimeUtility.ParseNatural("ny");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural003()
        {
            DateTime actual = DateTimeUtility.ParseNatural("nye");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural004()
        {
            DateTime actual = DateTimeUtility.ParseNatural("new year");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural005()
        {
            DateTime actual = DateTimeUtility.ParseNatural("new years");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural006()
        {
            DateTime actual = DateTimeUtility.ParseNatural("new year's");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural007()
        {
            DateTime actual = DateTimeUtility.ParseNatural("new year eve");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural008()
        {
            DateTime actual = DateTimeUtility.ParseNatural("new years eve");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural009()
        {
            DateTime actual = DateTimeUtility.ParseNatural("new year's eve");
            Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 1, 1), actual);
        }

        [TestMethod]
        public void TestParseNatural010()
        {
            DateTime actual = DateTimeUtility.ParseNatural("01/01/2001");
            Assert.AreEqual(DateTime.Parse("01/01/2001"), actual);
        }

        [TestMethod]
        public void TestParseNatural011()
        {
            DateTime actual = DateTimeUtility.ParseNatural("01/01/2001 2:35 PM");
            Assert.AreEqual(DateTime.Parse("01/01/2001 2:35 PM"), actual);
        }
    }
}
