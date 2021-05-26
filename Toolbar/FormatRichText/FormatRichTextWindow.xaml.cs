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

namespace FormatRichText
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FormatRichTextWindow : Window
    {
        private RichTextBox txtbox;

        public FormatRichTextWindow()
        {
            InitializeComponent();

            this.Title = "Format Rich Text";

            DockPanel dock = new DockPanel();
            this.Content = dock;

            ToolBarTray tray = new ToolBarTray();
            dock.Children.Add(tray);
            DockPanel.SetDock(tray, Dock.Top);

            txtbox = new RichTextBox();
            txtbox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            AddFileToolBar(tray, 0, 0);
            AddEditToolBar(tray, 1, 0);
            AddCharToolBar(tray, 2, 0);
            AddParaToolBar(tray, 2, 1);
            AddStatusBar(dock);

            dock.Children.Add(txtbox);
            txtbox.Focus();
        }        
    }
}
