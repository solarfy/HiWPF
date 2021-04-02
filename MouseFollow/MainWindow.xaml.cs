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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace MouseFollow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "拖到鼠标，产生特效";

            DispatcherTimer tmr = new DispatcherTimer(DispatcherPriority.Render);
            tmr.Tick += Tmr_Tick;
            tmr.Interval = TimeSpan.FromMilliseconds(50);
            tmr.Start();
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            Point mousePt = Mouse.GetPosition(this.grid);   //当前鼠标的位置
            Vector vMouse = new Vector(mousePt.X, mousePt.Y);
            Vector vCenter = new Vector(this.grid.ActualWidth / 2, this.grid.ActualHeight / 2);     //中心点向量
            Vector vOffset = new Vector(rand.Next(-25, 25), rand.Next(-20, 21));    //随机偏移点 （将方块布置在鼠标周围）
            Vector vResult = vOffset + vMouse - vCenter;

            TranslateTransform transform = new TranslateTransform();
            transform.X = vResult.X;
            transform.Y = vResult.Y;
            
            Rectangle rect = new Rectangle();
            rect.StrokeThickness = rand.Next(1, 5);
            rect.Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
            rect.SetBinding(HeightProperty, new Binding() { Path = new PropertyPath(nameof(Width)), Source = rect});
            rect.RenderTransform = transform;
            this.grid.Children.Add(rect);

            DoubleAnimation sizeAnimation = new DoubleAnimation();
            sizeAnimation.From = rand.Next(2, 20);
            sizeAnimation.To = rand.Next(40, 80);
            sizeAnimation.Duration = TimeSpan.FromSeconds(rand.Next(1, 4));
            Storyboard.SetTargetProperty(sizeAnimation, new PropertyPath(nameof(Width)));
            Storyboard.SetTarget(sizeAnimation, rect);

            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 1;
            opacityAnimation.To = 0;
            opacityAnimation.Duration = sizeAnimation.Duration;
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(nameof(Opacity)));
            Storyboard.SetTarget(opacityAnimation, rect);
            
            Storyboard sb = new Storyboard();            
            sb.Children.Add(sizeAnimation);
            sb.Children.Add(opacityAnimation);
            sb.Completed += (ss, args) => { this.grid.Children.Remove(rect); };
            sb.Begin();
        }
    }
}
