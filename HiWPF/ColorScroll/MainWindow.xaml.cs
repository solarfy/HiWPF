using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorScroll
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ScrollBar[] scrolls = new ScrollBar[3];
        TextBlock[] txtValue = new TextBlock[3];
        Panel pnlColor;


        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Color Scroll";
            this.Width = 500;
            this.Height = 300;

            Grid gridMain = new Grid();
            this.Content = gridMain;

            ColumnDefinition coldef = new ColumnDefinition();
            coldef.Width = new GridLength(200, GridUnitType.Pixel);
            gridMain.ColumnDefinitions.Add(coldef);

            coldef = new ColumnDefinition();
            coldef.Width = GridLength.Auto;
            gridMain.ColumnDefinitions.Add(coldef);

            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(100, GridUnitType.Star);
            gridMain.ColumnDefinitions.Add(coldef);

            //垂直Splitter
            GridSplitter split = new GridSplitter();
            split.HorizontalAlignment = HorizontalAlignment.Center;
            split.VerticalAlignment = VerticalAlignment.Stretch;
            split.Width = 6;
            gridMain.Children.Add(split);
            Grid.SetRow(split, 0);
            Grid.SetColumn(split, 1);

            //splitter右边的面板，用来显示颜色
            pnlColor = new StackPanel();
            pnlColor.Background = new SolidColorBrush(SystemColors.WindowColor);
            gridMain.Children.Add(pnlColor);
            Grid.SetRow(pnlColor, 0);
            Grid.SetColumn(pnlColor, 2);

            //Splitter左边的补充grid
            Grid grid = new Grid();
            gridMain.Children.Add(grid);
            Grid.SetRow(grid, 0);
            Grid.SetColumn(grid, 0);

            //3个row，分别为label,scroll,label
            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition();
            rowdef.Height = new GridLength(100, GridUnitType.Star);
            grid.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowdef);

            //3个Colum，分别为Red,Green,Blue
            for (int i = 0; i < 3; i++)
            {
                coldef = new ColumnDefinition();
                coldef.Width = new GridLength(33, GridUnitType.Star);
                grid.ColumnDefinitions.Add(coldef);
            }

            for (int i = 0; i < 3; i++)
            {
                Label lbl = new Label();
                lbl.Content = new string[] { "Red", "Green", "Blue" }[i];
                lbl.HorizontalAlignment = HorizontalAlignment.Center;
                grid.Children.Add(lbl);
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, i);

                scrolls[i] = new ScrollBar();
                scrolls[i].Focusable = true;
                scrolls[i].Orientation = Orientation.Vertical;  //竖直的Scrollbar
                scrolls[i].Minimum = 0;
                scrolls[i].Maximum = 255;
                scrolls[i].SmallChange = 1;
                scrolls[i].LargeChange = 16;
                scrolls[i].ValueChanged += ScrollOnValueChanged;
                grid.Children.Add(scrolls[i]);
                Grid.SetRow(scrolls[i], 1);
                Grid.SetColumn(scrolls[i], i);

                txtValue[i] = new TextBlock();
                txtValue[i].TextAlignment = TextAlignment.Center;
                txtValue[i].HorizontalAlignment = HorizontalAlignment.Center;
                txtValue[i].Margin = new Thickness(5);
                grid.Children.Add(txtValue[i]);
                Grid.SetRow(txtValue[i], 2);
                Grid.SetColumn(txtValue[i], i);
            }

            //初始化卷轴
            Color clr = (pnlColor.Background as SolidColorBrush).Color;
            scrolls[0].Value = clr.R;
            scrolls[1].Value = clr.G;
            scrolls[2].Value = clr.B;

            //设定初始焦点
            scrolls[0].Focus();
        }

        private void ScrollOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScrollBar scroll = sender as ScrollBar;
            Panel pnl = scroll.Parent as Panel;
            TextBlock txt = pnl.Children[1 + pnl.Children.IndexOf(scroll)] as TextBlock;        //一一对应，scrol控件下一个就是textBlock控件 共9个控件（3个Label，3个Scrool，3个TextBlock）

            txt.Text = string.Format("{0}\n0x{0:X2}", (int)scroll.Value);       //x2 小写的16进制输出  X2大写的16进制输出
            pnl.Background = new SolidColorBrush(Color.FromRgb((byte)scrolls[0].Value, (byte)scrolls[1].Value, (byte)scrolls[2].Value));           
        }
    }
}
