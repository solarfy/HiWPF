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
using System.Text;

namespace MyBrush
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        LinearGradientBrush brush;

        public MainWindow()
        {
            InitializeComponent();

            this.SizeChanged += MainWindow_SizeChanged;

            brush = new LinearGradientBrush(Colors.SkyBlue, Colors.Violet, 0);
            brush.MappingMode = BrushMappingMode.Absolute;
            this.Background = brush;    //程序只需修改brush，窗口就会自动将结果反应出来，这是Freezable类的Changed事件改变的。
            //this.Foreground = brush;  //当客户区与文字内容区完全一样时，且前景画刷与背景画刷相同，造成文字与背景完全融合，看不见文字。
            //this.Content = "123123";    

            this.BorderThickness = new Thickness(10);
            this.BorderBrush = new LinearGradientBrush(Colors.Violet, Colors.Green, new Point(0, 0), new Point(1, 1));

            DrawRainbow();
            RotateTheGradientOrigin();
            DrawFiveStar();

            this.borderCursor.Height = Math.Ceiling(this.textContent.FontSize * this.textContent.FontFamily.LineSpacing);   //字的高度
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = this.ActualWidth - 2 * SystemParameters.ResizeFrameVerticalBorderWidth;
            double height = this.ActualHeight - 2 * SystemParameters.ResizeFrameHorizontalBorderHeight - SystemParameters.CaptionHeight;

            Point ptCenter = new Point(width / 2, height / 2);
            Vector vectDiag = new Vector(width, -height);   //从右上到左下的向量（width， 0） - （0， height）
            Vector vectPerp = new Vector(vectDiag.Y, -vectDiag.X);  //垂直于对角线

            vectPerp.Normalize();
            vectPerp *= width * height / vectDiag.Length;

            brush.StartPoint = ptCenter + vectPerp;
            brush.EndPoint = ptCenter - vectPerp;
        }

        private void DrawRainbow()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 0);
            this.gridRainbow.Background = brush;

            //采用Rey G.Biv的彩虹记住系统 红、橙、黄、绿、蓝、靛、紫
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0));       // 1/6
            brush.GradientStops.Add(new GradientStop(Colors.Orange, 0.17));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.33));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.67));
            brush.GradientStops.Add(new GradientStop(Colors.Indigo, 0.84));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 1));


            RadialGradientBrush radialBrush = new RadialGradientBrush();
            radialBrush.SpreadMethod = GradientSpreadMethod.Repeat;
            radialBrush.GradientOrigin = new Point(0.75, 0.75); //渐变点
            this.gridCircleRaindbow.Background = radialBrush;
            radialBrush.GradientStops.Add(new GradientStop(Colors.Red, 0));
            radialBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.17));
            radialBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.33));
            radialBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            radialBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.67));
            radialBrush.GradientStops.Add(new GradientStop(Colors.Indigo, 0.84));
            radialBrush.GradientStops.Add(new GradientStop(Colors.Violet, 1));

        }

        private void RotateTheGradientOrigin()
        {
            double angle = 0;
            RadialGradientBrush brush = new RadialGradientBrush(Colors.White, Colors.Blue);
            brush.Center = brush.GradientOrigin = new Point(0.5, 0.5);
            brush.RadiusX = brush.RadiusY = 0.1;
            brush.SpreadMethod = GradientSpreadMethod.Repeat;
            this.gridActive.Background = brush;

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler((sender, e) =>
            {
                Point pt = new Point(0.5 + 0.05 * Math.Cos(angle), 0.5 + 0.05 * Math.Sin(angle));
                brush.GradientOrigin = pt;
                angle += Math.PI / 6;   //偏移30度
            });
            timer.Start();
        }

        /// <summary>
        /// 绘制五角星
        /// </summary>
        private void DrawFiveStar()
        {          
            this.Title = "Paint the Button";
            Button btn = new Button();
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            canvasFiveStar.Children.Add(btn);

            //建立一个Canvas当作按钮的内容
            Canvas canv = new Canvas();
            canv.Width = 144;
            canv.Height = 144;
            btn.Content = canv;

            Rectangle rect = new Rectangle();
            rect.Width = canv.Width;
            rect.Height = canv.Height;
            rect.RadiusX = 24;
            rect.RadiusY = 24;
            rect.Fill = Brushes.Blue;
            canv.Children.Add(rect);

            Polygon poly = new Polygon();
            poly.Fill = Brushes.Yellow;
            poly.Points = new PointCollection();        //Points的默认值是null，所以此处需new一个对象
            //poly.FillRule = FillRule.Nonzero;   //星星中间区域上色

            for (int i = 0; i < 5; i++)
            {
                double angle = i * 4 * Math.PI / 5;
                Point pt = new Point(48 * Math.Sin(angle), -48 * Math.Cos(angle));  //正
                //Point pt = new Point(-48 * Math.Cos(angle), 48 * Math.Sin(angle));        //倾斜的
                poly.Points.Add(pt);
            }
            canv.Children.Add(poly);

            Canvas.SetLeft(poly, canv.Width / 2);
            Canvas.SetTop(poly, canv.Height / 2);
        }    

    protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            StringBuilder builder = new StringBuilder(this.textContent.Content as string);

            if (e.Text == "\b") //前删除
            {
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            else
            {
                builder.Append(e.Text);

            }

            this.textContent.Content = builder.ToString();
            Canvas.SetLeft(this.borderCursor, this.textContent.ActualWidth);

        }


    }
}
