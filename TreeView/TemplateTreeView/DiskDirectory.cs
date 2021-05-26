using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace TemplateTreeView
{
    class DiskDirectory
    {
        DirectoryInfo dirInfo;

        public string Name
        {
            get => dirInfo.Name;
        }

        public List<DiskDirectory> SubDirectories
        {
            get
            {
                List<DiskDirectory> dirs = new List<DiskDirectory>();
                DirectoryInfo[] subdirs;

                try
                {
                    subdirs = dirInfo.GetDirectories();
                }
                catch
                {
                    return dirs;
                }

                foreach (DirectoryInfo subdir in subdirs)
                {
                    dirs.Add(new DiskDirectory(subdir));
                }

                return dirs;
            }
        }

        public DiskDirectory(DirectoryInfo dirInfo)
        {
            this.dirInfo = dirInfo;
        }
    }
}
