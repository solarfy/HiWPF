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

namespace SelectColor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Select Color";
            this.SizeToContent = SizeToContent.WidthAndHeight;

            InitGrid_1();
            InitGrid_2();            
        }

        void InitGrid_1()
        {            
            //建立StackPanel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            this.grid1.Children.Add(stack);

            //建立不做事的按钮,用意是测试输入焦点的转移
            Button btn = new Button();
            btn.Content = "Do-nothing button \nto test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);

            //建立ColorGrid控件
            ColorGrid clrgrid = new ColorGrid();
            clrgrid.Margin = new Thickness(24);
            clrgrid.HorizontalAlignment = HorizontalAlignment.Center;
            clrgrid.VerticalAlignment = VerticalAlignment.Center;
            clrgrid.SelectedColorChanged += ColorGridOnSelectedColorChanged;
            stack.Children.Add(clrgrid);

            //建立另一个不做事的按钮
            btn = new Button();
            btn.Content = "Do-nothing button \nto test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);
        }

        private void ColorGridOnSelectedColorChanged(object sender, EventArgs e)
        {
            ColorGrid clrgrid = sender as ColorGrid;
            this.grid1.Background = new SolidColorBrush(clrgrid.SelectedColor);
        }

        void InitGrid_2()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            this.grid2.Children.Add(stack);

            Button btn = new Button();
            btn.Content = "Do-nothing button \nto test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);

            ColorGridBox clrgrid = new ColorGridBox();
            clrgrid.Margin = new Thickness(24);
            clrgrid.HorizontalAlignment = HorizontalAlignment.Center;
            clrgrid.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(clrgrid);

            clrgrid.SetBinding(ColorGridBox.SelectedValueProperty, nameof(Background));
            clrgrid.DataContext = this.grid2;

            btn = new Button();
            btn.Content = "Do-nothing button \nto test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);
        }        
    }
}
