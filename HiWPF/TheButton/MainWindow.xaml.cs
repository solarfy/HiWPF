using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TheButton
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            //添加命令
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteOnExecute, PasteCanExecute));

            this.btnB.IsDefault = true; //默认对Enter键有反应
            this.btnB.IsCancel = true;  //默认对ESC键有反应（Espace)

            this.btnA.ClickMode = ClickMode.Hover;  //鼠标移动上即出发Click事件
            this.btnA.Foreground = Brushes.DarkSalmon;
            this.btnA.BorderBrush = Brushes.Magenta;
            this.btnA.BorderThickness = new Thickness(2);

            this.btnPaste.Command = ApplicationCommands.Paste;
            this.btnPaste.Content = ApplicationCommands.Paste.Text;

            //this.checkBox.SetBinding(CheckBox.IsCheckedProperty, nameof(Topmost));
            //this.checkBox.DataContext = this;   //"哪个对象"的Topmost
            //作用与上述写法相同
            Binding bind = new Binding(nameof(Topmost));
            bind.Source = this;
            this.checkBox.SetBinding(CheckBox.IsCheckedProperty, bind);

            Uri uri = new Uri("pack://application:,,/dog.jpg");
            BitmapImage bitmap = new BitmapImage(uri);
            Image img = new Image() { Source = bitmap, Height = 100, Width = 80};
            this.label.ToolTip = img;

            this.textBox.SelectionStart = this.textBox.Text.Length; //将光标置于文字之后
            this.textBox.AcceptsReturn = true;  //支持Enter换行
        }

        void PasteOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            this.Title = Clipboard.GetText();   //Clipboard剪贴板
        }

        void PasteCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = Clipboard.ContainsText();
        }

        private void ButtonTestA_Click(object sender, RoutedEventArgs e)
        {
            //按住Alt + A
            //MessageBox.Show("Test A");
            this.Title = "Test A";
        }

        private void ButtonTestB_Click(object sender, RoutedEventArgs e)
        {
            //Alt + B
            MessageBox.Show("Test B");
        }

        private void ButtonNavigateWeb_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = new Frame();
            frame.Source = new Uri(this.textBox.Text);

            Window win = new Window();
            win.Content = frame;
            win.Show();
           
        }

        RichTextBox rich = new RichTextBox();
        string strFilter = "Document Files(*.xaml)|*.xaml|All files(*.*)|*.*";
        private void ButtonRichTextBox_Click(object sender, RoutedEventArgs e)
        {
            rich.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            //Window win = new Window();
            //win.Content = rich;
            //win.Show();

            this.Content = rich;
        }

        /// <summary>
        /// 截取RichTextBox中的PreviewTextInput事件，将事件Handled设定为True，表示此事件已经被处理过了
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            //Ctrl + O 字体
            if (e.ControlText.Length > 0 && e.ControlText[0] == '\x0F')     //   \u000F
            {
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.CheckFileExists = true;
                dlg.Filter = strFilter;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FlowDocument flow = rich.Document;
                    TextRange range = new TextRange(flow.ContentStart, flow.ContentEnd);
                    Stream strm = null;

                    try
                    {
                        strm = new FileStream(dlg.FileName, FileMode.Open);
                        range.Load(strm, DataFormats.Xaml);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, this.Title);
                    }
                    finally
                    {
                        strm?.Close();
                    }

                    e.Handled = true;
                }
            }

            //Ctrl + S 字体
            if (e.ControlText.Length > 0 && e.ControlText[0] == '\x13')     //   \u0013
            {
                System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
                dlg.Filter = strFilter;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FlowDocument flow = rich.Document;
                    TextRange range = new TextRange(flow.ContentStart, flow.ContentEnd);
                    Stream strm = null;

                    try
                    {
                        strm = new FileStream(dlg.FileName, FileMode.Create);
                        range.Save(strm, DataFormats.Xaml);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, this.Title);
                    }
                    finally
                    {
                        strm?.Close();
                    }

                    e.Handled = true;
                }                
            }

            base.OnPreviewTextInput(e);
        }
    }
}
