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

namespace DrawCircles
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas canv;

        //绘图相关字段
        bool isDrawing;
        Ellipse elips;
        Point ptCenter;

        //拖拽相关字段
        bool isDragging;
        FrameworkElement elDragging;
        Point ptMouseStart, ptElementStart;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.Title = "Draw Circles";
            this.Content = canv = new Canvas();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (isDragging)
                return;

            //创建一个新的Ellispe的对象，并加入Canvas中
            ptCenter = e.GetPosition(canv);
            elips = new Ellipse();
            elips.Stroke = SystemColors.WindowTextBrush;
            elips.StrokeThickness = 1;
            elips.Width = 0;
            elips.Height = 0;
            canv.Children.Add(elips);
            Canvas.SetLeft(elips, ptCenter.X);
            Canvas.SetTop(elips, ptCenter.Y);

            //捕捉鼠标，为未来做准备
            CaptureMouse();         //捕获鼠标，即使鼠标已离开此element，依然会持续接收到MouseMove和MouseUp事件
            isDrawing = true;
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (isDrawing)
                return;

            //得到点击的事件，为未来做准备
            ptMouseStart = e.GetPosition(canv);
            elDragging = canv.InputHitTest(ptMouseStart) as FrameworkElement;   //获取鼠标点击的元素

            if (elDragging != null)
            {
                ptElementStart = new Point(Canvas.GetLeft(elDragging), Canvas.GetTop(elDragging));
                isDragging = true;
                //CaptureMouse();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            
            if (e.ChangedButton == MouseButton.Middle)
            {
                Shape shape = canv.InputHitTest(e.GetPosition(canv)) as Shape;

                if (shape != null)
                {
                    shape.Fill = (shape.Fill == Brushes.Red ? Brushes.Transparent : Brushes.Red);       //此处若将Fill设置为null，鼠标需要点击圆的周边部分，才能得到响应
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point ptMouse = e.GetPosition(canv);

            //改变椭圆的位置和尺寸
            if (isDrawing)
            {
                double dRadius = Math.Sqrt((Math.Pow(ptCenter.X - ptMouse.X, 2)) + Math.Pow(ptCenter.Y - ptMouse.Y, 2));    //勾股定理，求三角形斜边的长，该长度即为圆的的半径

                Canvas.SetLeft(elips, ptCenter.X - dRadius);
                Canvas.SetTop(elips, ptCenter.Y - dRadius);
                elips.Width = 2 * dRadius;
                elips.Height = 2 * dRadius;
            }
            //移动椭圆
            else if (isDragging)
            {
                Canvas.SetLeft(elDragging, ptElementStart.X + ptMouse.X - ptMouseStart.X);
                Canvas.SetTop(elDragging, ptElementStart.Y + ptMouse.Y - ptMouseStart.Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            //结束绘图操作
            if (isDrawing && e.ChangedButton == MouseButton.Left)
            {
                elips.Stroke = Brushes.Blue;
                elips.StrokeThickness = Math.Min(24, elips.Width / 2);
                elips.Fill = Brushes.Red;

                isDrawing = false;
                ReleaseMouseCapture();      //释放所捕获的鼠标
            }
            //结束移动操作
            else if (isDragging && e.ChangedButton == MouseButton.Right)
            {
                isDragging = false;
                //ReleaseMouseCapture();
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            //按下ESC，结束绘图或拖拽
            if (e.Text.IndexOf('\x1b') != -1)
            {
                if (isDrawing)
                {
                    ReleaseMouseCapture();
                }
                else if (isDragging)
                {
                    Canvas.SetLeft(elDragging, ptElementStart.X);
                    Canvas.SetTop(elDragging, ptElementStart.Y);
                    //ReleaseMouseCapture();
                    isDragging = false;
                }
            }
        }

        /// <summary>
        /// 由于Windows系统可以单方面解除捕获的鼠标（如系统对话框），所以为防止意外此处需安装LostMouseCapture的事件处理器，利用这个机会做一些收尾的工作
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);

            //不正常地结束绘图，将此Ellispe从孩子中删除
            if (isDrawing)
            {
                canv.Children.Remove(elips);
                isDrawing = false;
            }
        }
    }
}
