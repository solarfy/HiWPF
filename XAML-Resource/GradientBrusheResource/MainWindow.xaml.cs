using System;
using System.Collections.Generic;
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

namespace GradientBrusheResource
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //XAML通过x:Static调用
        public static readonly double RDFontSize = 24d;

        public MainWindow()
        {
            //同C#方式添加资源（在InitializeComponent之前）或者XAML中使用DynamicResource，使得资源存取被延后，当此资源真正需要显示在面板上时，才会存取资源。
            this.Resources.Add("thicknessMargin", new Thickness(24, 12, 24, 23));

            InitializeComponent();
        }
    }
}
