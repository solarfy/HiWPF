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
using System.Globalization;

namespace MedievalButtonWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();

            MedievalButton btn = new MedievalButton();
            btn.Text = "Click this button";
            btn.FontSize = 24;
            btn.Padding = new Thickness(5, 20, 5, 20);
            btn.HorizontalContentAlignment = HorizontalAlignment.Center;
            btn.VerticalContentAlignment = VerticalAlignment.Center;
            btn.Knock += Btn_Knock;
            btn.Width = 50;

            this.Title = "Get Medival";
            this.Content = btn;
        }

        private void Btn_Knock(object sender, RoutedEventArgs e)
        {
            MedievalButton btn = e.Source as MedievalButton;
            MessageBox.Show("The button labeled\"" + btn.Text + "\" has been knocked.", this.Title);
        }
    }

    public class MedievalButton : Control
    {
        FormattedText formtxt;
        bool isMouseReallyOver;

        public static readonly DependencyProperty TextProperty;
        public static readonly RoutedEvent KnockEvent;
        public static readonly RoutedEvent PreviewKnockEvent;

        public string Text
        {
            set { SetValue(TextProperty, value == null ? " " : value); }
            get { return (string)GetValue(TextProperty); }
        }

        public event RoutedEventHandler Knock
        {
            add { AddHandler(KnockEvent, value); }
            remove { RemoveHandler(KnockEvent, value); }
        }

        public event RoutedEventHandler PreviewKnock
        {
            add { AddHandler(PreviewKnockEvent, value); }
            remove { RemoveHandler(PreviewKnockEvent, value); }
        }

        static MedievalButton()
        {
            TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(MedievalButton), new FrameworkPropertyMetadata(" ", FrameworkPropertyMetadataOptions.AffectsMeasure));

            KnockEvent = EventManager.RegisterRoutedEvent(nameof(Knock), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MedievalButton));

            PreviewKnockEvent = EventManager.RegisterRoutedEvent(nameof(PreviewKnock), RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MedievalButton));
        }

        //当按钮尺寸改变时，MeasurOverride就会被调用
        protected override Size MeasureOverride(Size constraint)
        {
            formtxt = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground);

            //计算尺寸时，要将Padding纳入考虑
            Size sizeDesired = new Size(Math.Max(48, formtxt.Width) + 4, Math.Max(48, formtxt.Height) + 4);
            sizeDesired.Width += Padding.Left + Padding.Right;
            sizeDesired.Height += Padding.Top + Padding.Bottom;

            //sizeDesired的尺寸超过constraint的尺寸将会发生裁剪，此处用Min函数使sizeDesired尺寸小于等于constraint的尺寸，可强迫采用裁剪OnRender中开始处使用dc.PushClip
            //使按钮无论设置多小都显示其轮廓 
            sizeDesired.Width = Math.Min(sizeDesired.Width, constraint.Width);
            sizeDesired.Height = Math.Min(sizeDesired.Height, constraint.Height);

            return sizeDesired;
        }

        //调用OnRender以重绘按钮
        protected override void OnRender(DrawingContext dc)
        {
            //一开始就进行裁剪  注释掉后将正常显示文字
            dc.PushClip(new RectangleGeometry(new Rect(new Point(0, 0), RenderSize)));

            Brush brushBackground = SystemColors.ControlBrush;

            if (isMouseReallyOver && IsMouseCaptured)
            {
                brushBackground = SystemColors.ControlDarkBrush;
            }

            //决定笔的宽度
            Pen pen = new Pen(Foreground, IsMouseOver ? 2 : 1);

            //绘制实心圆角矩形
            dc.DrawRoundedRectangle(brushBackground, pen, new Rect(new Point(0, 0), RenderSize), 4, 4); //圆角矩形

            //判断文字的开始点
            Point ptText = new Point(2, 2);

            switch (HorizontalContentAlignment)
            {
                case HorizontalAlignment.Left:
                    ptText.X += Padding.Left;
                    break;

                case HorizontalAlignment.Right:
                    ptText.X += RenderSize.Width - formtxt.Width - Padding.Right;
                    break;

                case HorizontalAlignment.Center:
                case HorizontalAlignment.Stretch:
                    ptText.X += (RenderSize.Width - formtxt.Width - Padding.Left - Padding.Right) / 2;
                    break;
            }
            switch (VerticalContentAlignment)
            {
                case VerticalAlignment.Top:
                    ptText.Y += Padding.Top;
                    break;

                case VerticalAlignment.Bottom:
                    ptText.Y += RenderSize.Height - formtxt.Height - Padding.Bottom;
                    break;

                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    ptText.Y += (RenderSize.Height - formtxt.Height - Padding.Top - Padding.Bottom) / 2;
                    break;                    
            }

            //绘制文字
            dc.DrawText(formtxt, ptText);

          
        }

        //此鼠标事件会影响按钮的外关
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            InvalidateVisual();
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            InvalidateVisual();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //判读鼠标是在里面还是在外面
            Point pt = e.GetPosition(this);
            bool isReallyOverNow = (pt.X >= 0 && pt.X < ActualWidth && pt.Y >= 0 && pt.Y < ActualHeight);

            if (isReallyOverNow != isMouseReallyOver)
            {
                isMouseReallyOver = isReallyOverNow;
                InvalidateVisual();
            }
        }

        //'Knock'事件触发的起始点
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            CaptureMouse();
            InvalidateVisual();
            e.Handled = true;
        }
        //此事件会触发‘Knock’事件
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (IsMouseCaptured)
            {
                if (isMouseReallyOver)
                {
                    OnPreviewKnock();
                    OnKnock();
                }

                e.Handled = true;
                Mouse.Capture(null);    //释放捕捉到的鼠标
            }
        }
        //如果试区鼠标捕捉，就重绘
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            InvalidateVisual();
        }

        //键盘的Space键或Enter键也会触发按钮
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Space || e.Key == Key.Enter)
            {
                OnPreviewKnock();
                OnKnock();
                e.Handled = true;
            }
        }


        //OnKnock方法产生‘Knock’事件
        protected virtual void OnKnock()
        {
            RoutedEventArgs argsEvent = new RoutedEventArgs();
            argsEvent.RoutedEvent = MedievalButton.PreviewKnockEvent;
            argsEvent.Source = this;
            RaiseEvent(argsEvent);
        }
        //OnPreviewKnock方法产生‘PreviewKnock’事件
        protected virtual void OnPreviewKnock()
        {
            RoutedEventArgs argsEvent = new RoutedEventArgs();
            argsEvent.RoutedEvent = MedievalButton.KnockEvent;
            argsEvent.Source = this;
            RaiseEvent(argsEvent);
        }

    }
}
