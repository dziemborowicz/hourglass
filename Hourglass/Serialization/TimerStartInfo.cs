// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerStartInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using Hourglass.Parsing;
    using Hourglass.Timing;

    /// <summary>
    /// The representation of a <see cref="TimerStart"/> used for XML serialization.
    /// </summary>
    public class TimerStartInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="TimerStartToken"/>.
        /// </summary>
        public TimerStartToken TimerStartToken { get; set; }

        /// <summary>
        /// Returns a <see cref="TimerStartInfo"/> for a <see cref="TimerStart"/>.
        /// </summary>
        /// <param name="timerStart">A <see cref="TimerStart"/>.</param>
        /// <returns>The <see cref="TimerStartInfo"/> for the <see cref="TimerStart"/>.</returns>
        public static TimerStartInfo FromTimerStart(TimerStart timerStart)
        {
            if (timerStart == null)
            {
                return null;
            }

            return timerStart.ToTimerStartInfo();
        }
    }
}
