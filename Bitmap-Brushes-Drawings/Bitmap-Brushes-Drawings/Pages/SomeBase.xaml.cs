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
    /// SomeBase.xaml 的交互逻辑
    /// </summary>
    public partial class SomeBase : Page
    {
        public SomeBase()
        {
            InitializeComponent();

            Uri uri = new Uri("pack://application:,,,/hollow.jpg");

            BitmapImage bitmap1 = new BitmapImage(uri);
            Image img1 = new Image();
            img1.Source = bitmap1;
            this.uniform.Children.Add(img1);

            //使用无参的BitmapImage，设定property的前后，必须调用BeginInit和EndInit。这种做法有一个好处，在载入时，你可以旋转位图90度。
            BitmapImage bitmap2 = new BitmapImage();
            bitmap2.BeginInit();
            bitmap2.UriSource = uri;
            bitmap2.Rotation = Rotation.Rotate90;
            bitmap2.EndInit();
            Image img2 = new Image();
            img2.Source = bitmap2;
            this.uniform.Children.Add(img2);
        }
    }
}
