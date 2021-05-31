using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChooseFont;   //添加项目ChooseFont - 为了使用FontDialg

namespace NotepadClone
{
    partial class NotepadClone 
    {
        void AddFormatMenu(Menu menu)
        {
            MenuItem itemFormat = new MenuItem();
            itemFormat.Header = "F_ormat";
            menu.Items.Add(itemFormat);

            WordWrapMenuItem itemWrap = new WordWrapMenuItem();
            itemFormat.Items.Add(itemWrap);

            Binding bind = new Binding();
            bind.Path = new PropertyPath(TextBox.TextWrappingProperty);
            bind.Source = txtbox;
            bind.Mode = BindingMode.TwoWay;
            itemWrap.SetBinding(WordWrapMenuItem.WordWrapProperty, bind);

            MenuItem itemFont = new MenuItem();
            itemFont.Header = "_Font...";
            itemFont.Click += FontOnClick;
            itemFormat.Items.Add(itemFont);
        }

        void FontOnClick(object sender, RoutedEventArgs args)
        {
            FontDialog dlg = new FontDialog();
            dlg.Owner = this;
            dlg.Typeface = new Typeface(txtbox.FontFamily, txtbox.FontStyle, txtbox.FontWeight, txtbox.FontStretch);
            dlg.FaceSize = txtbox.FontSize;

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                txtbox.FontFamily = dlg.Typeface.FontFamily;
                txtbox.FontSize = dlg.FontSize;
                txtbox.FontStyle = dlg.Typeface.Style;
                txtbox.FontWeight = dlg.Typeface.Weight;
                txtbox.FontStretch = dlg.Typeface.Stretch;
            }
        }
    }
}
