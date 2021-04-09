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

namespace MoveTheToolbar
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ToolBarTray trayTop, trayLeft;

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            this.Title = "Move the Toolbar";

            DockPanel dock = new DockPanel();
            this.Content = dock;

            trayTop = new ToolBarTray();
            dock.Children.Add(trayTop);
            DockPanel.SetDock(trayTop, Dock.Top);

            trayLeft = new ToolBarTray();
            trayLeft.Orientation = Orientation.Vertical;
            dock.Children.Add(trayLeft);
            DockPanel.SetDock(trayLeft, Dock.Left);

            Button resetBtn = new Button();
            resetBtn.Width = 100;
            resetBtn.Height = 70;
            resetBtn.Content = "重置工具栏位置";
            resetBtn.Click += ResetBtn_Click;
            dock.Children.Add(resetBtn);

            for (int i = 0; i < 6; i++)
            {
                ToolBar toolbar = new ToolBar();
                toolbar.Header = "Toolbar" + (i + 1);

                if (i < 3)
                {
                    trayTop.ToolBars.Add(toolbar);
                }
                else
                {
                    trayLeft.ToolBars.Add(toolbar);
                }

                for (int j = 0; j < 6; j++)
                {
                    Button btn = new Button();
                    btn.FontSize = 16;
                    btn.Content = (char)('A' + j);
                    toolbar.Items.Add(btn);
                }
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                if (trayTop.ToolBars[i].Header.ToString() == "Toolbar" + (i + 1))
                {
                    //对于水平的Toolbar来说，Band表示Toolbar所占的行，BandIndex表示从左到右该行中的位置
                    trayTop.ToolBars[i].Band = i;
                    trayTop.ToolBars[i].BandIndex = 0;                    
                }

                if (trayLeft.ToolBars[i].Header.ToString() == "Toolbar" + (i + 4))
                {
                    //对于垂直的Toolbar来说，Band表示Toolbar所占的列，BandIndex表示从上到下该列中的位置
                    trayLeft.ToolBars[i].Band = i;
                    trayLeft.ToolBars[i].BandIndex = 0;
                }
            }
        }
    }
}
