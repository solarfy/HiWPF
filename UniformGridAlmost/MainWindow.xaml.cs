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

namespace DuplicateUniformGrid
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            this.Title = "Duplicate Uniform Grid";
            //this.SizeToContent = SizeToContent.WidthAndHeight;  //窗口和grid都会缩小，所有的格子都是最大按钮尺寸     

            //建立UniformGridAlmost，作为窗口内容
            UniformGridAlmost ungrid = new UniformGridAlmost();
            ungrid.Columns = 5;
            //ungrid.HorizontalAlignment = HorizontalAlignment.Center;      //窗口维持默认尺寸，grid会缩小，所有的格子都是最大按钮尺寸
            //ungrid.VerticalAlignment = VerticalAlignment.Center;
            //ungrid.Width = 20;        //grid会填入所有的按钮，但grid会被裁剪
            //ungrid.Height = 100;
            this.Content = ungrid;

            //用随机尺寸的按钮，填入UnifromGridAlmost
            Random rand = new Random();
            for (int index = 0; index < 48; index++)
            {
                Button btn = new Button();
                btn.Name = $"Button{index}";
                btn.Content = btn.Name;
                btn.FontSize += rand.Next(10);
                //btn.HorizontalAlignment = HorizontalAlignment.Center;     //窗口和grid不变，格子都是统一尺寸，按钮会根据内容调整尺寸
                //btn.VerticalAlignment = VerticalAlignment.Center;
                ungrid.Children.Add(btn);
            }

            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonOnClick));
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            Button btn = args.Source as Button;
            MessageBox.Show(btn.Name + " has been clicked", this.Title);
        }

    }
}
