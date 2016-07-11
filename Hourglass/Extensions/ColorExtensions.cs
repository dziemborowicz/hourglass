// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// Provides utility methods for the <see cref="Color"/> struct.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a <see cref="Color"/> to an <see cref="int"/> representation.
        /// </summary>
        /// <param name="color">A <see cref="Color"/>.</param>
        /// <returns>An <see cref="int"/> for <paramref name="color"/>.</returns>
        public static int ToInt(this Color color)
        {
            return (color.R << 0) | (color.G << 8) | (color.B << 16);
        }

        /// <summary>
        /// Converts a <see cref="string"/> representation of a <see cref="Color"/> into a <see cref="Color"/>.
        /// </summary>
        /// <param name="colorString">A <see cref="string"/> representation of a <see cref="Color"/>.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        public static Color FromString(string colorString)
        {
            object color = ColorConverter.ConvertFromString(colorString);

            if (color == null)
            {
                throw new ArgumentException("colorString");
            }

            return (Color)color;
        }
    }
}
