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
using DataEntryDemo.Model;
using System.ComponentModel;

namespace DataEntryDemo.Pages
{
    /// <summary>
    /// CollectionViewWithFilter.xaml 的交互逻辑
    /// </summary>
    public partial class CollectionViewWithFilter : Page
    {
        ListCollectionView collview;

        public CollectionViewWithFilter()
        {
            InitializeComponent();            
        }

        //只能通过Ctrl + O打开
        private void OpenOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            People people = People.Load(Application.Current.MainWindow);

            if (people != null)
            {
                collview = new ListCollectionView(people);

                collview.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));

                this.lstbox.ItemsSource = collview;

                if (this.lstbox.Items.Count > 0)
                    this.lstbox.SelectedIndex = 0;

                this.radioAll.IsChecked = true;                
            }
        }

        //此处命令与之前存在冲突，可能无法触发，所以用click强制触发
        private void OpenOnClick(object sender, RoutedEventArgs e)
        {
            OpenOnExecuted(null, null);
        }

        private void OpenOnCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RadionOnChecked(object sender, RoutedEventArgs e)
        {
            if (collview == null)
                return;

            RadioButton radio = e.Source as RadioButton;

            switch (radio.Name)
            {
                case "radioLiving":
                    collview.Filter = PersonIsLiving;   //过滤一次只能赋值一个方法
                    break;
                case "radioDead":
                    collview.Filter = PersonIsDead;
                    break;
                case "radioAll":
                    collview.Filter = null;
                    break;
                default:
                    break;
                
            }
        }

        bool PersonIsLiving(object obj)
        {
            return (obj as Person).DeathDate == null;
        }

        bool PersonIsDead(object obj)
        {
            return (obj as Person).DeathDate != null;
        }        
    }
}
