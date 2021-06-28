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
using System.Windows.Markup;
using System.Xml;
using System.Reflection;

namespace DumpControlTemplate
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Control ctrl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ControlItemOnClick(object sender, RoutedEventArgs e)
        {
            //移除Grid中已存在第一行的展示元素
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (Grid.GetRow(this.grid.Children[i]) == 0)
                {
                    this.grid.Children.Remove(this.grid.Children[i]);
                    break;
                }
            }

            this.txtbox.Text = string.Empty;

            MenuItem item = e.Source as MenuItem;
            Type typ = (Type)item.Tag;

            ConstructorInfo info = typ.GetConstructor(System.Type.EmptyTypes);  //用反射方式获取该类型的默认构造函数

            try
            {
                ctrl = (Control)info.Invoke(null);  //用构造函数创建该对象
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, this.Title);
                return;
            }

            try
            {
                this.grid.Children.Add(ctrl);   //将构造出的对象加入Grid首行展示区
            }
            catch
            {
                if (ctrl is Window)     //Window类不能作为子元素
                    (ctrl as Window).Show();
                else
                    return;
            }

            this.Title = this.Title.Remove(this.Title.IndexOf('-')) + "-" + typ.Name;
        }

        private void DumpOnOpened(object sender, RoutedEventArgs e)
        {
            this.itemTemplate.IsEnabled = ctrl != null;
            this.itemItmesPanel.IsEnabled = ctrl != null && ctrl is ItemsControl;
        }

        private void DumpTemplateOnClick(object sender, RoutedEventArgs e)
        {
            if (ctrl != null)
                Dump(ctrl.Template);
        }

        private void DumpItemPanelOnClick(object sender, RoutedEventArgs e)
        {
            if (ctrl != null && ctrl is ItemsControl)
                Dump((ctrl as ItemsControl).ItemsPanel);
        }

        void Dump(FrameworkTemplate template)
        {
            if (template != null)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = new string(' ', 4);  //缩进字符
                settings.NewLineOnAttributes = true;

                StringBuilder strBuild = new StringBuilder();
                XmlWriter xmlwriter = XmlWriter.Create(strBuild, settings);

                try
                {
                    XamlWriter.Save(template, xmlwriter);
                    this.txtbox.Text = strBuild.ToString();
                }
                catch (Exception exc)
                {
                    this.txtbox.Text = exc.Message;
                }
            }
            else
            {
                this.txtbox.Text = "no template";
            }
        }
    }
}
