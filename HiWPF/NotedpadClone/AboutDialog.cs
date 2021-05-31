using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Documents;
using System.Diagnostics;

namespace NotepadClone
{
    class AboutDialog : Window
    {
        public AboutDialog(Window owner)
        {
            Assembly asmbly = Assembly.GetExecutingAssembly();

            AssemblyTitleAttribute title = (AssemblyTitleAttribute)asmbly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
            string strTitle = title.Title;

            AssemblyFileVersionAttribute version = (AssemblyFileVersionAttribute)asmbly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];
            string strVersion = version.Version.Substring(0, 3);

            AssemblyCopyrightAttribute copy = (AssemblyCopyrightAttribute)asmbly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
            string strCopyright = copy.Copyright;

            this.Title = $"About {strTitle}";
            this.ShowInTaskbar = false;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.Left = owner.Left + 96;
            this.Top = owner.Top + 96;

            StackPanel stackMain = new StackPanel();
            this.Content = stackMain;

            TextBlock txtblk = new TextBlock();
            txtblk.Text = $"{strTitle} Version {strVersion}";
            txtblk.FontFamily = new FontFamily("Times New Roman");
            txtblk.FontStyle = FontStyles.Italic;
            txtblk.Margin = new Thickness(24);
            txtblk.HorizontalAlignment = HorizontalAlignment.Center;
            stackMain.Children.Add(txtblk);

            txtblk = new TextBlock();
            txtblk.Text = strCopyright;
            txtblk.FontSize = 20;  //15 Points
            txtblk.HorizontalAlignment = HorizontalAlignment.Center;
            stackMain.Children.Add(txtblk);

            Run run = new Run("www.charlespetzold.com");
            Hyperlink link = new Hyperlink(run);
            link.Click += LinkOnClick;
            txtblk = new TextBlock(link);
            txtblk.FontSize = 20;
            txtblk.HorizontalAlignment = HorizontalAlignment.Center;
            stackMain.Children.Add(txtblk);

            Button btn = new Button();
            btn.Content = "OK";
            btn.IsDefault = true;   //Enter
            btn.IsCancel = true;    //Escape
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Margin = new Thickness(24);
            btn.Click += OkOnClick;
            stackMain.Children.Add(btn);
            btn.Focus();
        }

        void LinkOnClick(object sender, RoutedEventArgs args)
        {
            Process.Start("http://www.charlespetzold.com");
        }

        void OkOnClick(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
        }
    }
}
