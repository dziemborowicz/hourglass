// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerStart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System;

    using Hourglass.Parsing;
    using Hourglass.Properties;
    using Hourglass.Serialization;

    /// <summary>
    /// Represents the <see cref="TimerStart"/> type.
    /// </summary>
    public enum TimerStartType
    {
        /// <summary>
        /// Represents a <see cref="TimerStart"/> that starts a timer counting down for a specified timer interval.
        /// </summary>
        TimeSpan,

        /// <summary>
        /// Represents a <see cref="TimerStart"/> that starts a timer counting down until a specified instant in time.
        /// </summary>
        DateTime
    }

    /// <summary>
    /// Specifies a set of values used to start a timer.
    /// </summary>
    public class TimerStart
    {
        /// <summary>
        /// A <see cref="TimerStartToken"/>.
        /// </summary>
        private readonly TimerStartToken timerStartToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerStart"/> class.
        /// </summary>
        /// <param name="timerStartToken">A <see cref="TimerStartToken"/>.</param>
        private TimerStart(TimerStartToken timerStartToken)
        {
            if (timerStartToken == null)
            {
                throw new ArgumentNullException("timerStartToken");
            }

            this.timerStartToken = timerStartToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerStart"/> class.
        /// </summary>
        /// <param name="timerStartInfo">A <see cref="TimerStartInfo"/>.</param>
        private TimerStart(TimerStartInfo timerStartInfo)
        {
            if (timerStartInfo == null)
            {
                throw new ArgumentNullException("timerStartInfo");
            }

            this.timerStartToken = timerStartInfo.TimerStartToken;
        }

        /// <summary>
        /// Gets the default <see cref="TimerStart"/> object.
        /// </summary>
        public static TimerStart Default
        {
            get { return TimerStart.FromString(Resources.TimerStartDefault); }
        }

        /// <summary>
        /// Gets the zero-length <see cref="TimerStart"/> object.
        /// </summary>
        public static TimerStart Zero
        {
            get { return TimerStart.FromString(Resources.TimerStartZero); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TimerStart"/> can be used to start a timer now.
        /// </summary>
        public bool IsCurrent
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime endTime;
                return this.timerStartToken.TryGetEndTime(now, out endTime) && endTime >= now;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TimerStart"/> is valid.
        /// </summary>
        public bool IsValid
        {
            get { return this.timerStartToken.IsValid; }
        }

        /// <summary>
        /// Gets the <see cref="TimerStart"/> type.
        /// </summary>
        public TimerStartType Type
        {
            get { return this.timerStartToken is DateTimeToken ? TimerStartType.DateTime : TimerStartType.TimeSpan; }
        }

        /// <summary>
        /// Returns a <see cref="TimerStart"/> for a string.
        /// </summary>
        /// <param name="str">A string.</param>
        /// <returns>The <see cref="TimerStart"/> for the string, or <c>null</c> if the string is not a supported
        /// representation of a <see cref="TimerStart"/>.</returns>
        public static TimerStart FromString(string str)
        {
            TimerStartToken timerStartToken = TimerStartToken.FromString(str);

            if (timerStartToken == null)
            {
                return null;
            }

            return new TimerStart(timerStartToken);
        }

        /// <summary>
        /// Returns a <see cref="TimerStart"/> for a <see cref="TimerStartInfo"/>.
        /// </summary>
        /// <param name="timerStartInfo">A <see cref="TimerStartInfo"/>.</param>
        /// <returns>The <see cref="TimerStart"/> for the <see cref="TimerStartInfo"/>.</returns>
        public static TimerStart FromTimerStartInfo(TimerStartInfo timerStartInfo)
        {
            if (timerStartInfo == null)
            {
                return null;
            }

            return new TimerStart(timerStartInfo);
        }

        /// <summary>
        /// Returns the end time for a timer started with this <see cref="TimerStart"/> at a specified time.
        /// </summary>
        /// <param name="startTime">The time the timer is started.</param>
        /// <returns>The end time for a timer started with this <see cref="TimerStart"/> at the specified time.
        /// </returns>
        public DateTime GetEndTime(DateTime startTime)
        {
            return this.timerStartToken.GetEndTime(startTime);
        }

        /// <summary>
        /// Returns the end time for a timer started with this <see cref="TimerStart"/> at a specified time.
        /// </summary>
        /// <param name="startTime">The time the timer is started.</param>
        /// <param name="endTime">The end time for a timer started with this <see cref="TimerStart"/> at the specified
        /// time if the end time could be computed, or <see cref="DateTime.MinValue"/> otherwise.</param>
        /// <returns><c>true</c> if the end time could be computed, or <c>false</c> otherwise.</returns>
        public bool TryGetEndTime(DateTime startTime, out DateTime endTime)
        {
            return this.timerStartToken.TryGetEndTime(startTime, out endTime);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.timerStartToken.ToString();
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerStart"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerStart"/> used for XML serialization.</returns>
        public TimerStartInfo ToTimerStartInfo()
        {
            return new TimerStartInfo { TimerStartToken = this.timerStartToken };
        }
    }
}
