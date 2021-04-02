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

namespace MyContent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Ellipse elips = new Ellipse();
            elips.Width = 300;
            elips.Height = 300;
            elips.HorizontalAlignment = HorizontalAlignment.Left;
            elips.VerticalAlignment = VerticalAlignment.Bottom;
            elips.Fill = Brushes.AliceBlue;
            elips.StrokeThickness = 24;
            elips.Stroke = new LinearGradientBrush(Colors.CadetBlue, Colors.Chocolate, new Point(1, 0), new Point(0, 1));

            this.Content = elips;
            //this.Content = new MyDrawRect();
        }

       
    }

    public class MyDrawRect : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Red, 24), new Rect(10, 100, 100, 100));
        }
    }
}
