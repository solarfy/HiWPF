using System;
using System.Windows;
using System.IO;
using System.Windows.Markup;
using System.Windows.Controls;

namespace LoadEmbeddedXaml
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            //app.Run(new LoadEmbeddedXamlWindow());
            //app.Run(new LoadXamlResourceWindow());

            Uri uri = new Uri("pack://application:,,,/LoadXamlWindow.xml");
            Stream stream = Application.GetResourceStream(uri).Stream;
            Window win = XamlReader.Load(stream) as Window;

            win.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonOnClick));

            app.Run(win);
        }

        static void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            MessageBox.Show($"The button labled '{(args.Source as Button).Content}' had been clicked");
        }
    }
}
