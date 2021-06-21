using System;
using System.Globalization;
using System.Windows.Data;

namespace DigitalClock
{
    class FormattedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string format)
                return string.Format(format, value);

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
