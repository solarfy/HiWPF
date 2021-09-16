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

namespace GraphicsTransforms.Pages
{
    /// <summary>
    /// RotateAndReflect.xaml 的交互逻辑
    /// </summary>
    public partial class RotateAndReflect : Page
    {
        public RotateAndReflect()
        {
            InitializeComponent();

            RotatedText();
        }

        private void RotatedText()
        {
            for (int angle = 0; angle < 360; angle += 20)
            {
                TextBlock txtblk = new TextBlock();
                txtblk.FontFamily = new FontFamily("Arial");
                txtblk.FontSize = 24;
                txtblk.Text = "    Rotated Text";
                txtblk.RenderTransformOrigin = new Point(0, 0.5);   //左侧竖直中心点，文字前填入4个空格，这样字母就不会挤成一团
                txtblk.RenderTransform = new RotateTransform(angle);
                this.rotateCanv.Children.Add(txtblk);
                Canvas.SetLeft(txtblk, 200);
                Canvas.SetTop(txtblk, 200);
            }
        }
    }
}
