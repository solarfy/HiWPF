using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Controls;
using SelectColor;  //添加项目引用ListBox->SelectColor

namespace CompileXamlWindow
{
    partial class ComXmlWindow : Window
    {
        /* 编译后，打开工程目录下的obj子目录，可能是obj下面的Debug或Release子目录；
         * ComXmlWindow.baml 这个扩展名的意思是Binary XAML，表示已经被解析、切割成token并且转成二进制格式的XAML文件。此文件变成可执行程序的一部分，如同应用程序的资源一样；
         * ComXmlWindow.g.cs 这是产生自XAML的文件（g表示generated 生成） - InitializeComponent方法就在该文件中         
         */

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new ComXmlWindow());
        }

        public ComXmlWindow()
        {            
            InitializeComponent();      //ComXmlWindow.g.cs文件中

            foreach (PropertyInfo prop in typeof(Brushes).GetProperties())
            {
                this.lstbox.Items.Add(prop.Name);
            }
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            Button btn = sender as Button;
            MessageBox.Show($"The Button labled '{btn.Content}' has been clicked.");
        }

        void ListBoxOnSelection(object sender, RoutedEventArgs args)
        {
            //ListBox lstbox = sender as ListBox;
            string strItem = lstbox.SelectedItem as string;
            PropertyInfo prop = typeof(Brushes).GetProperty(strItem);
            elips.Fill = (Brush)prop.GetValue(null);
        }

        void ColorGridOnColorChanged(object sender, EventArgs args)
        {
            ColorGrid clrgrid = sender as ColorGrid;
            this.Background = new SolidColorBrush(clrgrid.SelectedColor);
        }
    }
}
