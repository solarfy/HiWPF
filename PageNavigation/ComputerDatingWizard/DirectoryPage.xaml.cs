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
using System.IO;
using ManuallyPopulateTreeView;

namespace ComputerDatingWizard
{
    /// <summary>
    /// DirectoryPage.xaml 的交互逻辑
    /// </summary>
    public partial class DirectoryPage : PageFunction<DirectoryInfo>        /* PageFunction泛型类继承自Page，其目的是显示“会返回值”的页面 */
    {
        public DirectoryPage()
        {
            InitializeComponent();
            this.treevue.SelectedItemChanged += TreeViewOnSelectedItemChanged;
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.btnOk.IsEnabled = e.NewValue != null;
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnReturn(new ReturnEventArgs<DirectoryInfo>());
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirinfo = (this.treevue.SelectedItem as DirectoryTreeViewItem).DirectoryInfo;
            OnReturn(new ReturnEventArgs<DirectoryInfo>(dirinfo));
        }
    }
}
