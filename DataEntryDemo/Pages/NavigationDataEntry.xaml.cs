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

namespace DataEntryDemo.Pages
{    
    /// <summary>
    /// NavigationDataEntry.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationDataEntry : Page
    {
        People people;

        public NavigationDataEntry()
        {
            InitializeComponent();

            //模拟新建文件命令
            ApplicationCommands.New.Execute(null, this);
            //获取第一个TextBox控件的焦点
            this.pnlPerson.Children[1].Focus();
        }

        void InitializeNewPeopleObject()
        {
            this.navBar.Collection = people;
            this.navBar.ItemType = typeof(Person);
            this.pnlPerson.DataContext = people;    //DataContext设置为集合对象；People集合是以ListCollectionView为基础，所以可维护当前的记录，而当前的记录正是PersonPanel所显示的
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
            InitializeNewPeopleObject();
        }

        private void SaveOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            people.Save(Application.Current.MainWindow);
        }
    }
}
