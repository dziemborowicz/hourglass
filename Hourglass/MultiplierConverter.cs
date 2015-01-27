using System;
using System.Globalization;
using System.Windows.Data;

namespace Hourglass
{
    [ValueConversion(typeof(double), typeof(double))]
    public class MultiplierConverter : IValueConverter
    {
        public MultiplierConverter(double multiplier)
        {
            Multiplier = multiplier;
        }

        public double Multiplier { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * Multiplier;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / Multiplier;
        }
    }
}
