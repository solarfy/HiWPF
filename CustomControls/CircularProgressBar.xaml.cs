using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomControls
{
    /// <summary>
    /// CircularProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class CircularProgressBar
    {
        /* Nuget安装：Unnoficial.Microsoft.Expression.Drawing
         * 引用：Microsoft.Expression.Drawing.dll
         * 
         */

        public CircularProgressBar()
        {
            InitializeComponent();           
        }
    }

    public class PercentValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double value = double.Parse(values[0].ToString());
            double minMum = double.Parse(values[1].ToString());
            double maxMum = double.Parse(values[2].ToString());

            if (value < minMum)
                return 0.ToString() + "%";
            else if (value > maxMum)
                return 100.ToString() + "%";
            else
                return ((value - minMum) / (maxMum - minMum) * 100).ToString("0.0") + "%";

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EndAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double value = double.Parse(values[0].ToString());
            double minMum = double.Parse(values[1].ToString());
            double maxMum = double.Parse(values[2].ToString());

            if (value < minMum)
                return 0;
            else if (value > maxMum)
                return 360;
            else
                return (value - minMum) / (maxMum - minMum) * 360;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
