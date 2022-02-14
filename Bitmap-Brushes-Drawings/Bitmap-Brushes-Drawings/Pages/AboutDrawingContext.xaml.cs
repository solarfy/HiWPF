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
using System.Windows.Media.Animation;

namespace Bitmap_Brushes_Drawings.Pages
{
    /// <summary>
    /// AboutDrawingContext.xaml 的交互逻辑
    /// </summary>
    public partial class AboutDrawingContext : Page
    {
        public AboutDrawingContext()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            //亦可通过DrawingVisual对象的RenderOpen方法获取DrawingContext
            //DrawingVisual dv = new DrawingVisual();
            //dc = dv.RenderOpen();
            //RenderTargetBitmap bitmap = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Default);
            //bitmap.Render(dv);

            Pen pen = new Pen(Brushes.Blue, 2);
            Point pt1 = new Point(20, 20);
            Point pt2 = new Point(200, 200);           

            //绘制普通直线
            dc.DrawLine(pen, pt1, pt2);
            //绘制动画直线
            PointAnimation animation1 = new PointAnimation(pt1, new Point(20, 200), new Duration(TimeSpan.FromSeconds(5)));
            PointAnimation animation2 = new PointAnimation(pt2, new Point(200, 20), new Duration(TimeSpan.FromSeconds(5)));
            animation1.RepeatBehavior = animation2.RepeatBehavior = RepeatBehavior.Forever;            
            animation1.AutoReverse = animation2.AutoReverse = true;
            dc.DrawLine(pen, pt1, animation1.CreateClock(), pt2, animation2.CreateClock());

            //绘制矩形
            dc.DrawRectangle(Brushes.OrangeRed, pen, new Rect(250, 20, 100, 100));

            //绘制圆角矩形
            dc.DrawRoundedRectangle(null, pen, new Rect(250, 140, 100, 100), 20, 20);
            //用DrawRoundedRectangle绘制椭圆
            dc.DrawRoundedRectangle(null, pen, new Rect(250, 260, 100, 100), 100 / 2, 100 / 2);

            //绘制椭圆
            dc.DrawEllipse(null, pen, new Point(420, 70), 50, 30);

            //绘制文字
            //dc.DrawText(formtxt, ptOrigin);

            //绘制指定的文本 （glyphrun 精确位置的字符集合）
            //dc.DrawGlyphRun(brush, glyphrun);

            //播放视频和音频文件
            //dc.DrawVideo(media, rect);

            //显示位图
            //dc.DrawImage(imgSrc, rect);
            //dc.DrawDrawing(drawing);

            //DrawingContext还包含了一些方法，用来设定剪裁区域（clipping）、不透明以及transform。
            //PushClip需要一个Geometry类型的参数；PushOpacity需要一个double参数，范围0~1；PushTransform需要一个Transform参数。

        }
    }
}
