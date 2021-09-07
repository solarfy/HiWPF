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
using System.Xml.Serialization;
using System.IO;
using DataEntryDemo.Model;
using Microsoft.Win32;

namespace DataEntryDemo.Pages
{
    /// <summary>
    /// SingleRecordDataEntry.xaml 的交互逻辑
    /// </summary>
    public partial class SingleRecordDataEntry : Page
    {
        const string strFilter = "Person Xml files (*.PersonXml)|*.PersonXml|All files (*.*)|*.*";

        XmlSerializer xml = new XmlSerializer(typeof(Person));

        public SingleRecordDataEntry()
        {
            InitializeComponent();

            ApplicationCommands.New.Execute(null, this);

            this.pnlPerson.Children[1].Focus();
        }

        private void NewOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.pnlPerson.DataContext = new Person();
        }

        private void OpenOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = strFilter;
            Person pers;

            if ((bool)dlg.ShowDialog(Application.Current.MainWindow))
            {
                try
                {
                    StreamReader reader = new StreamReader(dlg.FileName);
                    pers = (Person)xml.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Could not load file: {exc.Message}", this.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                this.pnlPerson.DataContext = pers;
            }

        }

        private void SaveOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = strFilter;

            if ((bool)dlg.ShowDialog())
            {
                try
                {
                    StreamWriter writer = new StreamWriter(dlg.FileName);
                    xml.Serialize(writer, this.pnlPerson.DataContext);
                    writer.Close(); 
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Could not save file: {exc.Message}", this.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }
    }
}
