using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FirstWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {      
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow() { Width = 600, Height = 400, WindowStyle = WindowStyle.ThreeDBorderWindow, ResizeMode = ResizeMode.CanResizeWithGrip };
            mainWindow.Left = SystemParameters.WorkArea.Width - mainWindow.Width;   //SystemParameters.PrimaryScreenHeight  SystemParameters.PrimaryScreenWidth            
            mainWindow.Top = SystemParameters.WorkArea.Height - mainWindow.Height;
            mainWindow.Show();

            for (int i = 0; i < 2; i++)
            {
                Window window = new Window() { Width = 300, Height = 200 };
                window.Title = $"Window NO.{i + 1}";
                window.Owner = Application.Current.MainWindow;
                window.ShowInTaskbar = false;
                window.Show();
            }

            //int windowCount = Application.Current.Windows.Count;

        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

            string tip = (e.ReasonSessionEnding == ReasonSessionEnding.Shutdown) ? "关闭" : "注销";

            MessageBoxResult result = MessageBox.Show($"是否{tip}系统？", Application.Current.MainWindow.Title, MessageBoxButton.OKCancel);  //Application.Current.Windows[0]

            e.Cancel = (result == MessageBoxResult.Cancel);     //e.Cancel为true取消关闭
        }
        
        
    }
}
