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
    /// CreateColorBitmap.xaml 的交互逻辑
    /// </summary>
    public partial class CreateColorBitmap : Page
    {
        public CreateColorBitmap()
        {
            InitializeComponent();

            //通过逐一设定位图中的位来创建位图，BitmapSource静态的Create方法
            this.img1.Source = CreateIndexedBitmap();
            this.img2.Source = CreateFullColorBitmap();
        }

        ImageSource CreateIndexedBitmap()
        {
            List<Color> colors = new List<Color>();

            for (int r = 0; r < 256; r += 17)
                for (int b = 0; b < 256; b += 17)
                    colors.Add(Color.FromRgb((byte)r, 0, (byte)b));

            BitmapPalette palette = new BitmapPalette(colors);

            byte[] array = new byte[256 * 256];
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 256; y++)
                    array[256 * y + x] = (byte)(((int)Math.Round(y / 17.0) << 4) | (int)Math.Round(x / 17.0));

            BitmapSource bitmap = BitmapSource.Create(256, 256, 96, 96, PixelFormats.Indexed8, palette, array, 256);
            //前两个参数指定位图的宽和高（单位是像素）；接下来两个参数设定水平和竖直解析度（单位DPI）；第五个参数通常是PixelFormats类型的静态属性。
            //第六个参数是BitmapPalette类型的对象，这是Color对象的集合，该集合中的项数可以比格式所指定的项数少。如果格式不需要色彩表，就将这个参数设定为null；
            //第七个参数是一个一维数组，如果是byte型数组中的元素个数就等于最后一个参数乘以高
            //最后一个参数为每行的字节总数，然而你可以设定参数为更大的整数

            return bitmap;
        }

        ImageSource CreateFullColorBitmap()
        {
            int[] array = new int[256 * 256];

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    int b = x;
                    int g = 0;
                    int r = y;

                    array[256 * y + x] = b | (g << 8) | (r << 16);
                }
            }

            BitmapSource bitmap = BitmapSource.Create(256, 256, 96, 96, PixelFormats.Bgr32, null, array, 256 * 4);

            return bitmap; 
        }        
    }
}
