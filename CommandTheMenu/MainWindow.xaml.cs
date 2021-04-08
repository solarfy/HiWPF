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

namespace CommandTheMenu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock text;

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }        

        private void Init()
        {
            this.Title = "Command the Menu";

            DockPanel dock = new DockPanel();
            this.Content = dock;

            Menu menu = new Menu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);

            text = new TextBlock();
            text.Text = "Sample clipboard text";
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.FontSize = 32;
            text.TextWrapping = TextWrapping.Wrap;
            dock.Children.Add(text);

            MenuItem itemEdit = new MenuItem();
            itemEdit.Header = "_Edit";
            menu.Items.Add(itemEdit);

            MenuItem itemCut = new MenuItem();
            //itemCut.Header = "Cu_t";      //如果没有设置Header属性，它会默认使用RoutedUICommand的Text的值，此处为‘剪切’,但没有前面的下划线，并且会将Ctrl+X文字加入菜单项中
            itemCut.Command = ApplicationCommands.Cut;
            itemEdit.Items.Add(itemCut);

            MenuItem itemCopy = new MenuItem();
            itemCopy.Header = "_Copy";
            itemCopy.Command = ApplicationCommands.Copy;
            itemEdit.Items.Add(itemCopy);

            MenuItem itemPaste = new MenuItem();
            itemPaste.Header = "_Paste";
            itemPaste.Command = ApplicationCommands.Paste;
            itemEdit.Items.Add(itemPaste);

            MenuItem itemDelete = new MenuItem();
            itemDelete.Header = "_Delete";
            itemDelete.Command = ApplicationCommands.Delete;
            itemEdit.Items.Add(itemDelete);

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, CutOnExecute, CutCopyDeletCanExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyOnExecute, CutCopyDeletCanExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteOnExecute, PasteCanExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteOnExecute, CutCopyDeletCanExecute));

            /* 定义一个RoutedUICommand命令 */
            //由于一个RoutedUICommand可以和多个键盘手势关联，即使只想要一个手势，仍需定义一个collection集合
            InputGestureCollection collGestures = new InputGestureCollection();
            collGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            //创建一个Restore命令            
            RoutedUICommand commRestore = new RoutedUICommand("_Restore", "Restore", GetType(), collGestures);

            MenuItem itemRestore = new MenuItem();  //无需设置Header属性，可以从RoutedUICommand中取出Text属性
            itemRestore.Command = commRestore;
            itemEdit.Items.Add(itemRestore);

            //将命令加入到窗口的命令集合中
            this.CommandBindings.Add(new CommandBinding(commRestore, RestoreOnExecute));
        }

        void CutCopyDeletCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = !string.IsNullOrEmpty(text.Text);
        }        
        void PasteCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = Clipboard.ContainsText();
        }

        void CutOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            ApplicationCommands.Copy.Execute(null, this);
            ApplicationCommands.Delete.Execute(null, this);
        }
        void CopyOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            Clipboard.SetText(text.Text);
        }
        void PasteOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            if (Clipboard.ContainsText())
            {
                text.Text += Clipboard.GetText();
            }
        }
        void DeleteOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            text.Text = null;
        }

        void RestoreOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            text.Text = "Sample clipboard text";
        }
    }
}
