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
    /// MultiRecordDataEntry.xaml 的交互逻辑
    /// </summary>
    public partial class MultiRecordDataEntry : Page
    {
        People people;
        int index;

        public MultiRecordDataEntry()
        {
            InitializeComponent();

            ApplicationCommands.New.Execute(null, this);
            this.pnlPerson.Children[1].Focus();     //获取panel中第一个textbox的焦点
        }

        void EnableAndDisableButtons()
        {
            this.btnPrev.IsEnabled = index != 0;
            this.btnNext.IsEnabled = index < people.Count - 1;
            this.pnlPerson.Children[1].Focus();
        }

        private void FirstOnClick(object sender, RoutedEventArgs e)
        {
            this.pnlPerson.DataContext = people[index = 0];
            EnableAndDisableButtons();
        }

        private void PrevOnClick(object sender, RoutedEventArgs e)
        {
            this.pnlPerson.DataContext = people[index -= 1];
            EnableAndDisableButtons();
        }

        private void NextOnClick(object sender, RoutedEventArgs e)
        {
            this.pnlPerson.DataContext = people[index += 1];
            EnableAndDisableButtons();
        }

        private void LastOnClick(object sender, RoutedEventArgs e)
        {
            this.pnlPerson.DataContext = people[index = people.Count - 1];
            EnableAndDisableButtons();
        }

        private void AddOnClick(object sender, RoutedEventArgs e)
        {
            people.Insert(index = people.Count, new Person());
            this.pnlPerson.DataContext = people[index];
            EnableAndDisableButtons();
        }

        private void DelOnClick(object sender, RoutedEventArgs e)
        {
            people.RemoveAt(index);

            if (people.Count == 0)
                people.Insert(0, new Person());

            if (index > people.Count - 1)       //删除的最后一个，默认选中前一个
                index--;

            pnlPerson.DataContext = people[index];  //RemoveAt 集合内部数据自动调整
            EnableAndDisableButtons();
        }

        void InitializeNewPeopleObject()
        {
            index = 0;

            if (people.Count == 0)
                people.Insert(0, new Person());

            this.pnlPerson.DataContext = people[0];
            EnableAndDisableButtons();
        }

        private void NewOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            people = new People();
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
