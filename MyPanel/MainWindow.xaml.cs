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


namespace MyPanel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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

            sp.HorizontalAlignment = HorizontalAlignment.Center;
            //sp.AddHandler(Button.ClickEvent, new RoutedEventHandler(Btn_Click));

            ScrollViewer scroll = new ScrollViewer();
            scroll.Content = sp;
            scroll.AddHandler(Button.ClickEvent, new RoutedEventHandler(Btn_Click));

            this.MaxHeight = 500;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.CanMinimize;
            this.Content = scroll;

            //sp.Children[5]
            //sp.Children.IndexOf(btnC)
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
    }
}
