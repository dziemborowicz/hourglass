// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTokenTest.cs" company="Chris Dziemborowicz">
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
    /// Contains tests for the <see cref="DateTimeToken"/> class.
    /// </summary>
    [TestClass]
    public class DateTimeTokenTest
    {
        #region Parse Invalid Input

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> throws a <see
        /// cref="ArgumentNullException"/> for <c>null</c> input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseWithNullInputThrowsArgumentNullException()
        {
            // Arrange
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            DateTimeToken.Parser.Instance.Parse(null /* str */, provider);

            // Assert
            Assert.Fail("Expected ArgumentNullException.");
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> throws a <see
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
            DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            Assert.Fail("Expected FormatException.");
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> throws a <see
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
            DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            Assert.Fail("Expected FormatException.");
        }

        #endregion

        #region Parse Day of Week (Next)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Monday with
        /// <c>"mon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMonInputReturnsNextMonday()
        {
            // Arrange
            string str = "mon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Monday with
        /// <c>"Monday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMondayInputReturnsNextMonday()
        {
            // Arrange
            string str = "Monday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Monday with
        /// <c>"next mon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextMonInputReturnsNextMonday()
        {
            // Arrange
            string str = "next mon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Monday with
        /// <c>"next Monday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextMondayInputReturnsNextMonday()
        {
            // Arrange
            string str = "next Monday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Monday with
        /// <c>"this mon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisMonInputReturnsNextMonday()
        {
            // Arrange
            string str = "this mon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Monday with
        /// <c>"this Monday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisMondayInputReturnsNextMonday()
        {
            // Arrange
            string str = "this Monday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Tuesday with
        /// <c>"tue"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTueInputReturnsNextTuesday()
        {
            // Arrange
            string str = "tue";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Tuesday with
        /// <c>"Tuesday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTuesdayInputReturnsNextTuesday()
        {
            // Arrange
            string str = "Tuesday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Tuesday with
        /// <c>"next tue"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextTueInputReturnsNextTuesday()
        {
            // Arrange
            string str = "next tue";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Tuesday with
        /// <c>"next Tuesday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextTuesdayInputReturnsNextTuesday()
        {
            // Arrange
            string str = "next Tuesday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Tuesday with
        /// <c>"this tue"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisTueInputReturnsNextTuesday()
        {
            // Arrange
            string str = "this tue";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Tuesday with
        /// <c>"this Tuesday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisTuesdayInputReturnsNextTuesday()
        {
            // Arrange
            string str = "this Tuesday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Wednesday with
        /// <c>"wed"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWedInputReturnsNextWednesday()
        {
            // Arrange
            string str = "wed";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Wednesday with
        /// <c>"Wednesday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWednesdayInputReturnsNextWednesday()
        {
            // Arrange
            string str = "Wednesday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Wednesday with
        /// <c>"next wed"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextWedInputReturnsNextWednesday()
        {
            // Arrange
            string str = "next wed";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Wednesday with
        /// <c>"next Wednesday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextWednesdayInputReturnsNextWednesday()
        {
            // Arrange
            string str = "next Wednesday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Wednesday with
        /// <c>"this wed"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisWedInputReturnsNextWednesday()
        {
            // Arrange
            string str = "this wed";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Wednesday with
        /// <c>"this Wednesday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisWednesdayInputReturnsNextWednesday()
        {
            // Arrange
            string str = "this Wednesday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Thursday with
        /// <c>"thu"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThuInputReturnsNextThursday()
        {
            // Arrange
            string str = "thu";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Thursday with
        /// <c>"Thursday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThursdayInputReturnsNextThursday()
        {
            // Arrange
            string str = "Thursday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Thursday with
        /// <c>"next thu"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextThuInputReturnsNextThursday()
        {
            // Arrange
            string str = "next thu";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Thursday with
        /// <c>"next Thursday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextThursdayInputReturnsNextThursday()
        {
            // Arrange
            string str = "next Thursday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Thursday with
        /// <c>"this thu"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisThuInputReturnsNextThursday()
        {
            // Arrange
            string str = "this thu";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Thursday with
        /// <c>"this Thursday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisThursdayInputReturnsNextThursday()
        {
            // Arrange
            string str = "this Thursday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday with
        /// <c>"fri"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFriInputReturnsNextFriday()
        {
            // Arrange
            string str = "fri";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday with
        /// <c>"Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayInputReturnsNextFriday()
        {
            // Arrange
            string str = "Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday with
        /// <c>"next fri"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextFriInputReturnsNextFriday()
        {
            // Arrange
            string str = "next fri";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday with
        /// <c>"next Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextFridayInputReturnsNextFriday()
        {
            // Arrange
            string str = "next Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday with
        /// <c>"this fri"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisFriInputReturnsNextFriday()
        {
            // Arrange
            string str = "this fri";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday with
        /// <c>"this Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisFridayInputReturnsNextFriday()
        {
            // Arrange
            string str = "this Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Saturday with
        /// <c>"sat"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSatInputReturnsNextSaturday()
        {
            // Arrange
            string str = "sat";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Saturday with
        /// <c>"Saturday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSaturdayInputReturnsNextSaturday()
        {
            // Arrange
            string str = "Saturday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Saturday with
        /// <c>"next sat"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextSatInputReturnsNextSaturday()
        {
            // Arrange
            string str = "next sat";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Saturday with
        /// <c>"next Saturday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextSaturdayInputReturnsNextSaturday()
        {
            // Arrange
            string str = "next Saturday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Saturday with
        /// <c>"this sat"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisSatInputReturnsNextSaturday()
        {
            // Arrange
            string str = "this sat";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Saturday with
        /// <c>"this Saturday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisSaturdayInputReturnsNextSaturday()
        {
            // Arrange
            string str = "this Saturday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Sunday with
        /// <c>"sun"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSunInputReturnsNextSunday()
        {
            // Arrange
            string str = "sun";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Sunday with
        /// <c>"Sunday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSundayInputReturnsNextSunday()
        {
            // Arrange
            string str = "Sunday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Sunday with
        /// <c>"next sun"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextSunInputReturnsNextSunday()
        {
            // Arrange
            string str = "next sun";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Sunday with
        /// <c>"next Sunday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNextSundayInputReturnsNextSunday()
        {
            // Arrange
            string str = "next Sunday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Sunday with
        /// <c>"this sun"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisSunInputReturnsNextSunday()
        {
            // Arrange
            string str = "this sun";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Sunday with
        /// <c>"this Sunday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThisSundayInputReturnsNextSunday()
        {
            // Arrange
            string str = "this Sunday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.Next);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Day of Week (After Next)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday after next with
        /// <c>"mon next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMonNextInputReturnsMondayAfterNext()
        {
            // Arrange
            string str = "mon next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday after next with
        /// <c>"Monday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMondayNextInputReturnsMondayAfterNext()
        {
            // Arrange
            string str = "Monday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday after next with
        /// <c>"mon after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMonAfterNextInputReturnsMondayAfterNext()
        {
            // Arrange
            string str = "mon after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday after next with
        /// <c>"Monday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMondayAfterNextInputReturnsMondayAfterNext()
        {
            // Arrange
            string str = "Monday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday after next with
        /// <c>"tue next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTueNextInputReturnsTuesdayAfterNext()
        {
            // Arrange
            string str = "tue next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday after next with
        /// <c>"Tuesday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTuesdayNextInputReturnsTuesdayAfterNext()
        {
            // Arrange
            string str = "Tuesday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday after next with
        /// <c>"tue after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTueAfterNextInputReturnsTuesdayAfterNext()
        {
            // Arrange
            string str = "tue after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday after next with
        /// <c>"Tuesday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTuesdayAfterNextInputReturnsTuesdayAfterNext()
        {
            // Arrange
            string str = "Tuesday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday after next with
        /// <c>"wed next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWedNextInputReturnsWednesdayAfterNext()
        {
            // Arrange
            string str = "wed next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday after next with
        /// <c>"Wednesday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWednesdayNextInputReturnsWednesdayAfterNext()
        {
            // Arrange
            string str = "Wednesday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday after next with
        /// <c>"wed after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWedAfterNextInputReturnsWednesdayAfterNext()
        {
            // Arrange
            string str = "wed after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday after next with
        /// <c>"Wednesday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWednesdayAfterNextInputReturnsWednesdayAfterNext()
        {
            // Arrange
            string str = "Wednesday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday after next with
        /// <c>"thu next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThuNextInputReturnsThursdayAfterNext()
        {
            // Arrange
            string str = "thu next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday after next with
        /// <c>"Thursday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThursdayNextInputReturnsThursdayAfterNext()
        {
            // Arrange
            string str = "Thursday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday after next with
        /// <c>"thu after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThuAfterNextInputReturnsThursdayAfterNext()
        {
            // Arrange
            string str = "thu after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday after next with
        /// <c>"Thursday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThursdayAfterNextInputReturnsThursdayAfterNext()
        {
            // Arrange
            string str = "Thursday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday after next with
        /// <c>"fri next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFriNextInputReturnsFridayAfterNext()
        {
            // Arrange
            string str = "fri next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday after next with
        /// <c>"Friday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayNextInputReturnsFridayAfterNext()
        {
            // Arrange
            string str = "Friday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday after next with
        /// <c>"fri after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFriAfterNextInputReturnsFridayAfterNext()
        {
            // Arrange
            string str = "fri after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday after next with
        /// <c>"Friday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayAfterNextInputReturnsFridayAfterNext()
        {
            // Arrange
            string str = "Friday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday after next with
        /// <c>"sat next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSatNextInputReturnsSaturdayAfterNext()
        {
            // Arrange
            string str = "sat next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday after next with
        /// <c>"Saturday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSaturdayNextInputReturnsSaturdayAfterNext()
        {
            // Arrange
            string str = "Saturday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday after next with
        /// <c>"sat after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSatAfterNextInputReturnsSaturdayAfterNext()
        {
            // Arrange
            string str = "sat after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday after next with
        /// <c>"Saturday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSaturdayAfterNextInputReturnsSaturdayAfterNext()
        {
            // Arrange
            string str = "Saturday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday after next with
        /// <c>"sun next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSunNextInputReturnsSundayAfterNext()
        {
            // Arrange
            string str = "sun next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday after next with
        /// <c>"Sunday next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSundayNextInputReturnsSundayAfterNext()
        {
            // Arrange
            string str = "Sunday next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday after next with
        /// <c>"sun after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSunAfterNextInputReturnsSundayAfterNext()
        {
            // Arrange
            string str = "sun after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday after next with
        /// <c>"Sunday after next"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSundayAfterNextInputReturnsSundayAfterNext()
        {
            // Arrange
            string str = "Sunday after next";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.AfterNext);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Day of Week (After Next)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday next week with
        /// <c>"mon next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMonNextWkInputReturnsMondayNextWeek()
        {
            // Arrange
            string str = "mon next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday next week with
        /// <c>"Monday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMondayNextWkInputReturnsMondayNextWeek()
        {
            // Arrange
            string str = "Monday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday next week with
        /// <c>"mon next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMonNextWeekInputReturnsMondayNextWeek()
        {
            // Arrange
            string str = "mon next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Monday next week with
        /// <c>"Monday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMondayNextWeekInputReturnsMondayNextWeek()
        {
            // Arrange
            string str = "Monday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Monday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday next week with
        /// <c>"tue next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTueNextWkInputReturnsTuesdayNextWeek()
        {
            // Arrange
            string str = "tue next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday next week with
        /// <c>"Tuesday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTuesdayNextWkInputReturnsTuesdayNextWeek()
        {
            // Arrange
            string str = "Tuesday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday next week with
        /// <c>"tue next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTueNextWeekInputReturnsTuesdayNextWeek()
        {
            // Arrange
            string str = "tue next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Tuesday next week with
        /// <c>"Tuesday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTuesdayNextWeekInputReturnsTuesdayNextWeek()
        {
            // Arrange
            string str = "Tuesday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Tuesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday next week with
        /// <c>"wed next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWedNextWkInputReturnsWednesdayNextWeek()
        {
            // Arrange
            string str = "wed next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday next week with
        /// <c>"Wednesday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWednesdayNextWkInputReturnsWednesdayNextWeek()
        {
            // Arrange
            string str = "Wednesday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday next week with
        /// <c>"wed next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWedNextWeekInputReturnsWednesdayNextWeek()
        {
            // Arrange
            string str = "wed next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Wednesday next week with
        /// <c>"Wednesday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithWednesdayNextWeekInputReturnsWednesdayNextWeek()
        {
            // Arrange
            string str = "Wednesday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Wednesday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday next week with
        /// <c>"thu next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThuNextWkInputReturnsThursdayNextWeek()
        {
            // Arrange
            string str = "thu next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday next week with
        /// <c>"Thursday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThursdayNextWkInputReturnsThursdayNextWeek()
        {
            // Arrange
            string str = "Thursday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday next week with
        /// <c>"thu next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThuNextWeekInputReturnsThursdayNextWeek()
        {
            // Arrange
            string str = "thu next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Thursday next week with
        /// <c>"Thursday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThursdayNextWeekInputReturnsThursdayNextWeek()
        {
            // Arrange
            string str = "Thursday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Thursday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday next week with
        /// <c>"fri next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFriNextWkInputReturnsFridayNextWeek()
        {
            // Arrange
            string str = "fri next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday next week with
        /// <c>"Friday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayNextWkInputReturnsFridayNextWeek()
        {
            // Arrange
            string str = "Friday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday next week with
        /// <c>"fri next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFriNextWeekInputReturnsFridayNextWeek()
        {
            // Arrange
            string str = "fri next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Friday next week with
        /// <c>"Friday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayNextWeekInputReturnsFridayNextWeek()
        {
            // Arrange
            string str = "Friday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Friday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday next week with
        /// <c>"sat next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSatNextWkInputReturnsSaturdayNextWeek()
        {
            // Arrange
            string str = "sat next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday next week with
        /// <c>"Saturday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSaturdayNextWkInputReturnsSaturdayNextWeek()
        {
            // Arrange
            string str = "Saturday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday next week with
        /// <c>"sat next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSatNextWeekInputReturnsSaturdayNextWeek()
        {
            // Arrange
            string str = "sat next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Saturday next week with
        /// <c>"Saturday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSaturdayNextWeekInputReturnsSaturdayNextWeek()
        {
            // Arrange
            string str = "Saturday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Saturday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday next week with
        /// <c>"sun next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSunNextWkInputReturnsSundayNextWeek()
        {
            // Arrange
            string str = "sun next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday next week with
        /// <c>"Sunday next wk"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSundayNextWkInputReturnsSundayNextWeek()
        {
            // Arrange
            string str = "Sunday next wk";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday next week with
        /// <c>"sun next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSunNextWeekInputReturnsSundayNextWeek()
        {
            // Arrange
            string str = "sun next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Sunday next week with
        /// <c>"Sunday next week"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSundayNextWeekInputReturnsSundayNextWeek()
        {
            // Arrange
            string str = "Sunday next week";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), DayOfWeek.Sunday, DayOfWeekRelation.NextWeek);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Noon

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonInputReturnsNoon()
        {
            // Arrange
            string str = "noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12NoonInputReturnsNoon()
        {
            // Arrange
            string str = "12noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12 noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12SpaceNoonInputReturnsNoon()
        {
            // Arrange
            string str = "12 noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12.00noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00NoonInputReturnsNoon()
        {
            // Arrange
            string str = "12.00noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12.00 noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00SpaceNoonInputReturnsNoon()
        {
            // Arrange
            string str = "12.00 noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12:00noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00NoonInputReturnsNoon()
        {
            // Arrange
            string str = "12:00noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12:00 noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00SpaceNoonInputReturnsNoon()
        {
            // Arrange
            string str = "12:00 noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12.00.00noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00Dot00NoonInputReturnsNoon()
        {
            // Arrange
            string str = "12.00.00noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12.00.00 noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00Dot00SpaceNoonInputReturnsNoon()
        {
            // Arrange
            string str = "12.00.00 noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns noon with
        /// <c>"12:00:00 noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00Colon00SpaceNoonInputReturnsNoon()
        {
            // Arrange
            string str = "12:00:00 noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMiddayInputReturnsMidday()
        {
            // Arrange
            string str = "midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12MiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12 midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12SpaceMiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12 midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12.00midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00MiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12.00midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12.00 midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00SpaceMiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12.00 midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12:00midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00MiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12:00midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12:00 midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00SpaceMiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12:00 midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12.00.00midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00Dot00MiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12.00.00midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12.00.00 midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00Dot00SpaceMiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12.00.00 midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midday with
        /// <c>"12:00:00 midday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00Colon00SpaceMiddayInputReturnsMidday()
        {
            // Arrange
            string str = "12:00:00 midday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        #endregion

        #region Parse Midnight

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12MidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12 midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12SpaceMidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12 midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12.00midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00MidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12.00midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12.00 midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00SpaceMidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12.00 midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12:00midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00MidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12:00midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12:00 midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00SpaceMidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12:00 midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12.00.00midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00Dot00MidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12.00.00midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12.00.00 midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Dot00Dot00SpaceMidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12.00.00 midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns midnight with
        /// <c>"12:00:00 midnight"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12Colon00Colon00SpaceMidnightInputReturnsMidnight()
        {
            // Arrange
            string str = "12:00:00 midnight";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midnight);
        }

        #endregion

        #region Parse Hour

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 1:00:00 p.m. with
        /// <c>"1"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1InputReturns1Colon00Colon00Pm()
        {
            // Arrange
            string str = "1";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 1);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2InputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 3:00:00 p.m. with
        /// <c>"3"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith3InputReturns3Colon00Colon00Pm()
        {
            // Arrange
            string str = "3";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 3);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 4:00:00 p.m. with
        /// <c>"4"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith4InputReturns4Colon00Colon00Pm()
        {
            // Arrange
            string str = "4";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 4);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 5:00:00 p.m. with
        /// <c>"5"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith5InputReturns5Colon00Colon00Pm()
        {
            // Arrange
            string str = "5";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 5);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 6:00:00 p.m. with
        /// <c>"6"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith6InputReturns6Colon00Colon00Pm()
        {
            // Arrange
            string str = "6";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 6);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 7:00:00 p.m. with
        /// <c>"7"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith7InputReturns7Colon00Colon00Pm()
        {
            // Arrange
            string str = "7";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 7);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 8:00:00 a.m. with
        /// <c>"8"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith8InputReturns8Colon00Colon00Am()
        {
            // Arrange
            string str = "8";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 8);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 9:00:00 a.m. with
        /// <c>"9"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith9InputReturns9Colon00Colon00Am()
        {
            // Arrange
            string str = "9";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 9);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 10:00:00 a.m. with
        /// <c>"10"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith10InputReturns10Colon00Colon00Am()
        {
            // Arrange
            string str = "10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 10);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 11:00:00 a.m. with
        /// <c>"11"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith11InputReturns11Colon00Colon00Am()
        {
            // Arrange
            string str = "11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 11);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 12:00:00 p.m. with
        /// <c>"12"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith12InputReturns12Colon00Colon00Pm()
        {
            // Arrange
            string str = "12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 12);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 1:00:00 p.m. with
        /// <c>"13"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith13InputReturns1Colon00Colon00Pm()
        {
            // Arrange
            string str = "13";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 1);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"14"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14InputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "14";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 3:00:00 p.m. with
        /// <c>"15"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith15InputReturns3Colon00Colon00Pm()
        {
            // Arrange
            string str = "15";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 3);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 4:00:00 p.m. with
        /// <c>"16"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith16InputReturns4Colon00Colon00Pm()
        {
            // Arrange
            string str = "16";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 4);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 5:00:00 p.m. with
        /// <c>"17"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith17InputReturns5Colon00Colon00Pm()
        {
            // Arrange
            string str = "17";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 5);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 6:00:00 p.m. with
        /// <c>"18"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith18InputReturns6Colon00Colon00Pm()
        {
            // Arrange
            string str = "18";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 6);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 7:00:00 p.m. with
        /// <c>"19"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith19InputReturns7Colon00Colon00Pm()
        {
            // Arrange
            string str = "19";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 7);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 8:00:00 p.m. with
        /// <c>"20"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith20InputReturns8Colon00Colon00Pm()
        {
            // Arrange
            string str = "20";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 8);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 9:00:00 p.m. with
        /// <c>"21"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith21InputReturns9Colon00Colon00Pm()
        {
            // Arrange
            string str = "21";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 9);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 10:00:00 p.m. with
        /// <c>"22"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith22InputReturns10Colon00Colon00Pm()
        {
            // Arrange
            string str = "22";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 10);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 11:00:00 p.m. with
        /// <c>"23"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23InputReturns11Colon00Colon00Pm()
        {
            // Arrange
            string str = "23";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 11);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 1:00:00 a.m. with
        /// <c>"0100"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0100InputReturns1Colon00Colon00Am()
        {
            // Arrange
            string str = "0100";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 1);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"0200"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0200InputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "0200";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 3:00:00 a.m. with
        /// <c>"0300"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0300InputReturns3Colon00Colon00Am()
        {
            // Arrange
            string str = "0300";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 3);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 4:00:00 a.m. with
        /// <c>"0400"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0400InputReturns4Colon00Colon00Am()
        {
            // Arrange
            string str = "0400";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 4);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 5:00:00 a.m. with
        /// <c>"0500"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0500InputReturns5Colon00Colon00Am()
        {
            // Arrange
            string str = "0500";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 5);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 6:00:00 a.m. with
        /// <c>"0600"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0600InputReturns6Colon00Colon00Am()
        {
            // Arrange
            string str = "0600";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 6);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 7:00:00 a.m. with
        /// <c>"0700"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0700InputReturns7Colon00Colon00Am()
        {
            // Arrange
            string str = "0700";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 7);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 8:00:00 a.m. with
        /// <c>"0800"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0800InputReturns8Colon00Colon00Am()
        {
            // Arrange
            string str = "0800";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 8);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 9:00:00 a.m. with
        /// <c>"0900"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith0900InputReturns9Colon00Colon00Am()
        {
            // Arrange
            string str = "0900";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 9);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 10:00:00 a.m. with
        /// <c>"1000"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1000InputReturns10Colon00Colon00Am()
        {
            // Arrange
            string str = "1000";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 10);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 11:00:00 a.m. with
        /// <c>"1100"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1100InputReturns11Colon00Colon00Am()
        {
            // Arrange
            string str = "1100";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 11);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 12:00:00 p.m. with
        /// <c>"1200"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1200InputReturns12Colon00Colon00Pm()
        {
            // Arrange
            string str = "1200";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 12);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 1:00:00 p.m. with
        /// <c>"1300"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1300InputReturns1Colon00Colon00Pm()
        {
            // Arrange
            string str = "1300";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 1);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"1400"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1400InputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "1400";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 3:00:00 p.m. with
        /// <c>"1500"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1500InputReturns3Colon00Colon00Pm()
        {
            // Arrange
            string str = "1500";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 3);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 4:00:00 p.m. with
        /// <c>"1600"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1600InputReturns4Colon00Colon00Pm()
        {
            // Arrange
            string str = "1600";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 4);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 5:00:00 p.m. with
        /// <c>"1700"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1700InputReturns5Colon00Colon00Pm()
        {
            // Arrange
            string str = "1700";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 5);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 6:00:00 p.m. with
        /// <c>"1800"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1800InputReturns6Colon00Colon00Pm()
        {
            // Arrange
            string str = "1800";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 6);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 7:00:00 p.m. with
        /// <c>"1900"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1900InputReturns7Colon00Colon00Pm()
        {
            // Arrange
            string str = "1900";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 7);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 8:00:00 p.m. with
        /// <c>"2000"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2000InputReturns8Colon00Colon00Pm()
        {
            // Arrange
            string str = "2000";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 8);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 9:00:00 p.m. with
        /// <c>"2100"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2100InputReturns9Colon00Colon00Pm()
        {
            // Arrange
            string str = "2100";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 9);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 10:00:00 p.m. with
        /// <c>"2200"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2200InputReturns10Colon00Colon00Pm()
        {
            // Arrange
            string str = "2200";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 10);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 11:00:00 p.m. with
        /// <c>"2300"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2300InputReturns11Colon00Colon00Pm()
        {
            // Arrange
            string str = "2300";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 11);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"2a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2AInputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "2a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2PInputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"2 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2SpaceAInputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "2 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2SpacePInputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"2am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2AmInputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "2am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2PmInputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"2 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2SpaceAmInputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "2 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2SpacePmInputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"2a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2ADotMDotInputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "2a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2PDotMDotInputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 a.m. with
        /// <c>"2 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2SpaceADotMDotInputReturns2Colon00Colon00Am()
        {
            // Arrange
            string str = "2 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:00:00 p.m. with
        /// <c>"2 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2SpacePDotMDotInputReturns2Colon00Colon00Pm()
        {
            // Arrange
            string str = "2 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2);
        }

        #endregion

        #region Parse Hour and Minute

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230InputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"230a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230AInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "230a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230PInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"230 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230SpaceAInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "230 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230SpacePInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"230am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230AmInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "230am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230PmInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"230 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230SpaceAmInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "230 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230SpacePmInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"230a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230ADotMDotInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "230a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230PDotMDotInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"230 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230SpaceADotMDotInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "230 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"230 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith230SpacePDotMDotInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "230 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30InputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2.30a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30AInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2.30a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30PInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2.30 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30SpaceAInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2.30 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30SpacePInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2.30am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30AmInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2.30am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30PmInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2.30 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30SpaceAmInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2.30 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30SpacePmInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2.30a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30ADotMDotInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2.30a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30PDotMDotInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2.30 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30SpaceADotMDotInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2.30 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2.30 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30SpacePDotMDotInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2.30 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30InputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2:30a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30AInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2:30a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30PInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2:30 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30SpaceAInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2:30 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30SpacePInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2:30am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30AmInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2:30am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30PmInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2:30 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30SpaceAmInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2:30 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30SpacePmInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2:30a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30ADotMDotInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2:30a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30PDotMDotInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 a.m. with
        /// <c>"2:30 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30SpaceADotMDotInputReturns2Colon30Colon00Am()
        {
            // Arrange
            string str = "2:30 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:00 p.m. with
        /// <c>"2:30 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30SpacePDotMDotInputReturns2Colon30Colon00Pm()
        {
            // Arrange
            string str = "2:30 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30);
        }

        #endregion

        #region Parse Hour, Minute, and Second

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015InputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"23015a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015AInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "23015a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015PInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"23015 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015SpaceAInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "23015 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015SpacePInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"23015am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015AmInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "23015am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015PmInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"23015 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015SpaceAmInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "23015 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015SpacePmInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"23015a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015ADotMDotInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "23015a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015PDotMDotInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"23015 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015SpaceADotMDotInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "23015 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"23015 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith23015SpacePDotMDotInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "23015 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15InputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2.30.15a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15AInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2.30.15a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15PInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2.30.15 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15SpaceAInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2.30.15 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15SpacePInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2.30.15am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15AmInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2.30.15am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15PmInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2.30.15 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15SpaceAmInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2.30.15 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15SpacePmInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2.30.15a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15ADotMDotInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2.30.15a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15PDotMDotInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2.30.15 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15SpaceADotMDotInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2.30.15 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2.30.15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Dot30Dot15SpacePDotMDotInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2.30.15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15InputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Undefined, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2:30:15a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15AInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2:30:15a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2:30:15 a"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15SpaceAInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2:30:15 a";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15 p"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15SpacePInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2:30:15am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15AmInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2:30:15am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2:30:15 am"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15SpaceAmInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2:30:15 am";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15 pm"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15SpacePmInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 pm";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2:30:15a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15ADotMDotInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2:30:15a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PDotMDotInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 a.m. with
        /// <c>"2:30:15 a.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15SpaceADotMDotInputReturns2Colon30Colon15Am()
        {
            // Arrange
            string str = "2:30:15 a.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Am, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns 2:30:15 p.m. with
        /// <c>"2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15SpacePDotMDotInputReturns2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertIsEmpty(GetDateToken(actual));
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        #endregion

        #region Parse Day

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 1 with
        /// <c>"1st"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith1StInputReturnsDay1()
        {
            // Arrange
            string str = "1st";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 1);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 1 with
        /// <c>"the 1st"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThe1StInputReturnsDay1()
        {
            // Arrange
            string str = "the 1st";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 1);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 2 with
        /// <c>"2nd"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2NdInputReturnsDay2()
        {
            // Arrange
            string str = "2nd";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 2 with
        /// <c>"the 2nd"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThe2NdInputReturnsDay2()
        {
            // Arrange
            string str = "the 2nd";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 3 with
        /// <c>"3rd"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith3RdInputReturnsDay3()
        {
            // Arrange
            string str = "3rd";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 3);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 3 with
        /// <c>"the 3rd"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThe3RdInputReturnsDay3()
        {
            // Arrange
            string str = "the 3rd";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 3);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 14 with
        /// <c>"14th"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThInputReturnsDay14()
        {
            // Arrange
            string str = "14th";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns day 14 with
        /// <c>"the 14th"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithThe14ThInputReturnsDay14()
        {
            // Arrange
            string str = "the 14th";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Month

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns January with
        /// <c>"jan"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithJanInputReturnsJanuary()
        {
            // Arrange
            string str = "jan";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 1);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns January with
        /// <c>"January"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithJanuaryInputReturnsJanuary()
        {
            // Arrange
            string str = "January";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 1);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February with
        /// <c>"feb"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebInputReturnsFebruary()
        {
            // Arrange
            string str = "feb";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February with
        /// <c>"February"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruaryInputReturnsFebruary()
        {
            // Arrange
            string str = "February";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns March with
        /// <c>"mar"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMarInputReturnsMarch()
        {
            // Arrange
            string str = "mar";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 3);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns March with
        /// <c>"March"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMarchInputReturnsMarch()
        {
            // Arrange
            string str = "March";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 3);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns April with
        /// <c>"apr"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithAprInputReturnsApril()
        {
            // Arrange
            string str = "apr";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 4);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns April with
        /// <c>"April"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithAprilInputReturnsApril()
        {
            // Arrange
            string str = "April";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 4);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns May with
        /// <c>"may"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithMayInputReturnsMay()
        {
            // Arrange
            string str = "may";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 5);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns May with
        /// <c>"May"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithProperMayInputReturnsMay()
        {
            // Arrange
            string str = "May";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 5);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns June with
        /// <c>"jun"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithJunInputReturnsJune()
        {
            // Arrange
            string str = "jun";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 6);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns June with
        /// <c>"June"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithJuneInputReturnsJune()
        {
            // Arrange
            string str = "June";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 6);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns July with
        /// <c>"jul"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithJulInputReturnsJuly()
        {
            // Arrange
            string str = "jul";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 7);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns July with
        /// <c>"July"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithJulyInputReturnsJuly()
        {
            // Arrange
            string str = "July";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 7);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns August with
        /// <c>"aug"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithAugInputReturnsAugust()
        {
            // Arrange
            string str = "aug";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 8);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns August with
        /// <c>"August"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithAugustInputReturnsAugust()
        {
            // Arrange
            string str = "August";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 8);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns September with
        /// <c>"sep"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSepInputReturnsSeptember()
        {
            // Arrange
            string str = "sep";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 9);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns September with
        /// <c>"September"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithSeptemberInputReturnsSeptember()
        {
            // Arrange
            string str = "September";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 9);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October with
        /// <c>"oct"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithOctInputReturnsOctober()
        {
            // Arrange
            string str = "oct";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October with
        /// <c>"October"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithOctoberInputReturnsOctober()
        {
            // Arrange
            string str = "October";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November with
        /// <c>"nov"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNovInputReturnsNovember()
        {
            // Arrange
            string str = "nov";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November with
        /// <c>"November"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNovemberInputReturnsNovember()
        {
            // Arrange
            string str = "November";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns December with
        /// <c>"dec"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithDecInputReturnsDecember()
        {
            // Arrange
            string str = "dec";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 12);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns December with
        /// <c>"December"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithDecemberInputReturnsDecember()
        {
            // Arrange
            string str = "December";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 12);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Day and Month (Long Form)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"feb14"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb14InputReturnsFebruary14()
        {
            // Arrange
            string str = "feb14";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"feb14th"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb14ThInputReturnsFebruary14()
        {
            // Arrange
            string str = "feb14th";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"feb 14"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace14InputReturnsFebruary14()
        {
            // Arrange
            string str = "feb 14";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"feb 14th"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace14ThInputReturnsFebruary14()
        {
            // Arrange
            string str = "feb 14th";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"February14"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14InputReturnsFebruary14()
        {
            // Arrange
            string str = "February14";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"February14th"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14ThInputReturnsFebruary14()
        {
            // Arrange
            string str = "February14th";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"February 14"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace14InputReturnsFebruary14()
        {
            // Arrange
            string str = "February 14";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"February 14th"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace14ThInputReturnsFebruary14()
        {
            // Arrange
            string str = "February 14th";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14feb"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14FebInputReturnsFebruary14()
        {
            // Arrange
            string str = "14feb";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14thfeb"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThFebInputReturnsFebruary14()
        {
            // Arrange
            string str = "14thfeb";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14 feb"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14SpaceFebInputReturnsFebruary14()
        {
            // Arrange
            string str = "14 feb";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14th feb"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceFebInputReturnsFebruary14()
        {
            // Arrange
            string str = "14th feb";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14February"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14FebruaryInputReturnsFebruary14()
        {
            // Arrange
            string str = "14February";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14thFebruary"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThFebruaryInputReturnsFebruary14()
        {
            // Arrange
            string str = "14thFebruary";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14thofFebruary"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThOfFebruaryInputReturnsFebruary14()
        {
            // Arrange
            string str = "14thofFebruary";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14 February"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14SpaceFebruaryInputReturnsFebruary14()
        {
            // Arrange
            string str = "14 February";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14th February"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceFebruaryInputReturnsFebruary14()
        {
            // Arrange
            string str = "14th February";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14 with
        /// <c>"14th of February"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceOfSpaceFebruaryInputReturnsFebruary14()
        {
            // Arrange
            string str = "14th of February";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Month and Year (Long Form)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 2015 with
        /// <c>"feb2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb2015InputReturnsFebruary2015()
        {
            // Arrange
            string str = "feb2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 2015 with
        /// <c>"feb 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace2015InputReturnsFebruary2015()
        {
            // Arrange
            string str = "feb 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 2015 with
        /// <c>"February2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary2015InputReturnsFebruary2015()
        {
            // Arrange
            string str = "February2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 2015 with
        /// <c>"February 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace2015InputReturnsFebruary2015()
        {
            // Arrange
            string str = "February 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Day, Month, and Year (Long Form)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb14 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb14Space2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb14 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb14th 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb14ThSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb14th 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb 14 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace14Space2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb 14 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb 14th 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace14ThSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb 14th 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb14CommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb14th, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFeb14ThCommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb14th, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb 14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace14CommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb 14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"feb 14th, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebSpace14ThCommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "feb 14th, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February14 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14Space2015InputReturns14February2015()
        {
            // Arrange
            string str = "February14 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February14th 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14ThSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "February14th 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February 14 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace14Space2015InputReturns14February2015()
        {
            // Arrange
            string str = "February 14 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February 14th 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace14ThSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "February 14th 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14CommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "February14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February14th, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14ThCommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "February14th, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February 14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace14CommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "February 14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"February 14th, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruarySpace14ThCommaSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "February 14th, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14feb2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14Feb2015InputReturns14February2015()
        {
            // Arrange
            string str = "14feb2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14thfeb2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThFeb2015InputReturns14February2015()
        {
            // Arrange
            string str = "14thfeb2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14thoffeb2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThOfFeb2015InputReturns14February2015()
        {
            // Arrange
            string str = "14thoffeb2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14 feb 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14SpaceFebSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "14 feb 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14th feb 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceFebSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "14th feb 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14th of feb 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceOfSpaceFebSpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "14th of feb 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14February2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14February2015InputReturns14February2015()
        {
            // Arrange
            string str = "14February2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14thFebruary2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThFebruary2015InputReturns14February2015()
        {
            // Arrange
            string str = "14thFebruary2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14thofFebruary2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThOfFebruary2015InputReturns14February2015()
        {
            // Arrange
            string str = "14thofFebruary2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14 February 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14SpaceFebruarySpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "14 February 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14th February 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceFebruarySpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "14th February 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 with
        /// <c>"14th of February 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith14ThSpaceOfSpaceFebruarySpace2015InputReturns14February2015()
        {
            // Arrange
            string str = "14th of February 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Day and Month (Short Form)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10 with
        /// <c>"11/10"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Slash10InputAndEnUsCultureReturnsNovember10()
        {
            // Arrange
            string str = "11/10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10 with
        /// <c>"11-10"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dash10InputAndEnUsCultureReturnsNovember10()
        {
            // Arrange
            string str = "11-10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11 with
        /// <c>"11/10"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Slash10InputAndEnAuCultureReturnsOctober11()
        {
            // Arrange
            string str = "11/10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11 with
        /// <c>"11-10"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dash10InputAndEnAuCultureReturnsOctober11()
        {
            // Arrange
            string str = "11-10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10 with
        /// <c>"11/10"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Slash10InputAndJaJpCultureReturnsNovember10()
        {
            // Arrange
            string str = "11/10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10 with
        /// <c>"11-10"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dash10InputAndJaJpCultureReturnsNovember10()
        {
            // Arrange
            string str = "11-10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Month and Year (Short Form)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11/2012"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Slash2012InputAndEnUsCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11/2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11-2012"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dash2012InputAndEnUsCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11-2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11.2012"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dot2012InputAndEnUsCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11.2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11/2012"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Slash2012InputAndEnAuCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11/2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11-2012"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dash2012InputAndEnAuCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11-2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11.2012"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dot2012InputAndEnAuCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11.2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11/2012"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Slash2012InputAndJaJpCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11/2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11-2012"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dash2012InputAndJaJpCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11-2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"11.2012"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith11Dot2012InputAndJaJpCultureReturnsNovember2012()
        {
            // Arrange
            string str = "11.2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012/11"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Slash11InputAndEnUsCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012/11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012-11"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dash11InputAndEnUsCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012-11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012.11"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dot11InputAndEnUsCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012.11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012/11"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Slash11InputAndEnAuCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012/11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012-11"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dash11InputAndEnAuCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012-11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012.11"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dot11InputAndEnAuCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012.11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012/11"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Slash11InputAndJaJpCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012/11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012-11"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dash11InputAndJaJpCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012-11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 2012 with
        /// <c>"2012.11"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dot11InputAndJaJpCultureReturnsNovember2012()
        {
            // Arrange
            string str = "2012.11";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Day, Month, and Year (Short Form)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11, 2012 with
        /// <c>"10/11/12"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Slash11Slash12InputAndEnUsCultureReturns11October2012()
        {
            // Arrange
            string str = "10/11/12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11, 2012 with
        /// <c>"10-11-12"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dash11Dash12InputAndEnUsCultureReturns11October2012()
        {
            // Arrange
            string str = "10-11-12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11, 2012 with
        /// <c>"10.11.12"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dot11Dot12InputAndEnUsCultureReturns11October2012()
        {
            // Arrange
            string str = "10.11.12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10/11/12"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Slash11Slash12InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "10/11/12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10-11-12"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dash11Dash12InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "10-11-12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10.11.12"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dot11Dot12InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "10.11.12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 12, 2010 with
        /// <c>"10/11/12"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Slash11Slash12InputAndJaJpCultureReturns12November2010()
        {
            // Arrange
            string str = "10/11/12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2010, expectedMonth: 11, expectedDay: 12);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 12, 2010 with
        /// <c>"10-11-12"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dash11Dash12InputAndJaJpCultureReturns12November2010()
        {
            // Arrange
            string str = "10-11-12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2010, expectedMonth: 11, expectedDay: 12);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 12, 2010 with
        /// <c>"10.11.12"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dot11Dot12InputAndJaJpCultureReturns12November2010()
        {
            // Arrange
            string str = "10.11.12";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2010, expectedMonth: 11, expectedDay: 12);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11, 2012 with
        /// <c>"10/11/2012"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Slash11Slash2012InputAndEnUsCultureReturns11October2012()
        {
            // Arrange
            string str = "10/11/2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11, 2012 with
        /// <c>"10-11-2012"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dash11Dash2012InputAndEnUsCultureReturns11October2012()
        {
            // Arrange
            string str = "10-11-2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns October 11, 2012 with
        /// <c>"10.11.2012"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dot11Dot2012InputAndEnUsCultureReturns11October2012()
        {
            // Arrange
            string str = "10.11.2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 10, expectedDay: 11);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10/11/2012"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Slash11Slash2012InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "10/11/2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10-11-2012"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dash11Dash2012InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "10-11-2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10.11.2012"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dot11Dot2012InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "10.11.2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10/11/2012"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Slash11Slash2012InputAndJaJpCultureReturns10November2012()
        {
            // Arrange
            string str = "10/11/2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10-11-2012"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dash11Dash2012InputAndJaJpCultureReturns10November2012()
        {
            // Arrange
            string str = "10-11-2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"10.11.2012"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith10Dot11Dot2012InputAndJaJpCultureReturns10November2012()
        {
            // Arrange
            string str = "10.11.2012";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012/11/10"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Slash11Slash10InputAndEnUsCultureReturns10November2012()
        {
            // Arrange
            string str = "2012/11/10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012-11-10"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dash11Dash10InputAndEnUsCultureReturns10November2012()
        {
            // Arrange
            string str = "2012-11-10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012.11.10"</c> input and <c>"en-US"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dot11Dot10InputAndEnUsCultureReturns10November2012()
        {
            // Arrange
            string str = "2012.11.10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012/11/10"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Slash11Slash10InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "2012/11/10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012-11-10"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dash11Dash10InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "2012-11-10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012.11.10"</c> input and <c>"en-AU"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dot11Dot10InputAndEnAuCultureReturns10November2012()
        {
            // Arrange
            string str = "2012.11.10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-AU");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012/11/10"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Slash11Slash10InputAndJaJpCultureReturns10November2012()
        {
            // Arrange
            string str = "2012/11/10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012-11-10"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dash11Dash10InputAndJaJpCultureReturns10November2012()
        {
            // Arrange
            string str = "2012-11-10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns November 10, 2012 with
        /// <c>"2012.11.10"</c> input and <c>"ja-JP"</c> culture.
        /// </summary>
        [TestMethod]
        public void ParseWith2012Dot11Dot10InputAndJaJpCultureReturns10November2012()
        {
            // Arrange
            string str = "2012.11.10";
            IFormatProvider provider = CultureInfo.GetCultureInfo("ja-JP");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2012, expectedMonth: 11, expectedDay: 10);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Relative Date

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today with
        /// <c>"today"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTodayInputReturnsToday()
        {
            // Arrange
            string str = "today";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns tomorrow with
        /// <c>"tomorrow"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTomorrowInputReturnsTomorrow()
        {
            // Arrange
            string str = "tomorrow";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Tomorrow);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Special Date (New Year)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns New Year with
        /// <c>"ny"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNyInputReturnsNewYear()
        {
            // Arrange
            string str = "ny";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.NewYear);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns New Year with
        /// <c>"New Year"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNewYearInputReturnsNewYear()
        {
            // Arrange
            string str = "New Year";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.NewYear);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Special Date (Christmas Day)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day with
        /// <c>"xmas"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithXmasInputReturnsChristmasDay()
        {
            // Arrange
            string str = "xmas";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day with
        /// <c>"x-mas"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithXDashMasInputReturnsChristmasDay()
        {
            // Arrange
            string str = "x-mas";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day with
        /// <c>"xmas day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithXmasDayInputReturnsChristmasDay()
        {
            // Arrange
            string str = "xmas day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day with
        /// <c>"x-mas day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithXDashMasDayInputReturnsChristmasDay()
        {
            // Arrange
            string str = "x-mas day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day with
        /// <c>"Christmas"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithChristmasInputReturnsChristmasDay()
        {
            // Arrange
            string str = "Christmas";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day with
        /// <c>"Christmas Day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithChristmasDayInputReturnsChristmasDay()
        {
            // Arrange
            string str = "Christmas Day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion
        
        #region Parse Special Date (New Year's Eve)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns New Year's Eve with
        /// <c>"nye"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNyeInputReturnsNewYearsEve()
        {
            // Arrange
            string str = "nye";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.NewYearsEve);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns New Year's Eve with
        /// <c>"New Years Eve"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNewYearsEveInputReturnsNewYearsEve()
        {
            // Arrange
            string str = "New Years Eve";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.NewYearsEve);
            AssertIsEmpty(GetTimeToken(actual));
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns New Year's Eve with
        /// <c>"New Year's Eve"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNewYearApostropheSEveInputReturnsNewYearsEve()
        {
            // Arrange
            string str = "New Year's Eve";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.NewYearsEve);
            AssertIsEmpty(GetTimeToken(actual));
        }

        #endregion

        #region Parse Date and Time

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// 2:30:15 p.m. with <c>"Friday 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFriday2Colon30Colon15PmInputReturnsNextFridayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "Friday 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// 2:30:15 p.m. with <c>"Friday at 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayAt2Colon30Colon15PmInputReturnsNextFridayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "Friday at 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmFridayInputReturnsNextFridayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. on Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmOnFridayInputReturnsNextFridayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. on Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// noon with <c>"Friday noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridaySpaceNoonInputReturnsNextFridayAtNoon()
        {
            // Arrange
            string str = "Friday noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// noon with <c>"Friday at noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFridayAtNoonInputReturnsNextFridayAtNoon()
        {
            // Arrange
            string str = "Friday at noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// noon with <c>"noon Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonFridayInputReturnsNextFridayAtNoon()
        {
            // Arrange
            string str = "noon Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns next Friday at
        /// noon with <c>"noon on Friday"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonOnFridayInputReturnsNextFridayAtNoon()
        {
            // Arrange
            string str = "noon on Friday";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedDayOfWeek: DayOfWeek.Friday, expectedDayOfWeekRelation: DayOfWeekRelation.Next);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// 2:30:15 p.m. with <c>"February 14, 2015 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14Comma2015Space2Colon30Colon15PmInputReturns14February2015At2Colon30Colon15Pm()
        {
            // Arrange
            string str = "February 14, 2015 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// 2:30:15 p.m. with <c>"February 14, 2015 at 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14Comma2015At2Colon30Colon15PmInputReturns14February2015At2Colon30Colon15Pm()
        {
            // Arrange
            string str = "February 14, 2015 at 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. February 14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmFebruary14Comma2015InputReturns14February2015At2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. February 14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. on February 14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmOnFebruary14Comma2015InputReturns14February2015At2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. on February 14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// noon with <c>"February 14, 2015 noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14Comma2015SpaceNoonInputReturns14February2015AtNoon()
        {
            // Arrange
            string str = "February 14, 2015 noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// noon with <c>"February 14, 2015 at noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithFebruary14Comma2015AtNoonInputReturns14February2015AtNoon()
        {
            // Arrange
            string str = "February 14, 2015 at noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// noon with <c>"noon February 14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonFebruary14Comma2015InputReturns14February2015AtNoon()
        {
            // Arrange
            string str = "noon February 14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns February 14, 2015 at
        /// noon with <c>"noon on February 14, 2015"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonOnFebruary14Comma2015InputReturns14February2015AtNoon()
        {
            // Arrange
            string str = "noon on February 14, 2015";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedYear: 2015, expectedMonth: 2, expectedDay: 14);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// 2:30:15 p.m. with <c>"today 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithToday2Colon30Colon15PmInputReturnsTodayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "today 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// 2:30:15 p.m. with <c>"today at 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTodayAt2Colon30Colon15PmInputReturnsTodayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "today at 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. today"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmTodayInputReturnsTodayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. today";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. on today"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmOnTodayInputReturnsTodayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. on today";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// noon with <c>"today noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTodaySpaceNoonInputReturnsTodayAtNoon()
        {
            // Arrange
            string str = "today noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// noon with <c>"today at noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithTodayAtNoonInputReturnsTodayAtNoon()
        {
            // Arrange
            string str = "today at noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// noon with <c>"noon today"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonTodayInputReturnsTodayAtNoon()
        {
            // Arrange
            string str = "noon today";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns today at
        /// noon with <c>"noon on today"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonOnTodayInputReturnsTodayAtNoon()
        {
            // Arrange
            string str = "noon on today";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedRelativeDate: RelativeDate.Today);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// 2:30:15 p.m. with <c>"Christmas Day 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithChristmasDay2Colon30Colon15PmInputReturnsChristmasDayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "Christmas Day 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// 2:30:15 p.m. with <c>"Christmas Day at 2:30:15 p.m."</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithChristmasDayAt2Colon30Colon15PmInputReturnsChristmasDayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "Christmas Day at 2:30:15 p.m.";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. Christmas Day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmChristmasDayInputReturnsChristmasDayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. Christmas Day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// 2:30:15 p.m. with <c>"2:30:15 p.m. on Christmas Day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWith2Colon30Colon15PmOnChristmasDayInputReturnsChristmasDayAt2Colon30Colon15Pm()
        {
            // Arrange
            string str = "2:30:15 p.m. on Christmas Day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedHourPeriod: HourPeriod.Pm, expectedHour: 2, expectedMinute: 30, expectedSecond: 15);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// noon with <c>"Christmas Day noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithChristmasDaySpaceNoonInputReturnsChristmasDayAtNoon()
        {
            // Arrange
            string str = "Christmas Day noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// noon with <c>"Christmas Day at noon"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithChristmasDayAtNoonInputReturnsChristmasDayAtNoon()
        {
            // Arrange
            string str = "Christmas Day at noon";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// noon with <c>"noon Christmas Day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonChristmasDayInputReturnsChristmasDayAtNoon()
        {
            // Arrange
            string str = "noon Christmas Day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.Parser.Parse(string,IFormatProvider)"/> returns Christmas Day at
        /// noon with <c>"noon on Christmas Day"</c> input.
        /// </summary>
        [TestMethod]
        public void ParseWithNoonOnChristmasDayInputReturnsChristmasDayAtNoon()
        {
            // Arrange
            string str = "noon on Christmas Day";
            IFormatProvider provider = CultureInfo.GetCultureInfo("en-US");

            // Act
            TimerStartToken actual = DateTimeToken.Parser.Instance.Parse(str, provider);

            // Assert
            AssertAreEqual(GetDateToken(actual), expectedSpecialDate: SpecialDate.ChristmasDay);
            AssertAreEqual(GetTimeToken(actual), expectedSpecialTime: SpecialTime.Midday);
        }

        #endregion

        #region Get End Time (Day of Week)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 5, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for next Monday and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithNextMondayAnd20150101At000000Returns20150105At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new DayOfWeekDateToken { DayOfWeek = DayOfWeek.Monday, DayOfWeekRelation = DayOfWeekRelation.Next },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 5, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 2, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for next Friday and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithNextFridayAnd20150101At000000Returns20150102At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new DayOfWeekDateToken { DayOfWeek = DayOfWeek.Friday, DayOfWeekRelation = DayOfWeekRelation.Next },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 2, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 12, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for Monday after next and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithMondayAfterNextAnd20150101At000000Returns20150112At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new DayOfWeekDateToken { DayOfWeek = DayOfWeek.Monday, DayOfWeekRelation = DayOfWeekRelation.AfterNext },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 12, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 9, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for Friday after next and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithFridayAfterNextAnd20150101At000000Returns20150109At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new DayOfWeekDateToken { DayOfWeek = DayOfWeek.Friday, DayOfWeekRelation = DayOfWeekRelation.AfterNext },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 9, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 5, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for Monday next week and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithMondayNextWeekAnd20150101At000000Returns20150105At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new DayOfWeekDateToken { DayOfWeek = DayOfWeek.Monday, DayOfWeekRelation = DayOfWeekRelation.NextWeek },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 5, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 9, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for Friday next week and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithFridayNextWeekAnd20150101At000000Returns20150109At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new DayOfWeekDateToken { DayOfWeek = DayOfWeek.Friday, DayOfWeekRelation = DayOfWeekRelation.NextWeek },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 9, 0, 0, 0), endTime);
        }

        #endregion

        #region Get End Time (Normal Date)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 14, 2015 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for 14 and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith14And20150101At000000Returns20150114At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new NormalDateToken { Day = 14 },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 14, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns February 1, 2015 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for February and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithFebruaryAnd20150101At000000Returns20150201At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new NormalDateToken { Month = 2 },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 2, 1, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns February 14, 2015 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for February 14 and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithFebruary14And20150101At000000Returns20150214At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new NormalDateToken { Month = 2, Day = 14 },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 2, 14, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 1, 2016 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for 2016 and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith2016And20150101At000000Returns20160101At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new NormalDateToken { Year = 2016 },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns February 1, 2016 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for February, 2016 and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithFebruary2016And20150101At000000Returns20160201At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new NormalDateToken { Year = 2016, Month = 2 },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2016, 2, 1, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns February 14, 2016 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for February 14, 2016 and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithFebruary142016And20150101At000000Returns20160214At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new NormalDateToken { Year = 2016, Month = 2, Day = 14 },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2016, 2, 14, 0, 0, 0), endTime);
        }

        #endregion

        #region Get End Time (Relative Date)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 1, 2015 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for today at 12 p.m. and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithTodayAnd20150101At000000Returns20150101At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new RelativeDateToken { RelativeDate = RelativeDate.Today },
                TimeToken = new NormalTimeToken { HourPeriod = HourPeriod.Pm, Hour = 12, Minute = 0, Second = 0 }
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 12, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 2, 2015 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for tomorrow at 12 p.m. and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithTomorrowAnd20150101At000000Returns20150102At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new RelativeDateToken { RelativeDate = RelativeDate.Tomorrow },
                TimeToken = new NormalTimeToken { HourPeriod = HourPeriod.Pm, Hour = 12, Minute = 0, Second = 0 }
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 2, 12, 0, 0), endTime);
        }

        #endregion

        #region Get End Time (Special Date)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 1, 2016 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for the New Year and start time of December 31, 2015 at 10:00:00 p.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithNewYearAnd20151231At220000Returns20160101At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 12, 31, 22, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new SpecialDateToken { SpecialDate = SpecialDate.NewYear },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns December 25, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for Christmas Day and start time of December 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithChristmasDayAnd20151201At000000Returns20151225At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 12, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new SpecialDateToken { SpecialDate = SpecialDate.ChristmasDay },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 12, 25, 0, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns December 31, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for New Year's Eve and start time of December 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithNewYearsEveAnd20151201At000000Returns20151231At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 12, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new SpecialDateToken { SpecialDate = SpecialDate.NewYearsEve },
                TimeToken = new EmptyTimeToken()
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 12, 31, 0, 0, 0), endTime);
        }

        #endregion

        #region Get End Time (Normal Time)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 1, 2015 at 3:45:15 p.m. for a <see
        /// cref="DateTimeToken"/> for 3:45:15 p.m. and start time of January 1, 2015 at 12:00:00 p.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith154515And20150101At120000Returns20150101At154515()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 12, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new EmptyDateToken(),
                TimeToken = new NormalTimeToken { HourPeriod = HourPeriod.Pm, Hour = 3, Minute = 45, Second = 15 }
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 15, 45, 15), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 2, 2015 at 3:45:15 a.m. for a <see
        /// cref="DateTimeToken"/> for 3:45:15 a.m. and start time of January 1, 2015 at 12:00:00 p.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWith034515And20150101At120000Returns20150102At034515()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 12, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new EmptyDateToken(),
                TimeToken = new NormalTimeToken { HourPeriod = HourPeriod.Am, Hour = 3, Minute = 45, Second = 15 }
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 2, 3, 45, 15), endTime);
        }

        #endregion

        #region Get End Time (Special Time)

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 1, 2015 at 12:00:00 p.m. for a <see
        /// cref="DateTimeToken"/> for midday and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithMiddayAnd20150101At000000Returns20150101At120000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new EmptyDateToken(),
                TimeToken = new SpecialTimeToken { SpecialTime = SpecialTime.Midday }
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 1, 12, 0, 0), endTime);
        }

        /// <summary>
        /// Tests that <see cref="DateTimeToken.GetEndTime"/> returns January 2, 2015 at 12:00:00 a.m. for a <see
        /// cref="DateTimeToken"/> for midnight and start time of January 1, 2015 at 12:00:00 a.m.
        /// </summary>
        [TestMethod]
        public void GetEndTimeWithMidnightAnd20150101At000000Returns20150102At000000()
        {
            // Arrange
            DateTime startTime = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTimeToken dateTimeToken = new DateTimeToken
            {
                DateToken = new EmptyDateToken(),
                TimeToken = new SpecialTimeToken { SpecialTime = SpecialTime.Midnight }
            };

            // Act
            DateTime endTime = dateTimeToken.GetEndTime(startTime);

            // Assert
            Assert.AreEqual(new DateTime(2015, 1, 2, 0, 0, 0), endTime);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Asserts that a <see cref="DateToken"/> is an instance of the <see cref="DayOfWeekDateToken"/> class and
        /// that its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="DateToken"/>.</param>
        /// <param name="expectedDayOfWeek">The expected day of week.</param>
        /// <param name="expectedDayOfWeekRelation">The expected relation between the day of week and date.</param>
        private static void AssertAreEqual(DateToken actual, DayOfWeek? expectedDayOfWeek, DayOfWeekRelation? expectedDayOfWeekRelation)
        {
            Assert.AreEqual(typeof(DayOfWeekDateToken), actual.GetType());
            Assert.AreEqual(expectedDayOfWeek, ((DayOfWeekDateToken)actual).DayOfWeek);
            Assert.AreEqual(expectedDayOfWeekRelation, ((DayOfWeekDateToken)actual).DayOfWeekRelation);
        }

        /// <summary>
        /// Asserts that a <see cref="DateToken"/> is an instance of the <see cref="NormalDateToken"/> class and that
        /// its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="DateToken"/>.</param>
        /// <param name="expectedYear">The expected year.</param>
        /// <param name="expectedMonth">The expected month.</param>
        /// <param name="expectedDay">The expected day.</param>
        private static void AssertAreEqual(DateToken actual, int? expectedYear = null, int? expectedMonth = null, int? expectedDay = null)
        {
            Assert.AreEqual(typeof(NormalDateToken), actual.GetType());
            Assert.AreEqual(expectedYear, ((NormalDateToken)actual).Year);
            Assert.AreEqual(expectedMonth, ((NormalDateToken)actual).Month);
            Assert.AreEqual(expectedDay, ((NormalDateToken)actual).Day);
        }

        /// <summary>
        /// Asserts that a <see cref="DateToken"/> is an instance of the <see cref="RelativeDateToken"/> class and that
        /// its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="DateToken"/>.</param>
        /// <param name="expectedRelativeDate">The expected <see cref="RelativeDate"/>.</param>
        private static void AssertAreEqual(DateToken actual, RelativeDate expectedRelativeDate)
        {
            Assert.AreEqual(typeof(RelativeDateToken), actual.GetType());
            Assert.AreEqual(expectedRelativeDate, ((RelativeDateToken)actual).RelativeDate);
        }

        /// <summary>
        /// Asserts that a <see cref="DateToken"/> is an instance of the <see cref="SpecialDateToken"/> class and that
        /// its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="DateToken"/>.</param>
        /// <param name="expectedSpecialDate">The expected <see cref="SpecialDate"/>.</param>
        private static void AssertAreEqual(DateToken actual, SpecialDate expectedSpecialDate)
        {
            Assert.AreEqual(typeof(SpecialDateToken), actual.GetType());
            Assert.AreEqual(expectedSpecialDate, ((SpecialDateToken)actual).SpecialDate);
        }

        /// <summary>
        /// Asserts that a <see cref="TimeToken"/> is an instance of the <see cref="NormalTimeToken"/> class and that
        /// its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="TimeToken"/>.</param>
        /// <param name="expectedHourPeriod">The expected <see cref="HourPeriod"/>.</param>
        /// <param name="expectedHour">The expected hour.</param>
        /// <param name="expectedMinute">The expected minute.</param>
        /// <param name="expectedSecond">The expected second.</param>
        private static void AssertAreEqual(TimeToken actual, HourPeriod expectedHourPeriod, int expectedHour, int expectedMinute = 0, int expectedSecond = 0)
        {
            Assert.AreEqual(typeof(NormalTimeToken), actual.GetType());
            Assert.AreEqual(expectedHourPeriod, ((NormalTimeToken)actual).HourPeriod);
            Assert.AreEqual(expectedHour, ((NormalTimeToken)actual).Hour);
            Assert.AreEqual(expectedMinute, ((NormalTimeToken)actual).Minute);
            Assert.AreEqual(expectedSecond, ((NormalTimeToken)actual).Second);
        }

        /// <summary>
        /// Asserts that a <see cref="TimeToken"/> is an instance of the <see cref="SpecialTimeToken"/> class and that
        /// its value is equal to the specified parameters.
        /// </summary>
        /// <param name="actual">The actual <see cref="TimeToken"/>.</param>
        /// <param name="expectedSpecialTime">The expected <see cref="SpecialTime"/>.</param>
        private static void AssertAreEqual(TimeToken actual, SpecialTime expectedSpecialTime)
        {
            Assert.AreEqual(typeof(SpecialTimeToken), actual.GetType());
            Assert.AreEqual(expectedSpecialTime, ((SpecialTimeToken)actual).SpecialTime);
        }

        /// <summary>
        /// Asserts that a <see cref="DateToken"/> is an instance of the <see cref="EmptyDateToken"/> class.
        /// </summary>
        /// <param name="actual">The actual <see cref="DateToken"/>.</param>
        private static void AssertIsEmpty(DateToken actual)
        {
            Assert.AreEqual(typeof(EmptyDateToken), actual.GetType());
        }

        /// <summary>
        /// Asserts that a <see cref="TimeToken"/> is an instance of the <see cref="EmptyTimeToken"/> class.
        /// </summary>
        /// <param name="actual">The actual <see cref="TimeToken"/>.</param>
        private static void AssertIsEmpty(TimeToken actual)
        {
            Assert.AreEqual(typeof(EmptyTimeToken), actual.GetType());
        }

        /// <summary>
        /// Asserts that a <see cref="TimerStartToken"/> is an instance of the <see cref="DateTimeToken"/> class and
        /// returns the value of its <see cref="DateTimeToken.DateToken"/> property.
        /// </summary>
        /// <param name="actual">The actual <see cref="TimerStartToken"/>.</param>
        /// <returns>The value of the <see cref="DateTimeToken.DateToken"/> property.</returns>
        private static DateToken GetDateToken(TimerStartToken actual)
        {
            Assert.AreEqual(typeof(DateTimeToken), actual.GetType());
            return ((DateTimeToken)actual).DateToken;
        }

        /// <summary>
        /// Asserts that a <see cref="TimerStartToken"/> is an instance of the <see cref="DateTimeToken"/> class and
        /// returns the value of its <see cref="DateTimeToken.TimeToken"/> property.
        /// </summary>
        /// <param name="actual">The actual <see cref="TimerStartToken"/>.</param>
        /// <returns>The value of the <see cref="DateTimeToken.TimeToken"/> property.</returns>
        private static TimeToken GetTimeToken(TimerStartToken actual)
        {
            Assert.AreEqual(typeof(DateTimeToken), actual.GetType());
            return ((DateTimeToken)actual).TimeToken;
        }

        #endregion
    }
}
