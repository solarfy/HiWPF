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
    /// WheelAndSpokes.xaml 的交互逻辑
    /// </summary>
    public partial class WheelAndSpokes : Page
    {
        public WheelAndSpokes()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Canvas canv = new Canvas();
            this.Content = canv;

            Ellipse elips = new Ellipse();
            elips.Stroke = SystemColors.WindowTextBrush;
            elips.Width = 200;
            elips.Height = 200;
            canv.Children.Add(elips);
            Canvas.SetLeft(elips, 50);
            Canvas.SetTop(elips, 50);

            for (int i = 0; i < 72; i++)
            {
                Line line = new Line();
                line.Stroke = SystemColors.WindowTextBrush;
                line.X1 = 150;
                line.Y1 = 150;
                line.X2 = 250;
                line.Y2 = 150;

                //旋转中心点（150，150），每条线都沿着中心点旋转不同的角度
                line.RenderTransform = new RotateTransform(5 * i, 150, 150);
                canv.Children.Add(line);
            }

            elips = new Ellipse();
            elips.Stroke = SystemColors.WindowTextBrush;
            elips.Width = 200;
            elips.Height = 200;
            canv.Children.Add(elips);
            Canvas.SetLeft(elips, 300);
            Canvas.SetTop(elips, 50);

            for (int i = 0; i < 72; i++)
            {
                Line line = new Line();
                line.Stroke = SystemColors.WindowTextBrush;
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 100;
                line.Y2 = 0;

                //旋转中心点（0，0），每条线根据中心点旋转，之后通过Canvas.SetLeft和Canvas.SetTop进行位移
                line.RenderTransform = new RotateTransform(5 * i);
                canv.Children.Add(line);
                Canvas.SetLeft(line, 400);
                Canvas.SetTop(line, 150);
            }
        }
    }
}
