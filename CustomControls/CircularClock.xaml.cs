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

namespace CustomControls
{
    /// <summary>
    /// CircularClock.xaml 的交互逻辑
    /// </summary>
    public partial class CircularClock : UserControl
    {
        public CircularClock()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            double edge = Math.Min(this.grid.Height, this.grid.Height);

            Point ptCenter = new Point(this.grid.Width / 2, this.grid.Height / 2);

            Ellipse ellps = new Ellipse();
            ellps.Stroke = Brushes.Black;
            ellps.StrokeThickness = 2.0;
            ellps.Width = edge;
            ellps.Height = edge;
            this.grid.Children.Add(ellps);

            for (int i = 0; i < 12; i++)
            {
                //Rectangle rect = new Rectangle();
                //rect.Width = 5;
                //rect.Height = 10;
                //rect.Fill = Brushes.Red;
                //Transform transform = new TranslateTransform();
                //rect.RenderTransform = transform;               
                //this.grid.Children.Add(rect);


            }

        }
    }
}
