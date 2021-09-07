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
    /// FilterWithText.xaml 的交互逻辑
    /// </summary>
    public partial class FilterWithText : Page
    {
        ListCollectionView collview;

        public FilterWithText()
        {
            InitializeComponent();
        }

        private void FileOpenOnClick(object sender, RoutedEventArgs e)
        {
            People people = People.Load(Application.Current.MainWindow);

            if (people != null)
            {
                collview = new ListCollectionView(people);

                collview.SortDescriptions.Add(new SortDescription("BirthDate", ListSortDirection.Ascending));

                this.txtboxFilter.Text = "";
                collview.Filter = FirstNameFilter;

                this.lstbox.ItemsSource = collview;

                if (this.lstbox.Items.Count > 0)
                    this.lstbox.SelectedIndex = 0;
            }
        }

        bool FirstNameFilter(object obj)
        {
            return (obj as Person).FirstName.StartsWith(this.txtboxFilter.Text, StringComparison.CurrentCultureIgnoreCase);
        }

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (collview == null)
                return;

            collview.Refresh(); //刷新collview，使其进行重新排序，过滤，分组等操作
        }

        
    }
}
