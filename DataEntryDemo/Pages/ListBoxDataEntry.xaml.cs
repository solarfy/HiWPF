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
    /// ListBoxDataEntry.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxDataEntry : Page
    {
        ListCollectionView collview;
        People people;

        public ListBoxDataEntry()
        {
            InitializeComponent();

            ApplicationCommands.New.Execute(null, this);

            this.pnlPerson.Children[1].Focus();
        }

        void InitializeNewPeopleObject()
        {
            //创建一个基于People的ListCollectionView对象
            collview = new ListCollectionView(people);

            //定义一个属性，根据该属性值在视图上进行排序
            collview.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));

            //通过ListCollectionView链接ListBox和PersonPanel
            this.lstbox.ItemsSource = collview;     //默认的会将ListCollectionView的CurrentItems属性设定为ListBoxSelectedItem属性            
            this.pnlPerson.DataContext = collview;

            if (this.lstbox.Items.Count > 0)
                this.lstbox.SelectedIndex = 0;
        }

        private void AddOnClick(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            people.Add(person);
            this.lstbox.SelectedItem = person;
            this.pnlPerson.Children[1].Focus();
            this.collview.Refresh();        //刷新CollectionView，进行重新排序等操作
        }

        private void DeleteOnClick(object sender, RoutedEventArgs e)
        {
            if (this.lstbox.SelectedItem != null)
            {
                people.Remove(this.lstbox.SelectedItem as Person);

                if (this.lstbox.Items.Count > 0)
                    this.lstbox.SelectedIndex = 0;
                else
                    AddOnClick(sender, e);
            }
        }

        private void NewOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            people = new People();
            people.Add(new Person());
            InitializeNewPeopleObject();
        }

        private void OpenOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            people = People.Load(Application.Current.MainWindow);

            if (people != null)
                InitializeNewPeopleObject();
        }

        private void SaveOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            people.Save(Application.Current.MainWindow);
        }
    }
}
