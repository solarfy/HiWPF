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

namespace ColorScroll
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateBlueOnClick(object sender, RoutedEventArgs e)
        {
            /* UpdateSourceTrigger=Explicit
             * 此绑定所在的element上，调用GetBindingExpression方法（FrameworkElement所定义的方法），参数是所绑定的依赖属性。            
             */

            BindingExpression bindexp = this.txtboxBlue.GetBindingExpression(TextBox.TextProperty);
            bindexp.UpdateSource(); //此调用绑定模式必须是TwoWay或OneWayToSource，否则该调用将被忽略。
        }
    }
}
