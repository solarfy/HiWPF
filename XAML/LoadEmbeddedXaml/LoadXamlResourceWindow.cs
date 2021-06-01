using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace LoadEmbeddedXaml
{
    class LoadXamlResourceWindow : Window
    {
        public LoadXamlResourceWindow()
        {
            this.Title = "Load Xaml Resourece";

            Uri uri = new Uri("pack://application:,,,/LoadXamlResource.xml");
            Stream stream = Application.GetResourceStream(uri).Stream;      //获取资源文件流
            FrameworkElement el = XamlReader.Load(stream) as FrameworkElement;
            this.Content = el;

            Button btn = el.FindName("MyButton") as Button;

            if (btn != null)
                btn.Click += ButtonOnClick;
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            MessageBox.Show($"The button labeled '{(args.Source as Button).Content}' has been clicked");
        }
    }
}
