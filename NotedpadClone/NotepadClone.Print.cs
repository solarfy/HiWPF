using System.Windows;
using System.Printing;  //添加引用System.Printing.dll
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PrintWithMargins; //添加Print->PrintWithMargins的引用，为了使用PageMarginsDialog类

namespace NotepadClone
{
    public partial class NotepadClone : Window
    {
        PrintQueue prnqueue;
        PrintTicket prntkt;     //添加引用ReachFramwork.dll
        Thickness marginPage = new Thickness(96);

        void AddPrintMenuItems(MenuItem itemFile)
        {
            //Setup
            MenuItem itemSetup = new MenuItem();
            itemSetup.Header = "Page Set_up...";
            itemSetup.Click += PageSetupOnClick;
            itemFile.Items.Add(itemSetup);

            //Print
            MenuItem itemPrint = new MenuItem();
            itemPrint.Header = "_Print...";
            itemPrint.Command = ApplicationCommands.Print;
            itemFile.Items.Add(itemPrint);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, PrintOnExecuted));
        }

        void PageSetupOnClick(object sender, RoutedEventArgs args)
        {
            PageMarginsDialog dlg = new PageMarginsDialog();
            dlg.Owner = this;
            dlg.PagesMargins = marginPage;
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                marginPage = dlg.PagesMargins;
            }
        }

        void PrintOnExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            PrintDialog dlg = new PrintDialog();

            if (prnqueue != null)
                dlg.PrintQueue = prnqueue;
            if (prntkt != null)
                prntkt = dlg.PrintTicket;

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                prnqueue = dlg.PrintQueue;
                prntkt = dlg.PrintTicket;

                PlainTextDocumentPaginator paginator = new PlainTextDocumentPaginator();

                paginator.PrintTicker = prntkt;
                paginator.Text = txtbox.Text;
                paginator.Header = strLoadedFile;
                paginator.Typeface = new Typeface(txtbox.FontFamily, txtbox.FontStyle, txtbox.FontWeight, txtbox.FontStretch);
                paginator.FaceSize = txtbox.FontSize;
                paginator.TextWrapping = txtbox.TextWrapping;
                paginator.Margins = marginPage;
                paginator.PageSize = new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight);

                dlg.PrintDocument(paginator, this.Title);

            }
        }
    }
}
