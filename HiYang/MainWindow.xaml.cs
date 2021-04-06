using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace HiYang
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int index = 0;
        PropertyInfo[] props;

        public MainWindow()
        {
            InitializeComponent();

            string[] strDayNames = System.Globalization.DateTimeFormatInfo.InvariantInfo.DayNames;

            props = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);
            //SetTitleAndBackground();            
            //this.Background = SystemColors.WindowBrush;

            LinearGradientBrush linear = new LinearGradientBrush(Colors.Red, Colors.Green, new Point(0, 0), new Point(0.25, 0.25));
            //LinearGradientBrush linear = new LinearGradientBrush(Colors.Red, Colors.Green, 90);            
            //linear.MappingMode = BrushMappingMode.Absolute; //使用实际的点位
            //linear.SpreadMethod = GradientSpreadMethod.Repeat;
            linear.SpreadMethod = GradientSpreadMethod.Reflect;
            this.Background = linear;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                index += e.Key == Key.Up ? 1 : props.Length - 1;
                index %= props.Length;
                SetTitleAndBackground();
            }
        }

        private void SetTitleAndBackground()
        {
            this.Title = $"Flip Through the Brushes - {props[index].Name}";
            this.Background = (Brush)props[index].GetValue(null, null);
        }
    }
}
