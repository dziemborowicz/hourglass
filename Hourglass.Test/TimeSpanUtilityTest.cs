using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hourglass.Test
{
    [TestClass]
    public class TimeSpanUtilityTest
    {
        #region TestParseNatural

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParseNatural000()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural001()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("");
        }

        [TestMethod]
        public void TestParseNatural002()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural003()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1");
            Assert.AreEqual(new TimeSpan(0, 0, 1, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural004()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural005()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural006()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("60");
            Assert.AreEqual(new TimeSpan(0, 0, 60, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural007()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("10000");
            Assert.AreEqual(new TimeSpan(0, 0, 10000, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural008()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural(".30");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural009()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0.30");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural010()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1.30");
            Assert.AreEqual(new TimeSpan(0, 0, 1, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural011()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.30");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural012()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5.30");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural013()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("60.30");
            Assert.AreEqual(new TimeSpan(0, 0, 60, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural014()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("10000.30");
            Assert.AreEqual(new TimeSpan(0, 0, 10000, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural015()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0.0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural016()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0.0.0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural017()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0.0.0.0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural018()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5.5");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural019()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5.5.5");
            Assert.AreEqual(new TimeSpan(0, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural020()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5.5.5.5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural021()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("60.60");
            Assert.AreEqual(new TimeSpan(0, 0, 60, 60, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural022()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("24.60.60");
            Assert.AreEqual(new TimeSpan(0, 24, 60, 60, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural023()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100.24.60.60");
            Assert.AreEqual(new TimeSpan(100, 24, 60, 60, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural024()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100.100");
            Assert.AreEqual(new TimeSpan(0, 0, 100, 100, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural025()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100.100.100");
            Assert.AreEqual(new TimeSpan(0, 100, 100, 100, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural026()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100.100.100.100");
            Assert.AreEqual(new TimeSpan(100, 100, 100, 100, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural027()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0:0:0:0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural028()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5:5:5:5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural029()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100:24:60:60");
            Assert.AreEqual(new TimeSpan(100, 24, 60, 60, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural030()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100:100:100:100");
            Assert.AreEqual(new TimeSpan(100, 100, 100, 100, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural031()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0,0,0,0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural032()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5,5,5,5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural033()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100,24,60,60");
            Assert.AreEqual(new TimeSpan(100, 24, 60, 60, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural034()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100,100,100,100");
            Assert.AreEqual(new TimeSpan(100, 100, 100, 100, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural035()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("0 0 0 0");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural036()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5 5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural037()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100 24 60 60");
            Assert.AreEqual(new TimeSpan(100, 24, 60, 60, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural038()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("100 100 100 100");
            Assert.AreEqual(new TimeSpan(100, 100, 100, 100, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural039()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5s");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural040()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5m");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural041()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5h");
            Assert.AreEqual(new TimeSpan(0, 5, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural042()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural043()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 s");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural044()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 m");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural045()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 h");
            Assert.AreEqual(new TimeSpan(0, 5, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural046()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 d");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural047()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5sec");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural048()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5min");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural049()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5hr");
            Assert.AreEqual(new TimeSpan(0, 5, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural050()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5dy");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural051()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 sec");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural052()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 min");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural053()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 hr");
            Assert.AreEqual(new TimeSpan(0, 5, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural054()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 dy");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural055()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5seconds");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural056()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5minutes");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural057()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5hours");
            Assert.AreEqual(new TimeSpan(0, 5, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural058()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5days");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural059()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 seconds");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural060()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 minutes");
            Assert.AreEqual(new TimeSpan(0, 0, 5, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural061()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 hours");
            Assert.AreEqual(new TimeSpan(0, 5, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural062()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 days");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural063()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1second");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 1, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural064()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1minute");
            Assert.AreEqual(new TimeSpan(0, 0, 1, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural065()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1hour");
            Assert.AreEqual(new TimeSpan(0, 1, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural066()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1day");
            Assert.AreEqual(new TimeSpan(1, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural067()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1 second");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 1, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural068()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1 minute");
            Assert.AreEqual(new TimeSpan(0, 0, 1, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural069()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1 hour");
            Assert.AreEqual(new TimeSpan(0, 1, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural070()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("1 day");
            Assert.AreEqual(new TimeSpan(1, 0, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural071()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d5h5m5s");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural072()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d 5h 5m 5s");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural073()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 d 5 h 5 m 5 s");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural074()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d 5 5 5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural075()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5h 5 5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural076()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5m 5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural077()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5 5s");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural078()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d 5 5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 0, 0), actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural079()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5d 5");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural080()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5d");
        }

        [TestMethod]
        public void TestParseNatural081()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5h 5 5");
            Assert.AreEqual(new TimeSpan(0, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural082()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5h 5");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 0, 0), actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural083()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5h");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural084()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5m 5 5");
        }

        [TestMethod]
        public void TestParseNatural085()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5m 5");
            Assert.AreEqual(new TimeSpan(0, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural086()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5m");
            Assert.AreEqual(new TimeSpan(5, 5, 5, 0, 0), actual);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural087()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5s 5 5");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestParseNatural088()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5s 5");
        }

        [TestMethod]
        public void TestParseNatural089()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5 5 5s");
            Assert.AreEqual(new TimeSpan(0, 5, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural090()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d 5 5s");
            Assert.AreEqual(new TimeSpan(5, 5, 0, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural091()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("5d 5m 5");
            Assert.AreEqual(new TimeSpan(5, 0, 5, 5, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural092()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5s");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2, 500), actual);
        }

        [TestMethod]
        public void TestParseNatural093()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5m");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural094()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5h");
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural095()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5d");
            Assert.AreEqual(new TimeSpan(2, 12, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural096()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 s");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2, 500), actual);
        }

        [TestMethod]
        public void TestParseNatural097()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 m");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural098()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 h");
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural099()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 d");
            Assert.AreEqual(new TimeSpan(2, 12, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural100()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5sec");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2, 500), actual);
        }

        [TestMethod]
        public void TestParseNatural101()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5min");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural102()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5hr");
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural103()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5dy");
            Assert.AreEqual(new TimeSpan(2, 12, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural104()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 sec");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2, 500), actual);
        }

        [TestMethod]
        public void TestParseNatural105()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 min");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural106()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 hr");
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural107()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 dy");
            Assert.AreEqual(new TimeSpan(2, 12, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural108()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5seconds");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2, 500), actual);
        }

        [TestMethod]
        public void TestParseNatural109()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5minutes");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural110()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5hours");
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural111()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5days");
            Assert.AreEqual(new TimeSpan(2, 12, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural112()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 seconds");
            Assert.AreEqual(new TimeSpan(0, 0, 0, 2, 500), actual);
        }

        [TestMethod]
        public void TestParseNatural113()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 minutes");
            Assert.AreEqual(new TimeSpan(0, 0, 2, 30, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural114()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 hours");
            Assert.AreEqual(new TimeSpan(0, 2, 30, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural115()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5 days");
            Assert.AreEqual(new TimeSpan(2, 12, 0, 0, 0), actual);
        }

        [TestMethod]
        public void TestParseNatural116()
        {
            TimeSpan actual = TimeSpanUtility.ParseNatural("2.5d 2.5h 2.5m 2.5s");
            Assert.AreEqual(new TimeSpan(2, 14, 32, 32, 500), actual);
        }

        #endregion

        #region TestToNaturalString

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestToNaturalString00()
        {
            string actual = TimeSpanUtility.ToNaturalString(TimeSpan.FromTicks(-1));
        }

        [TestMethod]
        public void TestToNaturalString01()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 0));
            Assert.AreEqual("0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString02()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 1));
            Assert.AreEqual("1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString03()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 12));
            Assert.AreEqual("12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString04()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 60));
            Assert.AreEqual("1 minute 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString05()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 1920));
            Assert.AreEqual("32 minutes 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString06()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61));
            Assert.AreEqual("1 minute 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString07()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 1921));
            Assert.AreEqual("32 minutes 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString08()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 72));
            Assert.AreEqual("1 minute 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString09()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 1932));
            Assert.AreEqual("32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString10()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3600));
            Assert.AreEqual("1 hour 0 minutes 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString11()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3660));
            Assert.AreEqual("1 hour 1 minute 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString12()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 5520));
            Assert.AreEqual("1 hour 32 minutes 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString13()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3601));
            Assert.AreEqual("1 hour 0 minutes 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString14()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3661));
            Assert.AreEqual("1 hour 1 minute 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString15()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 5521));
            Assert.AreEqual("1 hour 32 minutes 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString16()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3612));
            Assert.AreEqual("1 hour 0 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString17()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3672));
            Assert.AreEqual("1 hour 1 minute 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString18()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 5532));
            Assert.AreEqual("1 hour 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString19()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61200));
            Assert.AreEqual("17 hours 0 minutes 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString20()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61260));
            Assert.AreEqual("17 hours 1 minute 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString21()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 63120));
            Assert.AreEqual("17 hours 32 minutes 0 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString22()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61201));
            Assert.AreEqual("17 hours 0 minutes 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString23()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61261));
            Assert.AreEqual("17 hours 1 minute 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString24()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 63121));
            Assert.AreEqual("17 hours 32 minutes 1 second", actual);
        }

        [TestMethod]
        public void TestToNaturalString25()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61212));
            Assert.AreEqual("17 hours 0 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString26()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 61272));
            Assert.AreEqual("17 hours 1 minute 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString27()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 63132));
            Assert.AreEqual("17 hours 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString28()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 88332));
            Assert.AreEqual("1 day 0 hours 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString29()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3803532));
            Assert.AreEqual("44 days 0 hours 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString30()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 91932));
            Assert.AreEqual("1 day 1 hour 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString31()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3807132));
            Assert.AreEqual("44 days 1 hour 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString32()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 149532));
            Assert.AreEqual("1 day 17 hours 32 minutes 12 seconds", actual);
        }

        [TestMethod]
        public void TestToNaturalString33()
        {
            string actual = TimeSpanUtility.ToNaturalString(new TimeSpan(0, 0, 3864732));
            Assert.AreEqual("44 days 17 hours 32 minutes 12 seconds", actual);
        }

        #endregion
    }
}
