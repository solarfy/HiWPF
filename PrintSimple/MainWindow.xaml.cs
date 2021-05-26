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

namespace PrintSimple
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Print Simple";
            this.FontSize = 24;

            StackPanel stack = new StackPanel();
            this.Content = stack;

            Button btn = new Button();
            btn.Content = "Print _Ellipse";
            btn.Margin = new Thickness(24);
            btn.Click += PrintEllipseOnClick;
            stack.Children.Add(btn);

            btn = new Button();
            btn.Content = "Print Bunch _Buttons";
            btn.Margin = new Thickness(24);
            btn.Click += PrintBunchButtonsOnClick;
            stack.Children.Add(btn);
        }

        //打印一个椭圆
        private void PrintEllipseOnClick(object sender, RoutedEventArgs e)
        {
            PrintDialog dlg = new PrintDialog();

            if ((bool)dlg.ShowDialog().GetValueOrDefault())
            {
                DrawingVisual vis = new DrawingVisual();
                DrawingContext dc = vis.RenderOpen();

                dc.DrawEllipse(Brushes.LightGray, 
                    new Pen(Brushes.Black, 3), 
                    new Point(dlg.PrintableAreaWidth / 2, dlg.PrintableAreaHeight / 2), 
                    dlg.PrintableAreaWidth / 2, 
                    dlg.PrintableAreaHeight / 2);   //PrintableArea不是指页面可打印区域，而是整个页面的实际尺寸，以设备无关单位（1/96英寸）表示
                dc.Close();

                dlg.PrintVisual(vis, "My first print job");
            }
        }

        //打印一束按钮
        private void PrintBunchButtonsOnClick(object sender, RoutedEventArgs e)
        {
            /* 当程序打印一个继承子UIElement的类实例时：
             * 必须让element进行layout，即调用对象的Measure与Arrange，否则此对象将会具有0的尺寸，不会出现在页面上。             
             */
            
            PrintDialog dlg = new PrintDialog();

            if ((bool)dlg.ShowDialog().GetValueOrDefault())
            {
                //建立Grid面板
                Grid grid = new Grid();

                //定义5个自动大小的列与行
                for (int i = 0; i < 5; i++)
                {
                    ColumnDefinition coldef = new ColumnDefinition();
                    coldef.Width = GridLength.Auto;
                    grid.ColumnDefinitions.Add(coldef);

                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = GridLength.Auto;
                    grid.RowDefinitions.Add(rowdef);
                }

                //让Grid具有渐变画刷
                grid.Background = new LinearGradientBrush(Colors.Gray, Colors.White, new Point(0, 0), new Point(1, 1));

                Random rand = new Random();
                //让Grid填入25个按钮
                for (int i = 0; i < 25; i++)
                {
                    Button btn = new Button();
                    btn.FontSize = 12 + rand.Next(8);
                    btn.Content = "Button No. " + (i + 1);
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    btn.VerticalAlignment = VerticalAlignment.Center;
                    btn.Margin = new Thickness(6);
                    grid.Children.Add(btn);
                    Grid.SetRow(btn, i % 5);
                    Grid.SetColumn(btn, i / 5);
                }

                //改变Grid的尺寸
                grid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                Size sizeGrid = grid.DesiredSize;

                //决定将Grid放置在页面中心的点
                Point ptGrid = new Point((dlg.PrintableAreaWidth - sizeGrid.Width) / 2, (dlg.PrintableAreaHeight - sizeGrid.Height) / 2);

                //进行Layout
                grid.Arrange(new Rect(ptGrid, sizeGrid));

                //开始打印
                dlg.PrintVisual(grid, this.Title); 
            }
        }

    }
}
