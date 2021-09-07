using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;

namespace DataEntryDemo.Model
{
    //ObservableCollection定义了两个重要的事件：PropertyChanged和CollectionChanged ；
    //当collection中某个成员的属性改变时，PropertyChanged事件发生，前提是ObservableCollection基于一个实现了INotifyPropertyChanged接口对象，
    //当该对象的property改变时，会发出自己的PropertyChanged事件（Person类中已实现）；
    //当项目整体改变（添加项目，移除项目，取代某个项目，或在collection内移动项目）都会触发这个事件。
    public class People : ObservableCollection<Person>
    {
        const string strFilter = "People XML files (*.PeopleXml)|*.PeopleXml|All files (*.*)|*.*";

        public static People Load(Window win)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = strFilter;
            dlg.InitialDirectory = Environment.CurrentDirectory;
            //Console.WriteLine(Environment.CurrentDirectory);
            People people = null;

            if ((bool)dlg.ShowDialog(win))
            {
                try
                {
                    StreamReader reader = new StreamReader(dlg.FileName);
                    XmlSerializer xml = new XmlSerializer(typeof(People));
                    people = (People)xml.Deserialize(reader);
                    reader.Close();                    
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Could not load file: {exc.Message}", win.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    people = null;
                }
            }

            return people;
        }

        public bool Save(Window win)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = strFilter;
            dlg.InitialDirectory = Environment.CurrentDirectory;

            if ((bool)dlg.ShowDialog(win))
            {
                try
                {
                    StreamWriter writer = new StreamWriter(dlg.FileName);
                    XmlSerializer xml = new XmlSerializer(GetType());
                    xml.Serialize(writer, this);
                    writer.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Could not save file: {exc.Message}", win.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            return true;
        }
    }
}
