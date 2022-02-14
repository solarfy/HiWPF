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
using System.Windows.Controls.Primitives;

namespace Bitmap_Brushes_Drawings.Pages
{
    /// <summary>
    /// DrawButtonsOnBitmap.xaml 的交互逻辑
    /// </summary>
    public partial class DrawButtonsOnBitmap : Page
    {
        public DrawButtonsOnBitmap()
        {
            InitializeComponent();

            UniformGrid unigrid = new UniformGrid();
            unigrid.Columns = 4;

            for (int i = 0; i < 32; i++)
            {
                ToggleButton btn = new ToggleButton();
                btn.Width = 96;
                btn.Height = 24;
                btn.IsChecked = (i < 4 | i > 27) ^ (i % 4 == 0 | i % 4 == 3);
                unigrid.Children.Add(btn);
            }

            //在打印机上页面显示元素和控件时，重要的一点是要调用父级的Measure和Arrange，这样尺寸才不会为0。
            unigrid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            Size szGrid = unigrid.DesiredSize;

            unigrid.Arrange(new Rect(new Point(0, 0), szGrid));

            RenderTargetBitmap renderbitmap = new RenderTargetBitmap((int)Math.Ceiling(szGrid.Width), (int)Math.Ceiling(szGrid.Height)
                ,96, 96, PixelFormats.Default);

            renderbitmap.Render(unigrid);

            Image img = new Image();
            img.Source = renderbitmap;
            this.grid.Children.Add(img);
        }
    }
}
