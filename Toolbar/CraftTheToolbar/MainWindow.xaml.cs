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

namespace CraftTheToolbar
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

        private void Init()
        {
            this.Title = "Craft the Toolbar";

            RoutedUICommand[] comms = 
            { 
                ApplicationCommands.New, ApplicationCommands.Open, ApplicationCommands.Save, ApplicationCommands.Print,
                ApplicationCommands.Cut, ApplicationCommands.Copy, ApplicationCommands.Paste, ApplicationCommands.Delete
            };

            string[] strImages =
            {
                "new.png", "open.png", "save.png", "print.png",
                "cut.png", "copy.png", "paste.png", "delete.png"
            };

            DockPanel dock = new DockPanel();
            //dock.LastChildFill = false;   //为ture时，最后一个控件将会填满剩余的空间
            this.Content = dock;

            ToolBar toolbar = new ToolBar();
            toolbar.Header = "工具栏";
            dock.Children.Add(toolbar);
            DockPanel.SetDock(toolbar, Dock.Top);

            RichTextBox txtBox = new RichTextBox();
            dock.Children.Add(txtBox);

            for (int i = 0; i < 8; i++)
            {
                if (i == 4)
                    toolbar.Items.Add(new Separator());

                Button btn = new Button();
                btn.Height = (btn.Width = 36) * 16.0 / 9.0;  // Width : Height = 9 : 16
                btn.Command = comms[i];
                toolbar.Items.Add(btn);

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Vertical;            
                btn.Content = stack;

                Image img = new Image();
                img.Source = new BitmapImage(new Uri($"pack://application:,,/Images/{strImages[i]}"));
                img.Stretch = Stretch.Uniform;
                stack.Children.Add(img);

                TextBlock txtblk = new TextBlock();
                txtblk.Text = comms[i].Text;
                txtblk.HorizontalAlignment = HorizontalAlignment.Center;
                stack.Children.Add(txtblk);

                ToolTip tip = new ToolTip();
                tip.Content = comms[i].Text;
                btn.ToolTip = tip;

                this.CommandBindings.Add(new CommandBinding(comms[i], ToolBarButtonOnClick));
                //快捷键Ctrl+N,Ctrl+O,Ctrl+S,Ctrl+P,Ctrl+X,Ctrl+C,Ctrl+V,Delete也会触发对应的命令
            }

            txtBox.Focus();
        }

        private void ToolBarButtonOnClick(object sender, ExecutedRoutedEventArgs args)
        {
            RoutedUICommand comm = args.Command as RoutedUICommand;
            MessageBox.Show(comm.Name + "command not yet implemented", this.Title);
        }
    }
}
