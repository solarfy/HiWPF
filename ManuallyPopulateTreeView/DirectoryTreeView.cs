using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;

namespace ManuallyPopulateTreeView
{
    class DirectoryTreeView : TreeView
    {
        public DirectoryTreeView()
        {
            RefreshTree();
        }

        public void RefreshTree()
        {
            this.BeginInit();       //执行后，TreeView控件会暂停进行显示的更新，不这么做会导致树的创建速度变慢
            this.Items.Clear();

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                char chDrive = drive.Name.ToUpper()[0];
                DirectoryTreeViewItem item = new DirectoryTreeViewItem(drive.RootDirectory);

                //如果是只读的就显示卷标，否则就显示驱动器类型
                if (chDrive != 'A' && chDrive != 'B' && drive.IsReady && drive.VolumeLabel.Length > 0)
                {
                    item.Text = string.Format("{0} ({1})", drive.VolumeLabel, drive.Name);
                }
                else
                {
                    item.Text = string.Format("{0} ({1})", drive.DriveType, drive.Name);
                }

                if (chDrive == 'A' || chDrive == 'B')       //软盘驱动器
                {
                    item.SelectedImage = item.UnselectedImage = new BitmapImage(new Uri("pack://application:,,/Images/floppy_disk.png"));
                }
                else if (drive.DriveType == DriveType.CDRom)
                {
                    item.SelectedImage = item.UnselectedImage = new BitmapImage(new Uri("pack://application:,,/Images/cd_drive.png"));
                }
                else
                {
                    item.SelectedImage = item.UnselectedImage = new BitmapImage(new Uri("pack://application:,,/Images/drive.png"));
                }

                this.Items.Add(item);

                if (chDrive != 'A' && chDrive != 'B' && drive.IsReady)
                {
                    item.Populate();
                }                
            }

            this.EndInit();

        }
    }
}
