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
using System.Printing;  //添加引用System.Printing，ReachFramewok

namespace PrintWithMargins
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        PrintQueue prnqueue;    //打印机
        PrintTicket prntkt;     //ReachFramewol.dll中 页面方向，横向 纵向
        Thickness marginPage = new Thickness(96);   //页边距

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            this.Title = "Print with Margins";
            this.FontSize = 24;

            StackPanel stack = new StackPanel();
            this.Content = stack;

            Button btn = new Button();
            btn.Content = "Page Set_up";
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Margin = new Thickness(24);
            btn.Click += SetupOnClick;
            stack.Children.Add(btn);

            btn = new Button();
            btn.Content = "_Print...";
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Margin = new Thickness(24);
            btn.Click += PrintOnClick;
            stack.Children.Add(btn);
        }

        private void SetupOnClick(object sender, RoutedEventArgs args)
        {
            PageMarginsDialog dlg = new PageMarginsDialog();
            dlg.Owner = this;
            dlg.PagesMargins = marginPage;

            if (dlg.ShowDialog().GetValueOrDefault())   //GetValueOrDefault()方法，会将null值转为false
            {
                marginPage = dlg.PagesMargins;
            }
        }

        [Obsolete]
        private void PrintOnClick(object sender, RoutedEventArgs args)
        {
            PrintDialog dlg = new PrintDialog();

            if (prnqueue != null)
                dlg.PrintQueue = prnqueue;

            if (prntkt != null)
                dlg.PrintTicket = prntkt;

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                prnqueue = dlg.PrintQueue;
                prntkt = dlg.PrintTicket;

                DrawingVisual vis = new DrawingVisual();
                DrawingContext dc = vis.RenderOpen();
                Pen pn = new Pen(Brushes.Black, 1);

                Rect rectPage = new Rect(marginPage.Left
                    , marginPage.Top
                    , dlg.PrintableAreaWidth - (marginPage.Left + marginPage.Right)
                    , dlg.PrintableAreaHeight - (marginPage.Top + marginPage.Bottom));

                //绘制矩形，以反映用户的边界
                dc.DrawRectangle(null, pn, rectPage);

                //建立格式化文字对象，将PrintableArea 属性显示出来
                FormattedText formtxt = new FormattedText(string.Format("Hello, Printer! {0} x {1}", (dlg.PrintableAreaWidth / 96).ToString("F1"), (dlg.PrintableAreaHeight / 96).ToString("0.0"))  // F1 同 0.0
                    , CultureInfo.CurrentCulture
                    , FlowDirection.LeftToRight
                    , new Typeface(new FontFamily("Times New Roman"), FontStyles.Italic, FontWeights.Normal, FontStretches.Normal)
                    , 48
                    , Brushes.Black);

                //取得格式化文字的实际尺寸
                Size sizeText = new Size(formtxt.Width, formtxt.Height);

                //计算将文字放置在页面中心点（有考虑边界）
                Point ptText = new Point(rectPage.Left + (rectPage.Width - formtxt.Width) / 2, rectPage.Top + (rectPage.Height - formtxt.Height) / 2);

                //绘制文字和周围的矩形
                dc.DrawText(formtxt, ptText);
                dc.DrawRectangle(null, pn, new Rect(ptText, sizeText));

                //关闭DrawingContext
                dc.Close();

                //打印页面
                dlg.PrintVisual(vis, this.Title);
            }
        }
    }
}
