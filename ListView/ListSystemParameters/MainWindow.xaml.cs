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
using System.ComponentModel;
using System.Reflection;

namespace ListSystemParameters
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "List System Parameters";

            InitGrid_1();
            InitGrid_2();
        }

        void InitGrid_1()
        {
            ListView lstvue = new ListView();
            this.grid1.Children.Add(lstvue);

            GridView grdvue = new GridView();
            lstvue.View = grdvue;
            //lstvue.View = null;   //其作用根ListBox类似，显示ToString方法返回的字符串

            GridViewColumn col = new GridViewColumn();
            col.Header = "Property Name";
            col.Width = 200;
            col.DisplayMemberBinding = new Binding(nameof(SystemParam.Name));
            grdvue.Columns.Add(col);

            col = new GridViewColumn();
            col.Header = "Value";
            col.Width = 200;
            col.DisplayMemberBinding = new Binding(nameof(SystemParam.Value));
            grdvue.Columns.Add(col);

            PropertyInfo[] props = typeof(SystemParameters).GetProperties();

            //数组排序 方法1 using System.Collections.Generic;
            //Array.Sort(props, new PropertyInfoCompare());  
            //Array.Reverse(props);   //颠倒数组

            foreach (PropertyInfo prop in props)
            {
                /*  SytetemParameters的每个属性都以两种形式存在。例如，MenuBarHeight是属于double类型，而MenuBarHeightKey是属于ResourceKey类型。
                 *  ResourceKey类型的属性几乎都是用在XAML中                 
                 */ 

                if (prop.PropertyType != typeof(ResourceKey))       //过滤掉ResourceKey类型
                {
                    SystemParam sysparm = new SystemParam();
                    sysparm.Name = prop.Name;
                    sysparm.Value = prop.GetValue(null, null);
                    lstvue.Items.Add(sysparm);
                    //数组排序 方法二  using System.ComponentModel;
                    SortDescription sort = new SortDescription(nameof(SystemParam.Name), ListSortDirection.Ascending);          //建立一个SortDescription对象            
                    lstvue.Items.SortDescriptions.Add(sort);        //将排序的对象加入到排序集合中
                }
            }

            //排序 方法三 用SortedList<TKey, TValue>对象 下个方法中将用到
        }

        void InitGrid_2()
        {
            ListView lstvue = new ListView();
            this.grid2.Children.Add(lstvue);

            GridView grdvue = new GridView();
            lstvue.View = grdvue;

            GridViewColumn col = new GridViewColumn();
            col.Header = "Property Name";
            col.Width = 200;
            col.DisplayMemberBinding = new Binding(nameof(SystemParam.Name));
            grdvue.Columns.Add(col);

            col = new GridViewColumn();
            col.Header = "Value";
            col.Width = 200;
            grdvue.Columns.Add(col);

            DataTemplate template = new DataTemplate(typeof(string));
            FrameworkElementFactory factoryTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            factoryTextBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            factoryTextBlock.SetValue(TextBlock.TextProperty, new Binding(nameof(SystemParam.Value)));
            template.VisualTree = factoryTextBlock;
            col.CellTemplate = template;

            PropertyInfo[] props = typeof(SystemParameters).GetProperties();

            SortedList<string, SystemParam> sortlist = new SortedList<string, SystemParam>();

            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType != typeof(ResourceKey))
                {
                    SystemParam sysparm = new SystemParam();
                    sysparm.Name = prop.Name;
                    sysparm.Value = prop.GetValue(null, null);
                    sortlist.Add(prop.Name, sysparm);
                }
            }

            lstvue.ItemsSource = sortlist.Values;       //注意此处是Values，因为Value的类型是SystemParam的类型
        }
    }
}
