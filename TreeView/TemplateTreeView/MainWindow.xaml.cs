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
using System.IO;

namespace TemplateTreeView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Show Class Hierarchy";

            InitGrid_1();
            InitGrid_2();
        }

        void InitGrid_1()
        {
            TreeView treevue = new TreeView();
            this.grid1.Children.Add(treevue);

            HierarchicalDataTemplate template = new HierarchicalDataTemplate(typeof(DiskDirectory));

            template.ItemsSource = new Binding(nameof(DiskDirectory.SubDirectories));

            FrameworkElementFactory factoryTextBlock = new FrameworkElementFactory(typeof(TextBlock));

            factoryTextBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(DiskDirectory.Name)));

            template.VisualTree = factoryTextBlock;

            DiskDirectory dir = new DiskDirectory(new DirectoryInfo(Path.GetPathRoot(Environment.SystemDirectory)));

            TreeViewItem item = new TreeViewItem();
            item.Header = dir.Name;
            item.ItemsSource = dir.SubDirectories;
            item.ItemTemplate = template;

            treevue.Items.Add(item);
            item.IsExpanded = true;
        }

        void InitGrid_2()
        {
            ClassHierarchyTreeView treevue = new ClassHierarchyTreeView(typeof(System.Windows.Threading.DispatcherObject)); //请勿使用Object类，因为在不同命名空间中出现相同名称类的时候，将会出现问题
            this.grid2.Children.Add(treevue);
        }
    }
}
