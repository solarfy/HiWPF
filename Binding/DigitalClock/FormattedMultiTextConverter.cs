using System;
using System.Globalization;
using System.Windows.Data;

namespace DigitalClock
{
    class FormattedMultiTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format((string)parameter, values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
