// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A representation of an input for a <see cref="Timer"/>.
    /// </summary>
    public abstract class TimerInput
    {
        /// <summary>
        /// The configuration data for the timer.
        /// </summary>
        private readonly TimerOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class.
        /// </summary>
        /// <param name="options">The configuration data for the timer.</param>
        protected TimerInput(TimerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = TimerOptions.FromTimerOptions(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class from a <see cref="TimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="TimerInputInfo"/>.</param>
        protected TimerInput(TimerInputInfo inputInfo)
        {
            if (inputInfo == null)
            {
                throw new ArgumentNullException("inputInfo");
            }

            this.options = TimerOptions.FromTimerOptionsInfo(inputInfo.Options);
        }

        /// <summary>
        /// Gets the configuration data for the timer.
        /// </summary>
        public TimerOptions Options
        {
            get { return this.options; }
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input.</returns>
        public static TimerInput FromString(string str)
        {
            if (Regex.IsMatch(str, @"^\s*(un)?till?\s*|^20\d\d$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                str = Regex.Replace(str, @"^\s*(un)?till?\s*", string.Empty, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                return TimerInput.FromDateTimeOrTimeSpanString(str);
            }

            return TimerInput.FromTimeSpanOrDateTimeString(str);
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input, favoring a <see cref="DateTimeTimerInput"/> in the case of ambiguity.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input.</returns>
        public static TimerInput FromDateTimeOrTimeSpanString(string str)
        {
            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
            {
                return new DateTimeTimerInput(dateTime);
            }

            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
            {
                return new TimeSpanTimerInput(timeSpan);
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input, favoring a <see cref="TimeSpanTimerInput"/> in the case of ambiguity.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="string"/>, or <c>null</c> if the <see
        /// cref="string"/> is not a valid input.</returns>
        public static TimerInput FromTimeSpanOrDateTimeString(string str)
        {
            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
            {
                return new TimeSpanTimerInput(timeSpan);
            }

            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
            {
                return new DateTimeTimerInput(dateTime);
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> for the specified <see cref="TimerInputInfo"/>, or <c>null</c> if the
        /// <see cref="TimerInputInfo"/> is not a supported type.
        /// </summary>
        /// <param name="inputInfo">A <see cref="TimerInputInfo"/>.</param>
        /// <returns>A <see cref="TimerInput"/> for the specified <see cref="TimerInputInfo"/>, or <c>null</c> if the
        /// <see cref="TimerInputInfo"/> is not a supported type.</returns>
        public static TimerInput FromTimerInputInfo(TimerInputInfo inputInfo)
        {
            TimeSpanTimerInputInfo timeSpanTimerInputInfo = inputInfo as TimeSpanTimerInputInfo;
            if (timeSpanTimerInputInfo != null)
            {
                return new TimeSpanTimerInput(timeSpanTimerInputInfo);
            }

            DateTimeTimerInputInfo dateTimeTimerInputInfo = inputInfo as DateTimeTimerInputInfo;
            if (dateTimeTimerInputInfo != null)
            {
                return new DateTimeTimerInput(dateTimeTimerInputInfo);
            }

            return null;
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="TimerInput"/> is equivalent to this <see
        /// cref="TimerInput"/> except for the <see cref="Options"/> field, which is ignored.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        /// <returns>A value indicating whether the <see cref="TimerInput"/> is equivalent to this <see
        /// cref="TimerInput"/> except for the <see cref="Options"/> field.</returns>
        public abstract bool EqualsExceptForOptions(TimerInput input);

        /// <summary>
        /// Returns the representation of the <see cref="TimerInput"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerInput"/> used for XML serialization.</returns>
        public TimerInputInfo ToTimerInputInfo()
        {
            TimerInputInfo info = this.GetNewTimerInputInfo();
            this.SetTimerInputInfo(info);
            return info;
        }

        /// <summary>
        /// Returns a new <see cref="TimerInputInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerInputInfo"/>.</returns>
        protected abstract TimerInputInfo GetNewTimerInputInfo();

        /// <summary>
        /// Sets the properties on a <see cref="TimerInputInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerInputInfo">A <see cref="TimerInputInfo"/>.</param>
        protected virtual void SetTimerInputInfo(TimerInputInfo timerInputInfo)
        {
            timerInputInfo.Options = TimerOptionsInfo.FromTimerOptions(this.options);
        }
    }
}
