// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerColorInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System.Windows.Media;

    /// <summary>
    /// The representation of a <see cref="TimerColor"/> used for XML serialization.
    /// </summary>
    public class TimerColorInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="Color"/> representation of this color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the color, or <c>null</c> if no friendly name is specified.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this color is defined in the assembly.
        /// </summary>
        public bool IsBuiltIn { get; set; }

        /// <summary>
        /// Returns a <see cref="TimerColorInfo"/> for the specified <see cref="TimerColor"/>, or <c>null</c> if the
        /// specified <see cref="TimerColor"/> is <c>null</c>.
        /// </summary>
        /// <param name="color">A <see cref="TimerColor"/>.</param>
        /// <returns>A <see cref="TimerColorInfo"/> for the specified <see cref="TimerColor"/>, or <c>null</c> if the
        /// specified <see cref="TimerColor"/> is <c>null</c>.</returns>
        public static TimerColorInfo FromTimerColor(TimerColor color)
        {
            if (color == null)
            {
                return null;
            }

            return color.ToTimerColorInfo();
        }
    }
}
