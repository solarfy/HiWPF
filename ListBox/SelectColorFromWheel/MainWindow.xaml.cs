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

namespace SelectColorFromWheel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Select Color from Wheel";
            this.SizeToContent = SizeToContent.WidthAndHeight;

            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            this.Content = stack;

            Button btn = new Button();
            btn.Content = "Do-nothing button \nto test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);

            ColorWheel clrgrid = new ColorWheel();
            clrgrid.Margin = new Thickness(24);
            clrgrid.HorizontalAlignment = HorizontalAlignment.Center;
            clrgrid.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(clrgrid);

            clrgrid.SetBinding(ColorWheel.SelectedValueProperty, nameof(Background));
            clrgrid.DataContext = this;

            btn = new Button();
            btn.Content = "Do-nothing button \nto test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);
        }
    }
}
