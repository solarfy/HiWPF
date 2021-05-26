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
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace ThePanel
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class WindowWrap : Window
    {
        public WindowWrap()
        {
            InitializeComponent();

            ScrollViewer scroll = new ScrollViewer();
            this.Content = scroll;

            WrapPanel panel = new WrapPanel();     //ItemHeight和ItemWidth会统一设置内容的尺寸
            //StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal};      //StackPanel不会自动排列，如不放在ScrollViewer中屏幕会裁剪
            scroll.Content = panel;

            panel.Children.Add(new FileSystemInfoButton());

        }
    }

    public class FileSystemInfoButton : Button
    {
        FileSystemInfo info;

        //一个参数，做出目录或文件的按钮
        public FileSystemInfoButton(FileSystemInfo info)
        {
            this.info = info;
            this.Content = info.Name;

            if (info is DirectoryInfo)
                this.FontWeight = FontWeights.Bold;

            this.Margin = new Thickness(10);
        }

        //两个参数，做出“父目录”的按钮
        public FileSystemInfoButton(FileSystemInfo info, string str) : this(info)
        {
            this.Content = str;
        }

        //无参，做出“My Document”按钮
        public FileSystemInfoButton() : this(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
        {

        }

        protected override void OnClick()
        {
            if (info is FileInfo)   //如果是文件，则打开该文件
            {
                Process.Start(info.FullName);       
            }
            else if (info is DirectoryInfo)
            {
                DirectoryInfo dir = info as DirectoryInfo;
                Application.Current.MainWindow.Title = dir.FullName;
                Panel pn1 = this.Parent as Panel;
                pn1.Children.Clear();       //清空显示面板，准备显示新的内容

                if (dir.Parent != null)
                {
                    pn1.Children.Add(new FileSystemInfoButton(dir.Parent, "..."));      //点击... 返回父级目录
                }

                foreach(FileSystemInfo inf in dir.GetFileSystemInfos())     
                {
                    pn1.Children.Add(new FileSystemInfoButton(inf));
                }                    
            }


            base.OnClick();
        }

    }
}
