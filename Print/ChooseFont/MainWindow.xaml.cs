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

namespace ChooseFont
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            this.Title = "Choose Font";
            Button btn = new Button();
            btn.Content = Title;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.Click += ButtonOnClick;
            this.Content = btn;
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            FontDialog dlg = new FontDialog();
            dlg.Owner = this;

            dlg.Typeface = new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch);
            dlg.FaceSize = this.FontSize;

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                this.FontFamily = dlg.Typeface.FontFamily;
                this.FontStyle = dlg.Typeface.Style;
                this.FontWeight = dlg.Typeface.Weight;
                this.FontStretch = dlg.Typeface.Stretch;
                this.FontSize = dlg.FaceSize;
            }
        }
    }
}
