using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;
using System.Windows.Documents;

namespace FormatRichText
{
    public partial class FormatRichTextWindow
    {
        ComboBox comboFamily, comboSize;
        ToggleButton btnBold, btnItalic;
        ColorGridBox clrboxBackground, clrboxForeground;

        void AddCharToolBar(ToolBarTray tray, int band, int index)
        {
            ToolBar toolbar = new ToolBar();
            toolbar.Band = band;
            toolbar.BandIndex = index;
            tray.ToolBars.Add(toolbar);

            comboFamily = new ComboBox();
            comboFamily.Width = 144;
            comboFamily.ItemsSource = Fonts.SystemFontFamilies;     //系统的系列字体
            comboFamily.SelectedItem = this.txtbox.FontFamily;
            comboFamily.SelectionChanged += FamilyComboOnSelection;
            toolbar.Items.Add(comboFamily);

            ToolTip tip = new ToolTip();
            tip.Content = "Font Family";
            comboFamily.ToolTip = tip;

            comboSize = new ComboBox();
            comboSize.Width = 72;
            comboSize.IsEditable = true;
            comboSize.StaysOpenOnEdit = true;            
            comboSize.IsTextSearchEnabled = false;      //关闭文本搜索，防止在输入文本时自动匹配触发SelectionChanged事件
            comboSize.Text = (0.75 * this.txtbox.FontSize).ToString();
            comboSize.ItemsSource = new double[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            comboSize.SelectionChanged += SizeComboOnSelection;
            comboSize.GotKeyboardFocus += SizeComboOnGotFocus;
            comboSize.LostKeyboardFocus += SizeComboOnLostFocus;
            comboSize.PreviewKeyDown += SizeComboOnKeyDowm;
            toolbar.Items.Add(comboSize);

            tip = new ToolTip();
            tip.Content = "Font Size";
            comboSize.ToolTip = tip;

            btnBold = new ToggleButton();
            btnBold.Checked += BoldButtonOnChecked;
            btnBold.Unchecked += BoldButtonOnChecked;
            toolbar.Items.Add(btnBold);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/bold.png"));
            img.Stretch = Stretch.None;
            btnBold.Content = img;

            tip = new ToolTip();
            tip.Content = "Bold";
            btnBold.ToolTip = tip;

            btnItalic = new ToggleButton();
            btnItalic.Checked += ItalicButtonOnChecked;
            btnItalic.Unchecked += ItalicButtonOnChecked;
            toolbar.Items.Add(btnItalic);

            img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/italic.png"));
            img.Stretch = Stretch.None;
            btnItalic.Content = img;

            tip = new ToolTip();
            tip.Content = "Italic";
            btnItalic.ToolTip = tip;

            toolbar.Items.Add(new Separator());

            Menu menu = new Menu();
            toolbar.Items.Add(menu);

            MenuItem item = new MenuItem();
            menu.Items.Add(item);

            img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/color_back.png"));
            img.Stretch = Stretch.None;
            item.Header = img;

            clrboxBackground = new ColorGridBox();
            clrboxBackground.SelectionChanged += BackgroundOnSelectionChanged;
            item.Items.Add(clrboxBackground);

            tip = new ToolTip();
            tip.Content = "Background Color";
            item.ToolTip = tip;

            item = new MenuItem();
            menu.Items.Add(item);

            img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/color_fore.png"));
            img.Stretch = Stretch.None;
            item.Header = img;

            clrboxForeground = new ColorGridBox();
            clrboxForeground.SelectionChanged += ForegroundOnSelectionChanged;
            item.Items.Add(clrboxForeground);

            tip = new ToolTip();
            tip.Content = "Foreground Color";
            item.ToolTip = tip;

            this.txtbox.SelectionChanged += TextBoxOnSelectionChanged;      //选择文字时，更新工具栏信息
        }

        void TextBoxOnSelectionChanged(object sender, RoutedEventArgs args)
        {
            object obj = this.txtbox.Selection.GetPropertyValue(FlowDocument.FontFamilyProperty);
            if (obj is FontFamily)
            {
                comboFamily.SelectedItem = (FontFamily)obj;
            }
            else
            {
                comboFamily.SelectedIndex = -1;
            }

            obj = this.txtbox.Selection.GetPropertyValue(FlowDocument.FontSizeProperty);
            if (obj is double)
            {
                comboSize.Text = (0.75 * (double)obj).ToString();
            }
            else
            {
                comboSize.SelectedIndex = -1;
            }

            obj = this.txtbox.Selection.GetPropertyValue(FlowDocument.FontWeightProperty);
            if (obj is FontWeight)
            {
                btnBold.IsChecked = (FontWeight)obj == FontWeights.Bold;
            }

            obj = this.txtbox.Selection.GetPropertyValue(FlowDocument.FontStyleProperty);
            if (obj is FontStyle)
            {
                btnItalic.IsChecked = (FontStyle)obj == FontStyles.Italic;
            }

            obj = this.txtbox.Selection.GetPropertyValue(FlowDocument.BackgroundProperty);
            if (obj != null && obj is Brush)
            {
                clrboxBackground.SelectedValue = (Brush)obj;
            }

            obj = this.txtbox.Selection.GetPropertyValue(FlowDocument.ForegroundProperty);
            if (obj != null && obj is Brush)
            {
                clrboxForeground.SelectedValue = (Brush)obj;
            }
        }

        void FamilyComboOnSelection(object sender, SelectionChangedEventArgs args)
        {
            ComboBox combo = args.Source as ComboBox;
            FontFamily family = combo.SelectedItem as FontFamily;

            if (family != null)
            {
                this.txtbox.Selection.ApplyPropertyValue(FlowDocument.FontFamilyProperty, family);
            }

            this.txtbox.Focus();
        }

        string strOriginal;
        void SizeComboOnGotFocus(object sender, KeyboardFocusChangedEventArgs args)
        {            
            strOriginal = (sender as ComboBox).Text;            
        }
        void SizeComboOnLostFocus(object sender, KeyboardFocusChangedEventArgs args)
        {
            double size;

            if (Double.TryParse((sender as ComboBox).Text, out size))
            {
                this.txtbox.Selection.ApplyPropertyValue(FlowDocument.FontSizeProperty, size / 0.75);
            }
            else
            {
                (sender as ComboBox).Text = strOriginal;
            }
        }

        void SizeComboOnKeyDowm(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Escape)
            {
                (sender as ComboBox).Text = strOriginal;
                args.Handled = true;
                this.txtbox.Focus();
            }
            else if (args.Key == Key.Enter)
            {
                args.Handled = true;
                this.txtbox.Focus();
            }
        }
        void SizeComboOnSelection(object sender, SelectionChangedEventArgs args)
        {
            ComboBox combo = args.Source as ComboBox;

            /*  设置 combo.IsTextSearchEnabled = true;
             *  当输入的combo.Text与combo.ItemSourec中的某个字符匹配时，此时combo.SelectedIndex != -1                      
             */

            if (combo.SelectedIndex != -1)      
            {
                double size = (double)combo.SelectedValue;
                this.txtbox.Selection.ApplyPropertyValue(FlowDocument.FontSizeProperty, size / 0.75);
                //this.txtbox.Focus();      //此处不需进行焦点切换，使其能够连续的进行选择操作
            }
        }

        void BoldButtonOnChecked(object sender, RoutedEventArgs args)
        {
            ToggleButton btn = args.Source as ToggleButton;

            this.txtbox.Selection.ApplyPropertyValue(FlowDocument.FontWeightProperty,
                (bool)btn.IsChecked ? FontWeights.Bold : FontWeights.Normal);
        }

        void ItalicButtonOnChecked(object sender, RoutedEventArgs args)
        {
            ToggleButton btn = args.Source as ToggleButton;

            this.txtbox.Selection.ApplyPropertyValue(FlowDocument.FontStyleProperty,
                (bool)btn.IsChecked ? FontStyles.Italic : FontStyles.Normal);
        }

        void BackgroundOnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ColorGridBox clrbox = args.Source as ColorGridBox;

            this.txtbox.Selection.ApplyPropertyValue(FlowDocument.BackgroundProperty, clrbox.SelectedValue);
        }

        void ForegroundOnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ColorGridBox clrbox = args.Source as ColorGridBox;

            this.txtbox.Selection.ApplyPropertyValue(FlowDocument.ForegroundProperty, clrbox.SelectedValue);
        }
    }
}
