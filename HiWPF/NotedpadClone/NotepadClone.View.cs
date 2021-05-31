using System.Windows;
using System.Windows.Controls;

namespace NotepadClone
{
    partial class NotepadClone
    {
        MenuItem itemStatus;

        void AddViewMenu(Menu menu)
        {
            MenuItem itemView = new MenuItem();
            itemView.Header = "_View";
            itemView.SubmenuOpened += ViewOnOpen;
            menu.Items.Add(itemView);

            itemStatus = new MenuItem();
            itemStatus.Header = "_Status Bar";
            itemStatus.IsCheckable = true;
            itemStatus.Checked += StatusOnCheck;
            itemStatus.Unchecked += StatusOnCheck;
            itemView.Items.Add(itemStatus);
        }

        void  ViewOnOpen(object sender, RoutedEventArgs args)
        {
            itemStatus.IsChecked = (status.Visibility == Visibility.Visible);
        }

        void StatusOnCheck(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;
            status.Visibility = item.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
