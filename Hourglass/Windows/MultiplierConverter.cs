// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiplierConverter.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Multiplies <see cref="double"/> values by a constant value.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class MultiplierConverter : IValueConverter
    {
        /// <summary>
        /// The constant <see cref="double"/> value to multiply by when converting.
        /// </summary>
        private readonly double multiplier;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplierConverter"/> class with the specified multiplier.
        /// </summary>
        /// <param name="multiplier">The constant <see cref="double"/> value to multiply by when converting.</param>
        public MultiplierConverter(double multiplier)
        {
            this.multiplier = multiplier;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The value multiplied by the multiplier, or <c>null</c> if the value was <c>null</c>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return (double)value * this.multiplier;
        }

        /// <summary>
        /// Converts a value back.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The value divided by the multiplier, or <c>null</c> if the value was <c>null</c>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return (double)value / this.multiplier;
        }
    }
}
