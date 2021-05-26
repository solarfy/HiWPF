using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace FormatRichText
{
    public partial class FormatRichTextWindow : Window
    {
        StatusBarItem itemDateTime;

        void AddStatusBar(DockPanel dock)
        {
            StatusBar status = new StatusBar();
            dock.Children.Add(status);
            DockPanel.SetDock(status, Dock.Bottom);

            itemDateTime = new StatusBarItem();
            itemDateTime.HorizontalAlignment = HorizontalAlignment.Right;
            status.Items.Add(itemDateTime);

            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromSeconds(1);
            tmr.Tick += TimerOnTick;
            tmr.Start();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            itemDateTime.Content = dt.ToLongDateString() + " " + dt.ToLongTimeString();
        }
    }
}
