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

namespace HelpDocument
{
    /// <summary>
    /// HelpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            this.treevue.Focus();
        }

        public HelpWindow(string strTopic) : this()
        {
            if (strTopic != null)
                this.frame.Source = new Uri(strTopic, UriKind.Relative);
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.treevue.SelectedValue != null)
                this.frame.Source = new Uri(this.treevue.SelectedValue as string, UriKind.Relative);
        }

        private void FrameOnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri != null && e.Uri.OriginalString != null && e.Uri.OriginalString.Length > 0)
            {
                FindItemToSelect(this.treevue, e.Uri.OriginalString);
            }
        }

        /// <summary>
        /// 根据Frame所导航的页，查找所选中的TreeViewItem
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="strSource"></param>
        /// <returns></returns>
        bool FindItemToSelect(ItemsControl ctrl, string strSource)
        {
            foreach (object obj in ctrl.Items)
            {
                System.Xml.XmlElement xml = obj as System.Xml.XmlElement;       //Xml元素
                string strAttribute = xml.GetAttribute("Source");
                TreeViewItem item = (TreeViewItem)ctrl.ItemContainerGenerator.ContainerFromItem(obj);       //获取ctrl的容器，返回指定项的UI元素

                if (strAttribute != null && strAttribute.Length > 0 && strSource.EndsWith(strAttribute))
                {
                    if (item != null && !item.IsSelected)       //防止重复选中
                        item.IsSelected = true;

                    return true;
                }

                if (item != null)
                {
                    bool isExpanded = item.IsExpanded;
                    item.IsExpanded = true;

                    if (item.HasItems && FindItemToSelect(item, strSource))
                        return true;

                    item.IsExpanded = isExpanded;
                }    
            }

            return false;
        }
    }
}
