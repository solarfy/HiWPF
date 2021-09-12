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

namespace GeometriesAndPaths.Pages
{
    /// <summary>
    /// BezierExperimenter.xaml 的交互逻辑
    /// </summary>
    public partial class BezierExperimenter : Page
    {
        Polyline Bezier;

        public BezierExperimenter()
        {
            InitializeComponent();
            Bezier = new Polyline();
            Bezier.Stroke = Brushes.OrangeRed;
            this.canvas.Children.Add(Bezier);
            this.canvas.SizeChanged += CanvasOnSizeChanged;
        }

       protected virtual void CanvasOnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            this.ptStart.Center = new Point(args.NewSize.Width / 4, args.NewSize.Height / 2);
            this.ptCtrl1.Center = new Point(args.NewSize.Width / 2, args.NewSize.Height / 4);
            this.ptCtrl2.Center = new Point(args.NewSize.Width / 2, args.NewSize.Height / 4);
            this.ptEnd.Center = new Point(args.NewSize.Width / 4, args.NewSize.Height / 2);

            DrawBezierManually();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Point pt = e.GetPosition(this.canvas);

            if (e.ChangedButton == MouseButton.Left)
                this.ptCtrl1.Center = pt;

            if (e.ChangedButton == MouseButton.Right)
                this.ptCtrl2.Center = pt;

            DrawBezierManually();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point pt = e.GetPosition(this.canvas);

            if (e.LeftButton == MouseButtonState.Pressed)
                this.ptCtrl1.Center = pt;

            if (e.RightButton == MouseButtonState.Pressed)
                this.ptCtrl2.Center = pt;

            DrawBezierManually();
        }

        void DrawBezierManually()
        {
            Point[] pts = new Point[10];

            for (int i = 0; i < pts.Length; i++)
            {
                double t = (double)i / (pts.Length - 1);

                double x = (1 - t) * (1 - t) * (1 - t) * this.ptStart.Center.X +
                    3 * t * (1 - t) * (1 - t) * this.ptCtrl1.Center.X +
                    3 * t * t * (1 - t) * this.ptCtrl2.Center.X +
                    t * t * t * this.ptEnd.Center.X;

                double y = (1 - t) * (1 - t) * (1 - t) * this.ptStart.Center.Y +
                    3 * t * (1 - t) * (1 - t) * this.ptCtrl1.Center.Y +
                    3 * t * t * (1 - t) * this.ptCtrl2.Center.Y +
                    t * t * t * this.ptEnd.Center.Y;

                pts[i] = new Point(x, y);
            }

            Bezier.Points = new PointCollection(pts);
        }
    }
}
