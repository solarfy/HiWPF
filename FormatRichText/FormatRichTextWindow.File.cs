using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.IO;
using Microsoft.Win32;

namespace FormatRichText
{
    public partial class FormatRichTextWindow : Window
    {
        string[] formats =
        {
            DataFormats.Xaml, DataFormats.XamlPackage, DataFormats.Rtf, DataFormats.Text, DataFormats.Text
        };

        string strFilter = "XAML Document Files (*.xaml)|*.xaml|" + "XAML Packages Files (*.zip)|*.zip|" + "Rich Text Format Files (*.rtf)|*.rtf|" + "All files (*.*)|*.*";

        void AddFileToolBar(ToolBarTray tray, int band, int index)
        {
            ToolBar toolbar = new ToolBar();
            toolbar.Band = band;
            toolbar.BandIndex = index;
            tray.ToolBars.Add(toolbar);

            RoutedUICommand[] comms = { ApplicationCommands.New, ApplicationCommands.Open, ApplicationCommands.Save };

            string[] strImages = { "new.png", "open.png", "save.png" };

            for(int i = 0; i < 3; i++)
            {
                Button btn = new Button();
                btn.Command = comms[i];
                toolbar.Items.Add(btn);

                Image img = new Image();
                img.Source = new BitmapImage(new Uri($"pack://application:,,/Images/{strImages[i]}"));
                img.Stretch = Stretch.None;
                btn.Content = img;

                ToolTip tip = new ToolTip();
                tip.Content = comms[i].Text;
                btn.ToolTip = tip;
            }

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, OnNew));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OnOpen));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, OnSave));
        }

        void OnNew(object sender, ExecutedRoutedEventArgs args)
        {
            FlowDocument flow = this.txtbox.Document;
            TextRange range = new TextRange(flow.ContentStart, flow.ContentEnd);
            range.Text = "";
        }

        void OnOpen(object sender, ExecutedRoutedEventArgs args)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = strFilter;

            if ((bool)dlg.ShowDialog(this))
            {
                FlowDocument flow = new FlowDocument();
                TextRange range = new TextRange(flow.ContentStart, flow.ContentEnd);
                FileStream strm = null;

                try
                {
                    strm = new FileStream(dlg.FileName, FileMode.Open);
                    range.Load(strm, formats[dlg.FilterIndex - 1]);     //FilterIndex是用户选择文件后缀，注意此值是从1开始，所以此处需减1
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, this.Title);
                }
                finally 
                {
                    strm?.Close();
                }
            }
        }

        void OnSave(object sender, ExecutedRoutedEventArgs args)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = strFilter;

            if ((bool)dlg.ShowDialog(this))
            {
                FlowDocument flow = txtbox.Document;
                TextRange range = new TextRange(flow.ContentStart, flow.ContentEnd);
                FileStream strm = null;

                try
                {
                    strm = new FileStream(dlg.FileName, FileMode.Create);
                    range.Save(strm, formats[dlg.FilterIndex - 1]);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, this.Title);
                }
                finally
                {
                    strm?.Close();
                }
            }
        }
    }
}
