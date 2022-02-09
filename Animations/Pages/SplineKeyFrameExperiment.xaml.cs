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

namespace Animations.Pages
{
    /// <summary>
    /// SplineKeyFrameExperiment.xaml 的交互逻辑
    /// </summary>
    public partial class SplineKeyFrameExperiment : Page
    {
        public static DependencyProperty ControlPoint1Property =
            DependencyProperty.Register("ControlPoint1", typeof(Point), typeof(SplineKeyFrameExperiment), new PropertyMetadata(new Point(0, 0), ControlPointOnChanged));

        public static DependencyProperty ControlPoint2Property =
            DependencyProperty.Register("ControlPoint2", typeof(Point), typeof(SplineKeyFrameExperiment), new PropertyMetadata(new Point(1, 1), ControlPointOnChanged));

        public SplineKeyFrameExperiment()
        {
            InitializeComponent();

            for (int i = 0; i <= 10; i++)
            {
                TextBlock txtblk = new TextBlock();
                txtblk.Text = (i / 10m).ToString("N1");
                this.canvMain.Children.Add(txtblk);
                Canvas.SetLeft(txtblk, 40 + 48 * i);
                Canvas.SetTop(txtblk, 14);

                Line line = new Line();
                line.X1 = 48 * (i + 1);
                line.Y1 = 30;
                line.X2 = line.X1;
                line.Y2 = 528;
                line.Stroke = Brushes.Black;
                this.canvMain.Children.Add(line);

                txtblk = new TextBlock();
                txtblk.Text = (i / 10m).ToString("N1");
                this.canvMain.Children.Add(txtblk);
                Canvas.SetLeft(txtblk, 5);
                Canvas.SetTop(txtblk, 40 + 48 * i);

                line = new Line();
                line.X1 = 30;
                line.Y1 = 48 * (i + 1);
                line.X2 = 528;
                line.Y2 = line.Y1;
                line.Stroke = Brushes.Black;
                this.canvMain.Children.Add(line);
            }

            UpdateLabel();
        }

        public Point ControlPoint1
        {
            set { SetValue(ControlPoint1Property, value); }
            get { return (Point)GetValue(ControlPoint1Property); }
        }

        public Point ControlPoint2
        {
            set { SetValue(ControlPoint2Property, value); }
            get { return (Point)GetValue(ControlPoint2Property); }
        }

        static void ControlPointOnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SplineKeyFrameExperiment page = obj as SplineKeyFrameExperiment;

            if (args.Property == ControlPoint1Property)
                page.spline.ControlPoint1 = (Point)args.NewValue;
            else if (args.Property == ControlPoint2Property)
                page.spline.ControlPoint2 = (Point)args.NewValue;
        }

        private void CanvasOnMouse(object sender, MouseEventArgs e)
        {
            Canvas canv = sender as Canvas;
            Point ptMouse = e.GetPosition(canv);

            ptMouse.X = Math.Min(1, Math.Max(0, ptMouse.X / canv.ActualWidth));
            ptMouse.Y = Math.Min(1, Math.Max(0, ptMouse.Y / canv.ActualHeight));

            if (e.LeftButton == MouseButtonState.Pressed)
                ControlPoint1 = ptMouse;

            if (e.RightButton == MouseButtonState.Pressed)
                ControlPoint2 = ptMouse;

            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Released)
                UpdateLabel();
        }

        void UpdateLabel()
        {
            this.lblInfo.Content = string.Format("Left mouse button changes ControlPoint1 = ({0:F2})\n" +
                "Right mouse button changes ControlPoint2 = ({1:F2})", ControlPoint1, ControlPoint2);
        }
    }
}
