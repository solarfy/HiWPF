using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotepadClone
{
    partial class NotepadClone 
    {
        string strFindWhat = "", strReplaceWith = "";
        StringComparison strcomp = StringComparison.OrdinalIgnoreCase;
        Direction dirFind = Direction.Down;

        void AddFindMenuItems(MenuItem itemEdit)
        {
            MenuItem itemFind = new MenuItem();
            itemFind.Header = "_Find...";
            itemFind.Command = ApplicationCommands.Find;
            itemEdit.Items.Add(itemFind);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, FindOnExecute, FindCanExecute));

            InputGestureCollection coll = new InputGestureCollection();
            coll.Add(new KeyGesture(Key.F3));
            RoutedUICommand commFindNext = new RoutedUICommand("Find _Next", "FindNext", GetType(), coll);

            MenuItem itemNext = new MenuItem();
            itemNext.Command = commFindNext;
            itemEdit.Items.Add(itemNext);
            this.CommandBindings.Add(new CommandBinding(commFindNext, FindNextOnExecute, FindNextCanExecute));

            MenuItem itemReplace = new MenuItem();
            itemReplace.Header = "_Replace...";
            itemReplace.Command = ApplicationCommands.Replace;
            itemEdit.Items.Add(itemReplace);
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Replace, ReplaceOnExecute, FindCanExecute));
        }

        void FindCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = (txtbox.Text.Length > 0 && this.OwnedWindows.Count == 0);
        }

        void FindNextCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = (txtbox.Text.Length > 0 && strFindWhat.Length > 0);
        }

        void FindOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            FindDialog dlg = new FindDialog(this);
            dlg.FindWhat = strFindWhat;
            dlg.MatchCase = strcomp == StringComparison.Ordinal;
            dlg.Direction = dirFind;

            dlg.FindNext += FindDialogOnFindNext;
            dlg.Show();
        }

        void FindNextOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            if (strFindWhat == null || strFindWhat.Length == 0)
                FindOnExecute(sender, args);
            else
                FindNext();
        }

        void ReplaceOnExecute(object sender, ExecutedRoutedEventArgs args)
        {
            ReplaceDialog dlg = new ReplaceDialog(this);
            dlg.FindWhat = strFindWhat;
            dlg.ReplaceWith = strReplaceWith;
            dlg.MatchCase = strcomp == StringComparison.Ordinal;
            dlg.Direction = dirFind;

            dlg.FindNext += FindDialogOnFindNext;
            dlg.Replace += ReplaceDialogOnReplace;
            dlg.ReplaceAll += ReplaceDialogOnReplaceAll;                            

            dlg.Show();
        }

        void FindDialogOnFindNext(object sender, EventArgs args)
        {
            FindReplaceDialog dlg = sender as FindReplaceDialog;

            strFindWhat = dlg.FindWhat;
            strcomp = dlg.MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            dirFind = dlg.Direction;

            FindNext();
        }

        void ReplaceDialogOnReplace(object sender, EventArgs args)
        {
            ReplaceDialog dlg = sender as ReplaceDialog;

            strFindWhat = dlg.FindWhat;
            strReplaceWith = dlg.ReplaceWith;
            strcomp = dlg.MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            if (strFindWhat.Equals(txtbox.SelectedText, strcomp))
                txtbox.SelectedText = strReplaceWith;

            FindNext();            
        }

        void ReplaceDialogOnReplaceAll(object sender, EventArgs args)
        {
            ReplaceDialog dlg = sender as ReplaceDialog;
            string str = txtbox.Text;
            strFindWhat = dlg.FindWhat;
            strReplaceWith = dlg.ReplaceWith;
            strcomp = dlg.MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int index = 0;
            while (index + strFindWhat.Length < str.Length)
            {
                index = str.IndexOf(strFindWhat, index, strcomp);

                if (index != -1)
                {
                    str = str.Remove(index, strFindWhat.Length);
                    str = str.Insert(index, strReplaceWith);
                    index += strReplaceWith.Length;
                }
                else
                    break;
            }
            txtbox.Text = str;
        }

        void FindNext()
        {
            int indexStart, indexFind;

            if (dirFind == Direction.Down)
            {
                indexStart = txtbox.SelectionStart + txtbox.SelectionLength;
                indexFind = txtbox.Text.IndexOf(strFindWhat, indexStart, strcomp);
            }
            else
            {
                indexStart = txtbox.SelectionStart;
                indexFind = txtbox.Text.LastIndexOf(strFindWhat, indexStart, strcomp);  //LastIndexOf
            }

            if (indexFind != -1)
            {
                txtbox.Select(indexFind, strFindWhat.Length);
                txtbox.Focus();
            }
            else
                MessageBox.Show("Cannot find \"" + strFindWhat + "\"", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
