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
    /// SineWave.xaml 的交互逻辑
    /// </summary>
    public partial class SineWave : Page
    {
        public SineWave()
        {
            InitializeComponent();

            this.Title = "Sine Wave";

            Polyline poly = new Polyline();
            poly.VerticalAlignment = VerticalAlignment.Center;
            poly.Stroke = SystemColors.WindowTextBrush;
            poly.StrokeThickness = 2;
            this.Content = poly;

            //poly.Points = new PointCollection(2000);  //如果点数过多最好在循环前指定点的个数
            for (int i = 0; i < 2000; i++)
                poly.Points.Add(new Point(i, 96 * (1 - Math.Sin(i * Math.PI / 192))));  //纵向坐标为0到192

            //如果纵坐标的范围被设定到-96到96（只需一次Math.Sin前的1和减号即可），该曲线被视作高度只有96，而不是192
        }
    }
}
