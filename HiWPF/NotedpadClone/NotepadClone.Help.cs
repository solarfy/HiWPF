using System.Windows;
using System.Windows.Controls;

namespace NotepadClone
{
    partial class NotepadClone
    {
        void AddHelpMenu(Menu menu)
        {
            MenuItem itemHelp = new MenuItem();
            itemHelp.Header = "_Help";
            itemHelp.SubmenuOpened += ViewOnOpen;
            menu.Items.Add(itemHelp);

            MenuItem itemAbout = new MenuItem();
            itemAbout.Header = "_About " + strAppTitle + "...";
            itemAbout.Click += AboutOnClick;
            itemHelp.Items.Add(itemAbout);
        }

        void AboutOnClick(object sender, RoutedEventArgs args)
        {
            AboutDialog dlg = new AboutDialog(this);
            dlg.ShowDialog();
        }
    }
}
