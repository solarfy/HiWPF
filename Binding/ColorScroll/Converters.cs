using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorScroll
{
    class DoubleToByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (byte)(double)value;     //需先转成double，再转成byte，直接转成byte会抛出异常            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out double ret))
            {
                return ret;
            }
            else
                return 0;            
        }
    }

    class RgbToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Color clr = Color.FromRgb((byte)(double)values[0]
                                    , (byte)(double)values[1]
                                    , (byte)(double)values[2]);

            if (targetType == typeof(Color))
                return clr;

            if (targetType == typeof(Brush))
                return new SolidColorBrush(clr);

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color clr;
            object[] primaries = new object[3];

            if (value is SolidColorBrush brush)
                clr = brush.Color;
            else
                return null;

            primaries[0] = clr.R;
            primaries[1] = clr.G;
            primaries[2] = clr.B;
            return primaries;
        }
    }
}
