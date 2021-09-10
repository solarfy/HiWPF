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

namespace GraphicalShapes.Pages
{
    /// <summary>
    /// Spiral.xaml 的交互逻辑
    /// </summary>
    public partial class Spiral : Page
    {
        const int revs = 20;
        const int numpts = 1000 * revs;
        Polyline poly;

        public Spiral()
        {
            InitializeComponent();

            this.Title = "Spiral";

            Canvas canv = new Canvas();
            canv.SizeChanged += CanvasOnSizeChanged;    //Canvas尺寸改变时，会重新调整螺旋的中心点
            this.Content = canv;

            poly = new Polyline();
            poly.Stroke = SystemColors.WindowTextBrush;
            canv.Children.Add(poly);

            Point[] pts = new Point[numpts];

            for (int i = 0; i < numpts; i++)
            {
                double angle = i * 2 * Math.PI / (numpts / revs);
                double scale = 250 * (1 - (double)i / numpts);

                pts[i].X = scale * Math.Cos(angle);
                pts[i].Y = scale * Math.Sin(angle);
            }
            poly.Points = new PointCollection(pts); //将Point数组作为PointCollection构造函数的参数
        }
        
        private void CanvasOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(poly, e.NewSize.Width / 2);
            Canvas.SetTop(poly, e.NewSize.Height / 2);
        }
    }
}
