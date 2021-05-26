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

namespace ExploreDependencyProperties
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            this.Title = "Explore Dependency Properties";

            Grid grid = new Grid();
            this.Content = grid;

            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col);

            col = new ColumnDefinition();
            col.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(col);

            col = new ColumnDefinition();
            col.Width = new GridLength(3, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col);

            ClassHierarchyTreeView treevue = new ClassHierarchyTreeView(typeof(DependencyObject));
            grid.Children.Add(treevue);
            Grid.SetColumn(treevue, 0);

            GridSplitter split = new GridSplitter();
            split.HorizontalAlignment = HorizontalAlignment.Center;
            split.VerticalAlignment = VerticalAlignment.Stretch;
            split.Width = 6;
            grid.Children.Add(split);
            Grid.SetColumn(split, 1);

            DependencyPropertyListView lstvue = new DependencyPropertyListView();
            grid.Children.Add(lstvue);
            Grid.SetColumn(lstvue, 2);
          
            lstvue.SetBinding(DependencyPropertyListView.TypeProperty, "SelectedItem.Type");          //SelectedItem是TypeTreeViewItem的类型，TypeTreeViewItem中含有Type属性  
            lstvue.DataContext = treevue;           
        }
    }
}
