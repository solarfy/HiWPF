using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;                                  //for StringBuilder
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;           //for StatusBarItem
using System.Windows.Markup;                        //for XmlReader.Load
using System.Xml;                                   //for XmlTextReader
using System.Windows.Threading;                     //for DispatcherUnhandledExceptionEventArgs
using System.IO;
using System.Windows.Media;

namespace XamlCruncher
{
    class XamlCruncher : NotepadClone.NotepadClone
    {
        Frame frameParent;
        Window win;
        StatusBarItem statusParse;
        int tabSpaces = 4;

        XamlCruncherSetting settingsXaml;

        XamlOrientationMenuItem itemOrientation;
        bool isSuspendParsing = false;  //是否暂停解析

        [STAThread]
        public new static void Main()   //覆盖掉父类中的Main方法
        {
            Application app = new Application();
            app.ShutdownMode = ShutdownMode.OnMainWindowClose;
            app.Run(new XamlCruncher());
        }

        public bool IsSuspendParsing
        {
            set => isSuspendParsing = value;
            get => isSuspendParsing;
        }

        public XamlCruncher()
        {
            strFilter = "XAML Files(*.xaml)|*.xaml|All Files(*.*)|*.*";

            DockPanel dock = txtbox.Parent as DockPanel;
            dock.Children.Remove(txtbox);

            Grid grid = new Grid();
            dock.Children.Add(grid);
            for (int i = 0; i < 3; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new GridLength(0);
                grid.RowDefinitions.Add(rowdef);

                ColumnDefinition coldef = new ColumnDefinition();
                coldef.Width = new GridLength(0);
                grid.ColumnDefinitions.Add(coldef);
            }

            grid.RowDefinitions[0].Height = new GridLength(100, GridUnitType.Star);
            grid.ColumnDefinitions[0].Width = new GridLength(100, GridUnitType.Star);

            GridSplitter split = new GridSplitter();
            split.HorizontalAlignment = HorizontalAlignment.Stretch;
            split.VerticalAlignment = VerticalAlignment.Center;
            split.Height = 6;
            grid.Children.Add(split);
            Grid.SetRow(split, 1);
            Grid.SetColumn(split, 0);
            Grid.SetColumnSpan(split, 3);

            split = new GridSplitter();
            split.HorizontalAlignment = HorizontalAlignment.Center;
            split.VerticalAlignment = VerticalAlignment.Stretch;
            split.Height = 6;
            grid.Children.Add(split);
            Grid.SetRow(split, 1);
            Grid.SetRowSpan(split, 3);

            frameParent = new Frame();
            frameParent.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            grid.Children.Add(frameParent);

            txtbox.TextChanged += TextBoxOnTextChanged;
            grid.Children.Add(txtbox);

            settingsXaml = (XamlCruncherSetting)settings;

            MenuItem itemXaml = new MenuItem();
            itemXaml.Header = "_Xaml";
            menu.Items.Insert(menu.Items.Count - 1, itemXaml);

            itemOrientation = new XamlOrientationMenuItem(grid, txtbox, frameParent);
            itemOrientation.Oreientation = settingsXaml.Orientation;
            itemXaml.Items.Add(itemOrientation);

            MenuItem itemTabs = new MenuItem();
            itemTabs.Header = "_Tab Spaces...";
            itemTabs.Click += TabSpacesOnClick;

            MenuItem itemNoParse = new MenuItem();
            itemNoParse.Header = "_Suspend Parsing";
            itemNoParse.IsCheckable = true;
            itemNoParse.SetBinding(MenuItem.IsCheckedProperty, nameof(IsSuspendParsing));
            itemNoParse.DataContext = this;
            itemXaml.Items.Add(itemNoParse);

            InputGestureCollection collGest = new InputGestureCollection();
            collGest.Add(new KeyGesture(Key.F6));
            RoutedUICommand commRepare = new RoutedUICommand("_Reparse", "Reparse", GetType(), collGest);

            MenuItem itemReparse = new MenuItem();
            itemReparse.Command = commRepare;
            itemXaml.Items.Add(itemReparse);

            this.CommandBindings.Add(new CommandBinding(commRepare, ReparseOnExecuted));

            collGest = new InputGestureCollection();
            collGest.Add(new KeyGesture(Key.F7));
            RoutedUICommand commShowWin = new RoutedUICommand("Show _Window", "ShowWindow", GetType(), collGest);

            MenuItem itemShowWin = new MenuItem();
            itemShowWin.Command = commShowWin;
            itemXaml.Items.Add(itemShowWin);

            this.CommandBindings.Add(new CommandBinding(commShowWin, ShowWindowOnExecuted, ShowWindowCanExecuted));

            MenuItem itemTemplate = new MenuItem();
            itemTemplate.Header = "Save as Startup _Document";
            itemTemplate.Click += NewStartupOnClick;
            itemXaml.Items.Add(itemTemplate);

            MenuItem itemXamlHelp = new MenuItem();
            itemXamlHelp.Header = "_Help";
            itemXamlHelp.Click += HelpOnClick;
            MenuItem itemHelp = (MenuItem)menu.Items[menu.Items.Count - 1];
            itemHelp.Items.Insert(0, itemXamlHelp);

            statusParse = new StatusBarItem();
            status.Items.Insert(0, statusParse);
            status.Visibility = Visibility.Visible;

            //某些时候建立自XAML的element tree中有些东西会抛出异常，此异常可能会造成该程序结束，尽管并非是程序本身的错误
            //所以设置了UnhandledException事件处理函数，处理事件的方式是在状态栏显示此信息
            this.Dispatcher.UnhandledException += DispatcherOnUnhandledException;   //UnhandledException 未处理的异常
        }

        protected override void NewOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            base.NewOnExecute(sender, args);
            string str = ((XamlCruncherSetting)settings).StartupDocument;
            //确保下一次替换不会增加太多
            str = str.Replace("\r\n", "\n");
            //用回车/换行符替换换行符
            str = str.Replace("\n", "\r\n");
            txtbox.Text = str;
            isFileDirty = false;
        }

        protected override object LoadSettings()
        {
            return XamlCruncherSetting.Load(typeof(XamlCruncherSetting), strAppData);
        }

        protected override void SaveSettings()
        {
            ((XamlCruncherSetting)settings).Save(strAppData);
        }

        void TabSpacesOnClick(object sender, RoutedEventArgs args)
        {
            XamlTabSpacesDialog dlg = new XamlTabSpacesDialog();
            dlg.Owner = this;
            dlg.TabSpaces = settingsXaml.TabSpaces;

            if ((bool)dlg.ShowDialog().GetValueOrDefault())
            {
                settingsXaml.TabSpaces = dlg.TabSpaces;
            }
        }

        void ReparseOnExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            Parse();
        }

        void ShowWindowCanExecuted(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = (win != null);
        }

        void ShowWindowOnExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            if (win != null)
                win.Show();
        }

        void NewStartupOnClick(object sender, RoutedEventArgs args)
        {
            ((XamlCruncherSetting)settings).StartupDocument = txtbox.Text;
        }

        void HelpOnClick(object sender, RoutedEventArgs args)
        {
            Uri uri = new Uri("pack://application:,,/XamlCruncherHelp.xaml");
            Stream stream = Application.GetResourceStream(uri).Stream;

            Window win = new Window();
            win.Title = "XAML Cruncher Help";
            win.Content = XamlReader.Load(stream);
            win.Show();

            //另一种方法
            //Frame frame = new Frame();
            //frame.Source = new Uri("pack://application:,,/XamlCruncherHelp.xaml");
            //win.Content = frame;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Source == txtbox && e.Key == Key.Tab)
            {
                string strInsert = new string(' ', tabSpaces);
                int iChar = txtbox.SelectionStart;
                int iLine = txtbox.GetLineIndexFromCharacterIndex(iChar);

                if (iLine != -1)
                {
                    int iCol = iChar - txtbox.GetCharacterIndexFromLineIndex(iLine);
                    strInsert = new string(' ', settingsXaml.TabSpaces - iCol % settingsXaml.TabSpaces);
                }

                txtbox.SelectedText = strInsert;
                txtbox.CaretIndex = txtbox.SelectionStart + txtbox.SelectionLength;
                e.Handled = true;
            }
        }

        void TextBoxOnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (IsSuspendParsing)
                txtbox.Foreground = SystemColors.WindowTextBrush;
            else
                Parse();
        }

        //解析XAML字符串
        void Parse()
        {
            StringReader strreader = new StringReader(txtbox.Text);
            XmlTextReader xmlreader = new XmlTextReader(strreader);

            try
            {
                object obj = XamlReader.Load(xmlreader);
                txtbox.Foreground = SystemColors.WindowTextBrush;

                if (obj is Window)
                {
                    //Window类型的对象，需在按下F7之后产生一个独立的窗口
                    win = obj as Window;
                    statusParse.Content = "Press F7 to display window";
                }
                else
                {
                    win = null;
                    frameParent.Content = obj;
                    statusParse.Content = "OK";
                }
            }
            catch (Exception exc)
            {
                //如果产生异常，TextBox的文字转成红色，并在状态栏显示错误信息
                txtbox.Foreground = Brushes.Red;    
                statusParse.Content = exc.Message;
            }
        }

        void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            statusParse.Content = "Unhandled Exception: " + args.Exception.Message;
            args.Handled = true;
        }
    }
}
