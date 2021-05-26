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

namespace ThePanel
{
    /// <summary>
    /// WindowDock.xaml 的交互逻辑
    /// </summary>
    public partial class WindowDock : Window
    {
        public WindowDock()
        {
            InitializeComponent();

            DockPanel dock = new DockPanel();
            dock.LastChildFill = false;
            this.Content = dock;

            //DockPanel中的控件，只会占用剩余的内部空间，对已被占用的空间不做任何的改变 
            //下例中Button0的空间占用最大，因为它是最先占用的

            for (int i = 0; i < 17; i++)
            {
                Button btn = new Button() { Content = $"Button {i}", HorizontalAlignment = HorizontalAlignment.Stretch };
                DockPanel.SetDock(btn, (Dock)(i % 4));
                dock.Children.Add(btn);
            }

            //Button btn1 = new Button() { Content = "测试1" };
            //Button btn2 = new Button() { Content = "测试2" };
            //Button btn3 = new Button() { Content = "测试3" };
            //dock.Children.Add(btn1);
            //dock.Children.Add(btn2);
            //dock.Children.Add(btn3);

            //DockPanel.SetDock(btn1, Dock.Right);
            //btn2.SetValue(DockPanel.DockProperty, Dock.Top);        //与上句意思相同

            //Dock dck = DockPanel.GetDock(btn1);  //等价于 (Dock)btn1.GetValue(DockPanel.DockProperty);
            
        }
    }
}
