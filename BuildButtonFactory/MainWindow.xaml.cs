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

namespace BuildButtonFactory
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitBuildButtonFactory();
        }

        void InitBuildButtonFactory()
        {
            this.Title = "Build Button Factory";

            //建立一个ControlTemplate
            ControlTemplate template = new ControlTemplate();

            //建立一个FrameworkElementFactory
            FrameworkElementFactory factoryBorder = new FrameworkElementFactory(typeof(Border));

            //取名字，以便以后使用
            factoryBorder.Name = "Border";

            //设定某些默认property
            factoryBorder.SetValue(Border.BorderBrushProperty, Brushes.Red);
            factoryBorder.SetValue(Border.BorderThicknessProperty, new Thickness(3));
            factoryBorder.SetValue(Border.BackgroundProperty, SystemColors.ControlLightBrush);

            //为ContentPresenter建立FrameworkElementFactory
            FrameworkElementFactory factoryContent = new FrameworkElementFactory(typeof(ContentPresenter));

            //取名字，以便以后使用
            factoryContent.Name = "Content";

            //将某些ContentPresenter property绑定到Button property
            factoryContent.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(Button.ContentProperty));

            //注意，按钮的Padding是内容的Margin
            factoryContent.SetValue(ContentPresenter.MarginProperty, new TemplateBindingExtension(Button.PaddingProperty));

            //让ContentPresenter作为Border的孩子
            factoryBorder.AppendChild(factoryContent);

            //让Boder成为视觉树的root element
            template.VisualTree = factoryBorder;

            //当IsMouseOver为True时，定义一个新的Trigger
            Trigger trig = new Trigger();
            trig.Property = UIElement.IsMouseOverProperty;
            trig.Value = true;

            //将一个Setter和Trigger关联起来，以便改变“border”element的CornerRadius property
            Setter set = new Setter();
            set.Property = Border.CornerRadiusProperty;
            set.Value = new CornerRadius(24);
            set.TargetName = "Border";

            //将此Setter加入Trigger的Setters collection中
            trig.Setters.Add(set);

            //类似地，定义一个Setter，以便改变FongStyle（不需要TargetName，因为这是按钮的Property）
            set = new Setter();
            set.Property = Control.FontStyleProperty;
            set.Value = FontStyles.Italic;

            //把它加到trigger的Setters collection
            trig.Setters.Add(set);

            //将Trigger加到template
            template.Triggers.Add(trig);

            //类似地，为IsPressed定义一个Trigger
            trig = new Trigger();
            trig.Property = Button.IsPressedProperty;
            trig.Value = true;

            set = new Setter();
            set.Property = Border.BackgroundProperty;
            set.Value = SystemColors.ControlDarkBrush;
            set.TargetName = "Border";

            //将此Setter加入trigger的Setters collection
            trig.Setters.Add(set);

            //将此Trigger加入到Template
            template.Triggers.Add(trig);

            //最后建立一个Button
            Button btn = new Button();

            //让此Button具有此template
            btn.Template = template;

            //用正常的方式，定义其他的property
            btn.Content = "Button with Custom Template";
            btn.Padding = new Thickness(20);
            btn.FontSize = 48;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.Click += Btn_Click;

            this.Content = btn;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You clicked the button", this.Title);
        }
    }
}
