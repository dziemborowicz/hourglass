// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInputInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System.Xml.Serialization;

    /// <summary>
    /// The representation of a <see cref="TimerInput"/> used for XML serialization.
    /// </summary>
    [XmlInclude(typeof(DateTimeTimerInputInfo))]
    [XmlInclude(typeof(TimeSpanTimerInputInfo))]
    public abstract class TimerInputInfo
    {
        /// <summary>
        /// Returns a <see cref="TimerInputInfo"/> for the specified <see cref="TimerInput"/>.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        /// <returns>A <see cref="TimerInputInfo"/> for the specified <see cref="TimerInput"/>.</returns>
        public static TimerInputInfo FromTimerInput(TimerInput input)
        {
            if (input == null)
            {
                return null;
            }

            return input.ToTimerInputInfo();
        }
    }
}
