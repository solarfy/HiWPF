using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace BindLabelToScrollBar
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Binding bind = new Binding();
            bind.Source = this.scroll;      //设定的是Source而不是ElementName
            bind.Path = new PropertyPath(ScrollBar.ValueProperty);
            this.label.SetBinding(Label.ContentProperty, bind);      //SetBinding方法对象，是绑定的“目标”而非源；第一个参数必须是DependencyObject型。
        }
    }
}
