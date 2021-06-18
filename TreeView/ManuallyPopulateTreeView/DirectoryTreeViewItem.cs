using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ManuallyPopulateTreeView
{
    public class DirectoryTreeViewItem : ImagedTreeViewItem
    {
        DirectoryInfo dir;

        public DirectoryInfo DirectoryInfo
        {
            get { return dir; }
        }

        public DirectoryTreeViewItem(DirectoryInfo dir)
        {
            this.dir = dir;
            this.Text = dir.Name;

            //this.SelectedImage = new BitmapImage(new Uri("pack://application:,,/Images/open_folder.png"));
            //this.UnselectedImage = new BitmapImage(new Uri("pack://application:,,/Images/close_folder.png"));

            this.SelectedImage = new BitmapImage(new Uri("pack://application:,,,/ManuallyPopulateTreeView;component/Images/open_folder.png"));
            this.UnselectedImage = new BitmapImage(new Uri("pack://application:,,,/ManuallyPopulateTreeView;component/Images/close_folder.png"));

        }

        public void Populate()
        {
            DirectoryInfo[] dirs;

            /*  这里使用try-catch，是因为某些目录是禁止访问的 */
            try
            {
                dirs = dir.GetDirectories();
            }
            catch
            {
                return;
            }

            foreach (DirectoryInfo dirChild in dirs)
            {
                this.Items.Add(new DirectoryTreeViewItem(dirChild));
            }
        }

        //当Item展开时预先对其子项目进行填充，注意是子项目，之所以这样做是因为，子项目中如果含有其他项目，将会在图标上显示三角形符号，表示含有子目录
        protected override void OnExpanded(RoutedEventArgs e)
        {
            base.OnExpanded(e);

            foreach(object obj in this.Items)
            {
                DirectoryTreeViewItem item = obj as DirectoryTreeViewItem;
                item.Populate();
            }
        }
    }
}
