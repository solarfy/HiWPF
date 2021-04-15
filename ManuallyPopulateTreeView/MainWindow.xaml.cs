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

namespace ManuallyPopulateTreeView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        StackPanel stack;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Manually Populate TreeView";

            InitGrid_1();
            //InitGrid_2();   //很耗时，不建议运行
            InitGrid_3();
        }

        void InitGrid_1()
        {
            TreeView tree = new TreeView();
            this.grid1.Children.Add(tree);

            TreeViewItem itemAnimal = new TreeViewItem();
            itemAnimal.Header = "Animal";
            tree.Items.Add(itemAnimal);

            TreeViewItem itemDog = new TreeViewItem();
            itemDog.Header = "Dog";
            itemDog.Items.Add("Poodle");
            itemDog.Items.Add("Irish Setter");
            itemDog.Items.Add("German Shepherd");
            itemAnimal.Items.Add(itemDog);

            TreeViewItem itemCat = new TreeViewItem();
            itemCat.Header = "Cat";
            itemCat.Items.Add("Calico");

            TreeViewItem item = new TreeViewItem();
            item.Header = "Alley Cat";
            itemCat.Items.Add(item);

            Button btn = new Button();
            btn.Content = "Noodles";
            itemCat.Items.Add(btn);

            itemCat.Items.Add("Siamese");
            itemAnimal.Items.Add(itemCat);

            TreeViewItem itemPrimate = new TreeViewItem();
            itemPrimate.Header = "Primate";
            itemPrimate.Items.Add("Chimpanzee");
            itemPrimate.Items.Add("Bonobo");
            itemPrimate.Items.Add("Human");
            itemAnimal.Items.Add(itemPrimate);

            TreeViewItem itemMineral = new TreeViewItem();
            itemMineral.Header = "Mineral";
            itemMineral.Items.Add("Calcium");
            itemMineral.Items.Add("Zinc");
            itemMineral.Items.Add("Iron");
            tree.Items.Add(itemMineral);

            TreeViewItem itemVegetable = new TreeViewItem();
            itemVegetable.Header = "Vegetable";
            itemVegetable.Items.Add("Carrot");
            itemVegetable.Items.Add("Asparagus");
            itemVegetable.Items.Add("Broccoli");
            tree.Items.Add(itemVegetable);
        }

        void InitGrid_2()
        {
            TreeView tree = new TreeView();
            this.grid2.Children.Add(tree);

            TreeViewItem item = new TreeViewItem();
            item.Header = Path.GetPathRoot(Environment.SystemDirectory);        //Sytstem.IO
            item.Tag = new DirectoryInfo(item.Header as string);
            tree.Items.Add(item);

            //递归填充
            GetSubdirectories(item);

            /* 更好的做法是，不要一开始就取得所有的子目录，而是需要时才取得。比方说，当用户点击某个目录旁边的加号是，采取取得其子目录。
             * 实际上，这有一点晚，因为加号只有在某个项目具有孩子时才会显示，所以程序必须比用户提早一步。
             * 每个项目只要被显示出来，其子项目也要备妥，但子-子项目就不必了                          
             */ 
        }

        void GetSubdirectories(TreeViewItem item)
        {
            DirectoryInfo dir = item.Tag as DirectoryInfo;
            DirectoryInfo[] subdirs;

            /*  这里使用try-catch，是因为某些目录是禁止访问的 */                          
            try
            {
                subdirs = dir.GetDirectories();
            }
            catch
            {
                return;
            }

            foreach(DirectoryInfo subdir in subdirs)
            {
                TreeViewItem subitem = new TreeViewItem();
                subitem.Header = subdir.Name;
                subitem.Tag = subdir;
                item.Items.Add(subitem);

                GetSubdirectories(subitem);
            }
        }

        void InitGrid_3()
        {
            ColumnDefinition coldef = new ColumnDefinition();
            coldef.Width = new GridLength(100, GridUnitType.Star);
            this.grid3.ColumnDefinitions.Add(coldef);

            coldef = new ColumnDefinition();
            coldef.Width = GridLength.Auto;
            this.grid3.ColumnDefinitions.Add(coldef);

            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(50, GridUnitType.Star);
            this.grid3.ColumnDefinitions.Add(coldef);

            DirectoryTreeView tree = new DirectoryTreeView();
            tree.SelectedItemChanged += TreeViewOnSelectedItemChanged;
            this.grid3.Children.Add(tree);
            Grid.SetColumn(tree, 0);

            GridSplitter split = new GridSplitter();
            split.Width = 6;
            split.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            this.grid3.Children.Add(split);
            Grid.SetColumn(split, 1);

            ScrollViewer scroll = new ScrollViewer();
            this.grid3.Children.Add(scroll);
            Grid.SetColumn(scroll, 2);

            stack = new StackPanel();
            scroll.Content = stack;
        }

        void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            DirectoryTreeViewItem item = args.NewValue as DirectoryTreeViewItem;

            stack.Children.Clear();

            FileInfo[] infos;

            try
            {
                infos = item.DirectoryInfo.GetFiles();
            }
            catch
            {
                return;
            }

            foreach (FileInfo info in infos)
            {
                TextBlock text = new TextBlock();
                text.Text = info.Name;
                stack.Children.Add(text);
            }
        }
    }
}
