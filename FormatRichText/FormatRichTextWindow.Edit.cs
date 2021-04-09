using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FormatRichText
{
    public partial class FormatRichTextWindow : Window
    {
        void AddEditToolBar(ToolBarTray tray, int band, int index)
        {
            ToolBar toolbar = new ToolBar();
            toolbar.Band = band;
            toolbar.BandIndex = index;
            tray.ToolBars.Add(toolbar);

            RoutedUICommand[] comms =
            {
                ApplicationCommands.Cut, ApplicationCommands.Copy, ApplicationCommands.Paste, ApplicationCommands.Delete, ApplicationCommands.Undo, ApplicationCommands.Redo
            };

            string[] strImages =
            {
                "cut.png", "copy.png", "paste.png", "delete.png", "undo.png", "redo.png"
            };

            for (int i = 0; i < 6; i++)
            {
                if (i == 4)
                {
                    toolbar.Items.Add(new Separator());
                }

                Button btn = new Button();
                btn.Command = comms[i];
                toolbar.Items.Add(btn);

                Image img = new Image();
                img.Source = new BitmapImage(new Uri($"pack://application:,,/Images/{strImages[i]}"));
                img.Stretch = Stretch.None;
                btn.Content = img;

                ToolTip tip = new ToolTip();
                tip.Content = comms[i].Text;
                btn.ToolTip = tip;
            }

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, OnDelete, CanDelete));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo));
        }

        void OnDelete(object sender, ExecutedRoutedEventArgs args)
        {
            this.txtbox.Selection.Text = "";
        }

        void CanDelete(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = !this.txtbox.Selection.IsEmpty;
        }
    }
}
