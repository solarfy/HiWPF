using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.IO;
using System;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;

namespace NotepadClone
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NotepadClone : Window
    {
        protected string strAppTitle;
        protected string strAppData;
        protected NotepadCloneSetting settings;
        protected bool isFileDirty = false;

        protected Menu menu;
        protected TextBox txtbox;
        protected StatusBar status;

        string strLoadedFile;
        StatusBarItem statLineCol;

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.ShutdownMode = ShutdownMode.OnMainWindowClose;
            app.Run(new NotepadClone());
        }

        public NotepadClone()
        {
            Assembly asmbly = Assembly.GetExecutingAssembly();  //获取当前执行代码的程序集
            AssemblyTitleAttribute title = (AssemblyTitleAttribute)asmbly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];    //获取此程序集的程序集说明
            strAppTitle = title.Title;  //程序集标题
            AssemblyProductAttribute product = (AssemblyProductAttribute)asmbly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0];    //获取此程序集的程序集清单
            strAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HiWPF\\" + product.Product + "\\" + product.Product + ".Settings.xml");   //配置文件保存路径  C:\Users\Yang\AppData\Local\HiWPF\NotedpadClone\NotedpadClone.Settings.xml

            DockPanel dock = new DockPanel();
            this.Content = dock;

            menu = new Menu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);

            status = new StatusBar();
            dock.Children.Add(status);
            DockPanel.SetDock(status, Dock.Bottom);

            statLineCol = new StatusBarItem();
            statLineCol.HorizontalAlignment = HorizontalAlignment.Right;
            status.Items.Add(statLineCol);
            DockPanel.SetDock(statLineCol, Dock.Right);

            txtbox = new TextBox();
            txtbox.AcceptsReturn = true;
            txtbox.AcceptsTab = true;
            txtbox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            txtbox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            txtbox.TextChanged += TextBoxOnTextChanged;
            txtbox.SelectionChanged += TextBoxOnSelectionChanged;
            dock.Children.Add(txtbox);

            AddFileMenu(menu);  //NotepadClone.File.cs
            AddEditMenu(menu);  //NotepadClone.Edit.cs
            AddFormatMenu(menu); //NotepadClone.Format.cs
            AddViewMenu(menu);  //NotepadClone.View.cs
            AddHelpMenu(menu);  //NotedpadClone.Help.cs

            settings = (NotepadCloneSetting)LoadSettings();

            this.WindowState = settings.WindowState;

            if (settings.RestoreBounds != Rect.Empty)
            {
                this.Left = settings.RestoreBounds.Left;
                this.Top = settings.RestoreBounds.Top;
                this.Width = settings.RestoreBounds.Width;
                this.Height = settings.RestoreBounds.Height;
            }

            txtbox.TextWrapping = settings.TextWrapping;
            txtbox.FontFamily = new FontFamily(settings.FontFamily);
            txtbox.FontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(settings.FontStyle);
            txtbox.FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(settings.FontWeight);
            txtbox.FontStretch = (FontStretch)new FontStretchConverter().ConvertFromString(settings.FontStretch);
            txtbox.FontSize = settings.FontSize;

            this.Loaded += WindowOnLoaded;

            txtbox.Focus();            
        }

        protected virtual object LoadSettings()
        {
            return NotepadCloneSetting.Load(typeof(NotepadCloneSetting), strAppData);
        }

        void WindowOnLoaded(object sender, RoutedEventArgs args)
        {
            ApplicationCommands.New.Execute(null, this);

            string[] strArgs = Environment.GetCommandLineArgs();

            if (strArgs.Length > 1)
            {
                if (File.Exists(strArgs[1]))
                {
                    LoadFile(strArgs[1]);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Cannnot find the" + Path.GetFileName(strArgs[1]) + " file.\r\n\r\n" + "Do you want to create a new file?"
                        , strAppTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                        this.Close();
                    else if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            File.Create(strLoadedFile = strArgs[1]).Close();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Error on File Creation: " + exc.Message, strAppTitle, MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            return;
                        }

                        UpdateTitle();
                    }
                    //No action for "NO"
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //e.Cancel = !OkToTrash();
            settings.RestoreBounds = this.RestoreBounds;    //在最小化或最大化之前获取窗口的大小和位置
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            settings.WindowState = this.WindowState;
            settings.TextWrapping = txtbox.TextWrapping;
            settings.FontFamily = txtbox.FontFamily.ToString();

            settings.FontStyle = new FontStyleConverter().ConvertToString(txtbox.FontStyle);
            settings.FontWeight = new FontWeightConverter().ConvertToString(txtbox.FontWeight);
            settings.FontStretch = new FontStretchConverter().ConvertToString(txtbox.FontStretch);
            settings.FontSize = txtbox.FontSize;

            SaveSettings();
        }

        protected virtual void SaveSettings()
        {
            settings.Save(strAppData);
        }

        protected void UpdateTitle()
        {
            if (strLoadedFile == null)
                this.Title = "Untitled - " + strAppTitle;
            else
                this.Title = Path.GetFileName(strLoadedFile) + " - " + strAppTitle;
        }

        void TextBoxOnTextChanged(object sender, RoutedEventArgs args)
        {
            isFileDirty = true; //表示文件有变动
        }

        //在状态栏中显示文字光标的位置在第几行第几列
        void TextBoxOnSelectionChanged(object sender, RoutedEventArgs args)
        {
            int iChar = txtbox.SelectionStart;
            int iLine = txtbox.GetLineIndexFromCharacterIndex(iChar);

            if (iLine == -1)
            {
                statLineCol.Content = "";
                return;
            }

            int iCol = iChar - txtbox.GetCharacterIndexFromLineIndex(iLine);
            string str = string.Format("Line {0} Col {1}", iLine + 1, iCol + 1);

            if (txtbox.SelectionLength > 0)
            {
                iChar += txtbox.SelectionLength;
                iLine = txtbox.GetLineIndexFromCharacterIndex(iChar);
                iCol = iChar - txtbox.GetCharacterIndexFromLineIndex(iLine);
                str += string.Format(" - Line {0} Col {1}", iLine + 1, iCol + 1);
            }

            statLineCol.Content = str;
        }

    }
}
