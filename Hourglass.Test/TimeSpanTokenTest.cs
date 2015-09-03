// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTokenTest.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Test
{
    using System;
    using System.Globalization;

    using Hourglass.Parsing;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains tests for the <see cref="TimeSpanToken"/> class.
    /// </summary>
    [TestClass]
    public class TimeSpanTokenTest
    {
        #region Parse Invalid Input

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> throws a <see
        /// cref="ArgumentNullException"/> for <c>null</c> input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseWithNullInputThrowsArgumentNullException()
        {
            // Arrange
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimeSpanToken.Parser.Instance.Parse(null /* str */, provider);

            // Assert
            Assert.Fail("Expected ArgumentNullException.");
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> throws a <see
        /// cref="FormatException"/> for <see cref="string.Empty"/> input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseWithEmptyInputThrowsFormatException()
        {
            // Arrange
            string str = string.Empty;
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            Assert.Fail("Expected FormatException.");
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> throws a <see
        /// cref="FormatException"/> for a garbage string input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseWithGarbageInputThrowsFormatException()
        {
            // Arrange
            string str = "garbage";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            Assert.Fail("Expected FormatException.");
        }

        #endregion

        #region Parse Minutes Only

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 0 seconds for
        /// <c>"0"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0InputReturns0Seconds()
        {
            // Arrange
            string str = "0";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual /* 0 seconds */);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 5 minutes for
        /// <c>"5"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith5InputReturns5Minutes()
        {
            // Arrange
            string str = "5";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 90 minutes for
        /// <c>"90"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith90InputReturns90Minutes()
        {
            // Arrange
            string str = "90";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 90);
        }

        #endregion

        #region Parse Short Form

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// for <c>"15 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Space30InputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// for <c>"15.30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Dot30InputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15.30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// for <c>"15:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Colon30InputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15:30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes 30
        /// seconds for <c>"72 15 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Space15Space30InputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72 15 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes 30
        /// seconds for <c>"72:15:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Dot15Dot30InputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72.15.30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes 30
        /// seconds for <c>"72:15:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Colon15Colon30InputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72:15:30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38 days 72 hours 15
        /// minutes 30 seconds for <c>"38 72 15 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38Space72Space15Space30InputReturns38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "38 72 15 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38 days 72 hours 15
        /// minutes 30 seconds for <c>"38.72.15.30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38Dot72Dot15Dot30InputReturns38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "38.72.15.30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38 days 72 hours 15
        /// minutes 30 seconds for <c>"38:72:15:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38Colon72Colon15Colon30InputReturns38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "38:72:15:30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94 months 38 days 72
        /// hours 15 minutes 30 seconds for <c>"94 38 72 15 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94Space38Space72Space15Space30InputReturns94Months38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "94 38 72 15 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94 months 38 days 72
        /// hours 15 minutes 30 seconds for <c>"94.38.72.15.30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94Dot38Dot72Dot15Dot30InputReturns94Months38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "94.38.72.15.30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94 months 38 days 72
        /// hours 15 minutes 30 seconds for <c>"94:38:72:15:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94Colon38Colon72Colon15Colon30InputReturns94Months38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "94:38:72:15:30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21 years 94 months 38
        /// days 72 hours 15 minutes 30 seconds for <c>"21 94 38 72 15 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21Space94Space38Space72Space15Space30InputReturns21Years94Months38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "21 94 38 72 15 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21, expectedMonths: 94, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21 years 94 months 38
        /// days 72 hours 15 minutes 30 seconds for <c>"21.94.38.72.15.30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21Dot94Dot38Dot72Dot15Dot30InputReturns21Years94Months38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "21.94.38.72.15.30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21, expectedMonths: 94, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21 years 94 months 38
        /// days 72 hours 15 minutes 30 seconds for <c>"21:94:38:72:15:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21Colon94Colon38Colon72Colon15Colon30InputReturns21Years94Months38Days72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "21:94:38:72:15:30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21, expectedMonths: 94, expectedDays: 38, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        #endregion

        #region Parse Long Form (One Unit)

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 30 seconds with
        /// <c>"30s"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith30SInputReturns30Seconds()
        {
            // Arrange
            string str = "30s";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 30 seconds with
        /// <c>"30sec"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith30SecInputReturns30Seconds()
        {
            // Arrange
            string str = "30sec";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 30 seconds with <c>"30
        /// seconds"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith30SecondsInputReturns30Seconds()
        {
            // Arrange
            string str = "30 seconds";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes with
        /// <c>"15m"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15MInputReturns15Minutes()
        {
            // Arrange
            string str = "15m";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes with
        /// <c>"15min"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15MinInputReturns15Minutes()
        {
            // Arrange
            string str = "15min";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes with <c>"15
        /// minutes"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15MinutesInputReturns15Minutes()
        {
            // Arrange
            string str = "15 minutes";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours with
        /// <c>"72h"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72HInputReturns72Hours()
        {
            // Arrange
            string str = "72h";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours with
        /// <c>"72hr"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72HrInputReturns72Hours()
        {
            // Arrange
            string str = "72hr";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours with <c>"72
        /// hours"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72HoursInputReturns72Hours()
        {
            // Arrange
            string str = "72 hours";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38 days with
        /// <c>"38d"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38DInputReturns38Days()
        {
            // Arrange
            string str = "38d";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38 days with
        /// <c>"38dy"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38DyInputReturns38Days()
        {
            // Arrange
            string str = "38dy";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38 days with <c>"38
        /// days"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38DaysInputReturns38Days()
        {
            // Arrange
            string str = "38 days";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 54 weeks with
        /// <c>"54w"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith54WInputReturns54Weeks()
        {
            // Arrange
            string str = "54w";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedWeeks: 54);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 54 weeks with
        /// <c>"54wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith54WkInputReturns54Weeks()
        {
            // Arrange
            string str = "54wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedWeeks: 54);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 54 weeks with <c>"54
        /// weeks"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith54WeeksInputReturns54Weeks()
        {
            // Arrange
            string str = "54 weeks";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedWeeks: 54);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94 months with
        /// <c>"94mo"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94MoInputReturns94Months()
        {
            // Arrange
            string str = "94mo";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94 months with
        /// <c>"94mon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94MonInputReturns94Months()
        {
            // Arrange
            string str = "94mon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94 months with <c>"94
        /// months"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94MonthsInputReturns94Months()
        {
            // Arrange
            string str = "94 months";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21 years with
        /// <c>"21y"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21YInputReturns21Years()
        {
            // Arrange
            string str = "21y";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21 years with
        /// <c>"21yr"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21YrInputReturns21Years()
        {
            // Arrange
            string str = "21yr";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21 years with <c>"21
        /// years"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21YearsInputReturns21Years()
        {
            // Arrange
            string str = "21 years";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21);
        }

        #endregion

        #region Parse Long Form (Multiple Units)

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// with <c>"15m30s"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15M30SInputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15m30s";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// with <c>"15min 30sec"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Min30SecInputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15min 30sec";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// with <c>"15 minutes 30 seconds"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Minutes30SecondsInputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15 minutes 30 seconds";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// with <c>"72h15m"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72H15MInputReturns72Hours15Minutes()
        {
            // Arrange
            string str = "72h15m";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// with <c>"72hr 15min"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hr15MinInputReturns72Hours15Minutes()
        {
            // Arrange
            string str = "72hr 15min";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// with <c>"72 hours 15 minutes"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hours15MinutesInputReturns72Hours15Minutes()
        {
            // Arrange
            string str = "72 hours 15 minutes";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// 30 seconds with <c>"72h15m30s"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72H15M30SInputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72h15m30s";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// 30 seconds with <c>"72hr 15min 30sec"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hr15Min30SecInputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72hr 15min 30sec";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// 30 seconds with <c>"72 hours 15 minutes 30 seconds"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hours15Minutes30SecondsInputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72 hours 15 minutes 30 seconds";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        #endregion

        #region Parse Long Form (Inferred Units)

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// with <c>"15m30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15M30InputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15m30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// with <c>"15min 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Min30InputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15min 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15 minutes 30 seconds
        /// with <c>"15 minutes 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Minutes30InputReturns15Minutes30Seconds()
        {
            // Arrange
            string str = "15 minutes 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// with <c>"72h15"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72H15InputReturns72Hours15Minutes()
        {
            // Arrange
            string str = "72h15";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// with <c>"72hr 15"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hr15InputReturns72Hours15Minutes()
        {
            // Arrange
            string str = "72hr 15";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// with <c>"72 hours 15"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hours15InputReturns72Hours15Minutes()
        {
            // Arrange
            string str = "72 hours 15";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// 30 seconds with <c>"72h15m30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72H15M30InputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72h15m30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// 30 seconds with <c>"72hr 15min 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hr15Min30InputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72hr 15min 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72 hours 15 minutes
        /// 30 seconds with <c>"72 hours 15 minutes 30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Hours15Minutes30InputReturns72Hours15Minutes30Seconds()
        {
            // Arrange
            string str = "72 hours 15 minutes 30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72, expectedMinutes: 15, expectedSeconds: 30);
        }

        #endregion

        #region Parse Long Form (Fractional Units)

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 30.5 seconds with
        /// <c>"30.5s"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith30Point5SInputReturns30Seconds()
        {
            // Arrange
            string str = "30.5s";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedSeconds: 30.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 30.5 seconds with
        /// <c>"30.5sec"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith30Point5SecInputReturns30Seconds()
        {
            // Arrange
            string str = "30.5sec";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedSeconds: 30.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 30.5 seconds with
        /// <c>"30.5 seconds"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith30Point5SecondsInputReturns30Seconds()
        {
            // Arrange
            string str = "30.5 seconds";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedSeconds: 30.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15.5 minutes with
        /// <c>"15.5m"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Point5MInputReturns15Minutes()
        {
            // Arrange
            string str = "15.5m";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15.5 minutes with
        /// <c>"15.5min"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Point5MinInputReturns15Minutes()
        {
            // Arrange
            string str = "15.5min";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 15.5 minutes with
        /// <c>"15.5 minutes"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15Point5MinutesInputReturns15Minutes()
        {
            // Arrange
            string str = "15.5 minutes";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMinutes: 15.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72.5 hours with
        /// <c>"72.5h"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Point5HInputReturns72Hours()
        {
            // Arrange
            string str = "72.5h";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72.5 hours with
        /// <c>"72.5hr"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Point5HrInputReturns72Hours()
        {
            // Arrange
            string str = "72.5hr";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 72.5 hours with
        /// <c>"72.5 hours"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith72Point5HoursInputReturns72Hours()
        {
            // Arrange
            string str = "72.5 hours";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedHours: 72.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38.5 days with
        /// <c>"38.5d"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38Point5DInputReturns38Days()
        {
            // Arrange
            string str = "38.5d";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38.5 days with
        /// <c>"38.5dy"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38Point5DyInputReturns38Days()
        {
            // Arrange
            string str = "38.5dy";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 38.5 days with
        /// <c>"38.5 days"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith38Point5DaysInputReturns38Days()
        {
            // Arrange
            string str = "38.5 days";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedDays: 38.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 54.5 weeks with
        /// <c>"54.5w"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith54Point5WInputReturns54Weeks()
        {
            // Arrange
            string str = "54.5w";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedWeeks: 54.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 54.5 weeks with
        /// <c>"5.54wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith54Point5WkInputReturns54Weeks()
        {
            // Arrange
            string str = "54.5wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedWeeks: 54.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 54.5 weeks with
        /// <c>"54.5 weeks"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith54Point5WeeksInputReturns54Weeks()
        {
            // Arrange
            string str = "54.5 weeks";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedWeeks: 54.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94.5 months with
        /// <c>"94.5mo"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94Point5MoInputReturns94Months()
        {
            // Arrange
            string str = "94.5mo";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94.5 months with
        /// <c>"94.5mon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94Point5MonInputReturns94Months()
        {
            // Arrange
            string str = "94.5mon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 94.5 months with
        /// <c>"94.5 months"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith94Point5MonthsInputReturns94Months()
        {
            // Arrange
            string str = "94.5 months";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedMonths: 94.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21.5 years with
        /// <c>"21.5y"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21Point5YInputReturns21Years()
        {
            // Arrange
            string str = "21.5y";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21.5 years with
        /// <c>"21.5yr"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21Point5YrInputReturns21Years()
        {
            // Arrange
            string str = "21.5yr";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21.5);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.Parser.Parse(string,IFormatProvider)"/> returns 21.5 years with
        /// <c>"21.5 years"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21Point5YearsInputReturns21Years()
        {
            // Arrange
            string str = "21.5 years";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = TimeSpanToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(actual, expectedYears: 21.5);
        }

        #endregion

        #region Get End Time (Integer Units)

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2015 at 12:00:05 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 seconds and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5SecondsAnd20150101At000000Returns20150101At000005()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Seconds = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 5), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2015 at 12:05:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 minutes and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5MinutesAnd20150101At000000Returns20150101At000500()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Minutes = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 5, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2015 at 05:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 hours and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5HoursAnd20150101At000000Returns20150101At050000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Hours = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 5, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 6, 2015 at 12:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 days and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5DaysAnd20150101At000000Returns20150106At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Days = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 6, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns February 5, 2015 at 12:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 weeks and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5WeeksAnd20150101At000000Returns20150205At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Weeks = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 2, 5, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns June 1, 2015 at 12:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 months and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5MonthsAnd20150101At000000Returns20150601At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Months = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 6, 1, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2020 at 12:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 5 years and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith5YearsAnd20150101At000000Returns20200101At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Years = 5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2020, 1, 1, 0, 0, 0), endTime);
        }

        #endregion

        #region Get End Time (Fractional Units)

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2015 at 12:00:01.500 a.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 seconds and start time of January 1, 2015 at 12:00:00.00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5SecondsAnd20150101At000000000Returns20150101At000001500()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Seconds = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 0, 1, 500), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2015 at 12:01:30 a.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 minutes and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5MinutesAnd20150101At000000Returns20150101At000130()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Minutes = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 0, 1, 30), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 1, 2015 at 01:30:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 hours and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5HoursAnd20150101At000000Returns20150101At013000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Hours = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 1, 30, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 2, 2015 at 12:00:00 p.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 days and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5DaysAnd20150101At000000Returns20150102At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Days = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 2, 12, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns January 11, 2015 at 12:00:00 p.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 weeks and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5WeeksAnd20150101At000000Returns20150111At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Weeks = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 11, 12, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns February 15, 2015 at 12:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 months and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5MonthsAnd20150101At000000Returns20150215At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Months = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 2, 15, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="TimeSpanToken.GetEndTime"/> returns July 1, 2016 at 12:00:00 a.m. for a <see
        /// cref="TimeSpanToken"/> for 1.5 years and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith1Point5YearsAnd20150101At000000Returns20160701At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            TimeSpanToken timeSpanToken = new TimeSpanToken { Years = 1.5 };

            // Act
            DateTime endTime = timeSpanToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2016, 7, 1, 0, 0, 0), endTime);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Asserts that a <see cref="TimerStartToken"/> is an instance of the <see cref="TimeSpanToken"/> class and
        /// that its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="TimerStartToken"/>.</param>
        /// <param name="expectedYears">The expected years.</param>
        /// <param name="expectedMonths">The expected months.</param>
        /// <param name="expectedWeeks">The expected weeks.</param>
        /// <param name="expectedDays">The expected days.</param>
        /// <param name="expectedHours">The expected hours.</param>
        /// <param name="expectedMinutes">The expected minutes.</param>
        /// <param name="expectedSeconds">The expected seconds.</param>
        private static void AssertAreEqual(TimerStartToken actual, double expectedYears = 0, double expectedMonths = 0, double expectedWeeks = 0, double expectedDays = 0, double expectedHours = 0, double expectedMinutes = 0, double expectedSeconds = 0)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(typeof(TimeSpanToken), actual.GetType());
            Assert.AreEqual(expectedYears, ((TimeSpanToken)actual).Years);
            Assert.AreEqual(expectedMonths, ((TimeSpanToken)actual).Months);
            Assert.AreEqual(expectedWeeks, ((TimeSpanToken)actual).Weeks);
            Assert.AreEqual(expectedDays, ((TimeSpanToken)actual).Days);
            Assert.AreEqual(expectedHours, ((TimeSpanToken)actual).Hours);
            Assert.AreEqual(expectedMinutes, ((TimeSpanToken)actual).Minutes);
            Assert.AreEqual(expectedSeconds, ((TimeSpanToken)actual).Seconds);
        }

        #endregion
    }
}
