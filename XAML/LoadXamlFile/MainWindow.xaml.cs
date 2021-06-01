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
using System.Xml;
using System.Windows.Markup;
using Microsoft.Win32;

namespace LoadXamlFile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Frame frame;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Load XAML File";

            DockPanel dock = new DockPanel();
            this.Content = dock;

            Button btn = new Button();
            btn.Content = "Open File...";
            btn.Margin = new Thickness(12);
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.Click += ButtonOnClick;
            dock.Children.Add(btn);
            DockPanel.SetDock(btn, Dock.Top);

            frame = new Frame();
            dock.Children.Add(frame);
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files (*.xml)|*.xml|All files(*.*)|*.*";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if ((bool)dlg.ShowDialog())
            {
                try
                {
                    XmlTextReader xmlReader = new XmlTextReader(dlg.FileName);

                    object obj = XamlReader.Load(xmlReader);

                    if (obj is Window)  //Window不能作为孩子
                    {
                        Window win = obj as Window;
                        win.Owner = this;
                        win.Show();
                    }
                    else
                        frame.Content = obj;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, this.Title);
                }

            }
        }
    }
}
