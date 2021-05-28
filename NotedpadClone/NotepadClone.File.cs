using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotepadClone
{
    public partial class NotepadClone : Window
    {
        protected string strFilter = "Text Document(*.txt)|*.txt|All Files(*.*)|*.*";

        void AddFileMenu(Menu menu)
        {
            //[文件]
            MenuItem itemFile = new MenuItem();
            itemFile.Header = "_File";
            menu.Items.Add(itemFile);

            //新建
            MenuItem itemNew = new MenuItem();
            itemNew.Header = "_New";
            itemNew.Command = ApplicationCommands.New;
            itemFile.Items.Add(itemNew);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewOnExecute));

            //打开
            MenuItem itemOpen = new MenuItem();
            itemOpen.Header = "_Open...";
            itemOpen.Command = ApplicationCommands.Open;
            itemFile.Items.Add(itemOpen);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenOnExecute));

            //保存
            MenuItem itemSave = new MenuItem();
            itemSave.Header = "_Save";
            itemSave.Command = ApplicationCommands.Save;
            itemFile.Items.Add(itemSave);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveOnExecute));

            //另存为
            MenuItem itemSaveAs = new MenuItem();
            itemSaveAs.Header = "Save _As...";
            itemSaveAs.Command = ApplicationCommands.SaveAs;
            itemFile.Items.Add(itemSaveAs);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, SaveAsOnExecute));

            //打印
            itemFile.Items.Add(new Separator());
            AddPrintMenuItems(itemFile);
            itemFile.Items.Add(new Separator());

            //关闭
            MenuItem itemExit = new MenuItem();
            itemExit.Header = "E_xit";
            itemExit.Click += ExitOnClick;
            itemFile.Items.Add(itemExit);
        }

        protected virtual void NewOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            if (!OkToTrash())
                return;

            txtbox.Text = "";
            strLoadedFile = null;
            isFileDirty = false;
            UpdateTitle();
        }

        void OpenOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            if (!OkToTrash())
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = strFilter;
            if ((bool)dlg.ShowDialog(this))
            {
                LoadFile(dlg.FileName);
            }
        }

        void SaveOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            if (strLoadedFile == null || strLoadedFile.Length == 0)
                DisplaySaveDialog("");
            else
                SaveFile(strLoadedFile);
        }

        void SaveAsOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            DisplaySaveDialog(strLoadedFile);
        }

        bool DisplaySaveDialog(string strFileName)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = strFilter;
            dlg.FileName = strFileName;

            if ((bool)dlg.ShowDialog(this))
            {
                SaveFile(dlg.FileName);
                return true;
            }

            return false;   //for OkToTrash
        }

        void ExitOnClick(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

        //决定是保存或放弃已修改且尚未保存的文件内容
        bool OkToTrash()
        {
            if (!isFileDirty)
                return true;

            MessageBoxResult result = MessageBox.Show("The text in the file " + strLoadedFile + " has changed\n\n" + "Do you want to save the changes?"
                , strAppTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Cancel)
                return false;
            else if (result == MessageBoxResult.No)
                return true;
            else //result == MessageBoxResult.Yes
            {
                if (!string.IsNullOrEmpty(strLoadedFile))
                    return SaveFile(strLoadedFile);
                return DisplaySaveDialog("");
            }
        }

        void LoadFile(string strFileName)
        {
            try
            {
                txtbox.Text = File.ReadAllText(strFileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on File Open: " + exc.Message, strAppTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            strLoadedFile = strFileName;
            UpdateTitle();
            txtbox.SelectionStart = 0;
            txtbox.SelectionLength = 0;
            isFileDirty = false;
        }

        bool SaveFile(string strFileName)
        {
            try
            {
                File.WriteAllText(strFileName, txtbox.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Error on File Save {exc.Message}", strAppTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return false;
            }

            strLoadedFile = strFileName;
            UpdateTitle();
            isFileDirty = false;
            return true;
        }
    }
}
