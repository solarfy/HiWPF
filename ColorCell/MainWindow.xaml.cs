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
using ColorCell;

namespace SelectCell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            this.Title = "Select Color";
            this.SizeToContent = SizeToContent.WidthAndHeight;

            //建立StackPanel,作为窗口的内容
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            this.Content = stack;

            //建立不做事的按钮,用意是测试输入焦点的转移
            Button btn = new Button();
            btn.Content = "Do-nothing button to test tabbing";
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
            btn.Content = "Do-nothing button to test tabbing";
            btn.Margin = new Thickness(24);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(btn);
        }

        private void ColorGridOnSelectedColorChanged(object sender, EventArgs e)
        {
            ColorGrid clrgrid = sender as ColorGrid;
            this.Background = new SolidColorBrush(clrgrid.SelectedColor);
        }
    }
}
