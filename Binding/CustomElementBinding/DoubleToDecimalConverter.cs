using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomElementBinding
{
    class DoubleToDecimalConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value">要被转换的对象</param>
        /// <param name="targetType">被转化成的类型，即此方法返回的类型</param>
        /// <param name="parameter">Binding的ConvertParameter属性指定的对象</param>
        /// <param name="culture">转换时要注意到的地域文化（Culture）处理</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal num = new decimal((double)value);

            if (parameter != null)
            {
                if (parameter is Int32 it)
                    num = decimal.Round(num, it);
                else
                    num = decimal.Round(num, Int32.Parse(parameter as string));     //Round 舍入的小数位数
            }

            return num;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return decimal.ToDouble((decimal)value);
        }
    }
}
