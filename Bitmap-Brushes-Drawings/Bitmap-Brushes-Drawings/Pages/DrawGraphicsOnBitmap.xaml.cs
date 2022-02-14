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

namespace Bitmap_Brushes_Drawings.Pages
{
    /// <summary>
    /// DrawGraphicsOnBitmap.xaml 的交互逻辑
    /// </summary>
    public partial class DrawGraphicsOnBitmap : Page
    {
        public DrawGraphicsOnBitmap()
        {
            InitializeComponent();

            //创建100x100像素的位图，然后在上面绘制圆角矩形。窗口的背景色为卡其色。

            this.grid.Background = Brushes.Khaki;

            RenderTargetBitmap renderbitmap = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Default);
            //前两个参数是位图的宽和高，接下来的两个参数是水平和竖直解析度，单位是DPI；最后的参数是像素的格式。
            //刚创建RenderTargetBitmap的时候，图像是完全透明的，RenderTargetBitmap类型具有Render方法，需要一个Visual参数，所以
            //可以像在打印机页面上绘图那样，在位图上绘图。你可以调用Render多次；想清除位图，恢复到初始状态，就调用Clear方法。

            DrawingVisual drawvis = new DrawingVisual();
            DrawingContext dc = drawvis.RenderOpen();
            dc.DrawRoundedRectangle(Brushes.Blue, new Pen(Brushes.Red, 10), new Rect(25, 25, 50, 50), 10, 10);
            dc.Close();

            renderbitmap.Render(drawvis);

            Image img = new Image();
            img.Source = renderbitmap;
            img.Stretch = Stretch.None; //默认情况下，Image会拉伸以填满可用区域，且保持原始位图的长宽比；为避免模糊出现，此处希望图像保持原有尺寸。

            this.grid.Children.Add(img);
        }
    }
}
