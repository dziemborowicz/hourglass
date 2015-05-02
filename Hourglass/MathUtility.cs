// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions
    /// beyond those provided by the <see cref="Math"/> class.
    /// </summary>
    public static class MathUtility
    {
        /// <summary>
        /// Limits a value to a specified range.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="min">The minimum value of the range (inclusive).</param>
        /// <param name="max">The maximum value of the range (inclusive).</param>
        /// <returns><paramref name="value"/> limited to the specified range.</returns>
        public static double LimitToRange(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}
