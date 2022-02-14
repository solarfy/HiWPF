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

namespace HybridClock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly Color clrBackground = Colors.Aqua;
        public static readonly Brush brushBackground = Brushes.Aqua;

        TranslateTransform[] xform = new TranslateTransform[60];

        public MainWindow()
        {
            InitializeComponent();

            this.storyboard.BeginTime = -DateTime.Now.TimeOfDay;

            ContextMenu menu = new ContextMenu();
            menu.Opened += ContextMenuOnOpened;
            this.ContextMenu = menu;

            this.Loaded += WindowOnLoaded;
        }

        private void WindowOnLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 60; i++)
            {
                Ellipse elips = new Ellipse();
                elips.HorizontalAlignment = HorizontalAlignment.Center;
                elips.VerticalAlignment = VerticalAlignment.Center;
                elips.Fill = Brushes.Blue;
                elips.Width = elips.Height = i % 5 == 0 ? 6 : 2;

                TransformGroup group = new TransformGroup();
                group.Children.Add(xform[i] = new TranslateTransform(this.datetime.ActualWidth, 0));
                group.Children.Add(new TranslateTransform(this.grd.Margin.Left / 2, 0));
                group.Children.Add(new TranslateTransform(-elips.Width / 2, -elips.Height / 2));
                group.Children.Add(new RotateTransform(i * 6));
                group.Children.Add(new TranslateTransform(elips.Width / 2, elips.Height / 2));

                elips.RenderTransform = group;
                this.grd.Children.Add(elips);
            }

            MakeMask();

            this.datetime.SizeChanged += DateTimeOnSizeChanged;
        }

        private void DateTimeOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                for (int i = 0; i < 60; i++)
                {
                    xform[i].X = this.datetime.ActualWidth;
                }

                MakeMask();
            }
        }

        //实现表盘一半刻度是可见的，另一半被椭圆不透明部分遮挡。
        void MakeMask()
        {
            DrawingGroup group = new DrawingGroup();
            Point ptCenter = new Point(this.datetime.ActualWidth + this.grd.Margin.Left, this.datetime.ActualWidth + this.grd.Margin.Left);

            for (int i = 0; i < 256; i++)
            {
                Point ptInner1 = new Point(ptCenter.X + this.datetime.ActualWidth * Math.Cos(i * 2 * Math.PI / 256), ptCenter.Y + this.datetime.ActualWidth * Math.Sin(i * 2 * Math.PI / 256));
                Point ptInner2 = new Point(ptCenter.X + this.datetime.ActualWidth * Math.Cos((i + 2) * 2 * Math.PI / 256), ptCenter.Y + this.datetime.ActualWidth * Math.Sin((i + 2) * 2 * Math.PI / 256));
                Point ptOuter1 = new Point(ptCenter.X + (this.datetime.ActualWidth + this.grd.Margin.Left) * Math.Cos(i * 2 * Math.PI / 256), ptCenter.Y + (this.datetime.ActualWidth + this.grd.Margin.Left) * Math.Sin(i * 2 * Math.PI / 256));
                Point ptOuter2 = new Point(ptCenter.X + (this.datetime.ActualWidth + this.grd.Margin.Left) * Math.Cos((i + 2) * 2 * Math.PI / 256), ptCenter.Y + (this.datetime.ActualWidth + this.grd.Margin.Left) * Math.Sin((i + 2) * 2 * Math.PI / 256));

                PathSegmentCollection segcoll = new PathSegmentCollection();
                segcoll.Add(new LineSegment(ptInner2, false));
                segcoll.Add(new LineSegment(ptOuter2, false));
                segcoll.Add(new LineSegment(ptOuter1, false));
                segcoll.Add(new LineSegment(ptInner1, false));

                PathFigure fig = new PathFigure(ptInner1, segcoll, true);
                PathFigureCollection figcoll = new PathFigureCollection();
                figcoll.Add(fig);

                PathGeometry path = new PathGeometry(figcoll);
                byte byOpacity = (byte)Math.Min(255, 512 - 2 * i);

                SolidColorBrush br = new SolidColorBrush(Color.FromArgb(byOpacity, clrBackground.R, clrBackground.G, clrBackground.B));

                GeometryDrawing draw = new GeometryDrawing(br, new Pen(br, 2), path);
                group.Children.Add(draw);
            }

            DrawingBrush brush = new DrawingBrush(group);
            this.mask.Fill = brush;
        }

        private void ContextMenuOnOpened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            menu.Items.Clear();

            string[] strFormats = { "d", "D", "f", "F", "g", "G", "M", "R", "s", "t", "T", "u", "U", "Y"};

            foreach (string strFormat in strFormats)
            {
                MenuItem item = new MenuItem();
                item.Header = DateTime.Now.ToString(strFormat);
                item.Tag = strFormat;
                item.IsChecked = strFormat == (this.Resources["clock"] as ClockTicker).Format;
                item.Click += MenuItemOnClick;
                menu.Items.Add(item);
            }
        }

        private void MenuItemOnClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            (this.Resources["clock"] as ClockTicker).Format = item.Tag as string;
        }
    }
}
