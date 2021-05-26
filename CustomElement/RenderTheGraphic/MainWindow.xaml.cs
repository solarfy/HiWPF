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

namespace RenderTheGraphic
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Grid grid = new Grid();
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col);

            col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col);

            this.Content = grid;


            SimpleEllipse elips = new SimpleEllipse();
            //elips.HorizontalAlignment = HorizontalAlignment.Center;
            //elips.VerticalAlignment = VerticalAlignment.Center;
            //elips.Width = 50;       //文字不会被裁剪
            //elips.Height = 26;
            elips.Stroke = new Pen(new LinearGradientBrush(Colors.CadetBlue, Colors.Chocolate, new Point(1, 0), new Point(0, 1)), 24);
            elips.Fill = Brushes.AliceBlue;
            grid.Children.Add(elips);


            EllipseWithChild elipsWithChild = new EllipseWithChild();
            elipsWithChild.Stroke = new Pen(Brushes.Magenta, 48);
            elipsWithChild.Fill = Brushes.ForestGreen;
            elipsWithChild.Text = string.Empty;

            TextBlock text = new TextBlock();
            text.FontFamily = new FontFamily("Times New Roman");
            text.FontSize = 48;
            text.Text = "Text inside ellipse";
            elipsWithChild.Child = text;

            grid.Children.Add(elipsWithChild);
            Grid.SetColumn(elipsWithChild, 1);

        }
    }
}
