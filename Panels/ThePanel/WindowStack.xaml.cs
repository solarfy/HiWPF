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
using System.Windows.Shapes;

namespace ThePanel
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class WindowStack : Window
    {
        public WindowStack()
        {
            InitializeComponent();

            Random rand = new Random();
            StackPanel sp = new StackPanel();

            for (int i = 0; i < 20; i++)
            {
                Button btn = new Button();
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.Margin = new Thickness(5);
                btn.FontSize += rand.Next(10);
                btn.Name = ((char)('A' + i)).ToString();
                btn.Content = $"Button {btn.Name} says 'Click me'";
                //btn.Click += Btn_Click;

                sp.Children.Add(btn);
            }

            sp.Children.Add(ZigZag(10));
            sp.Children.Add(ZigZag(0));

            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.AddHandler(Button.ClickEvent, new RoutedEventHandler(Btn_Click));

            Viewbox view = new Viewbox();   //StackPanel中的按钮会缩减尺寸以适应窗口
            view.Child = sp;

            this.Content = view;
            
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //Button btn = sender as Button;            //注意此处事件的触发源 是StackPanel
            Button btn = e.Source as Button;

            if (btn != null)        //scroll的箭头按钮也会触发Blick事件，所以此处判断一下是否为Null
            {
                MessageBox.Show($"Button {btn.Name} has been Clicked", "Button Click");
            }
        }

        private Polyline ZigZag(int offset)
        {
            Polyline polyline = new Polyline();
            polyline.Stroke = SystemColors.ControlTextBrush;
            polyline.Points = new PointCollection();
            for (int i = 0; i <= 300; i+=10)
            {
                polyline.Points.Add(new Point(i, (i + offset) % 20));
            }

            return polyline;
        }
    }
}
