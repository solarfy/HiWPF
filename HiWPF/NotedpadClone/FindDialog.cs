using System.Windows;

namespace NotepadClone
{
    class FindDialog : FindReplaceDialog
    {
        public FindDialog(Window owner) : base(owner)
        {
            this.Title = "Find";

            //隐藏不需要的控件
            lblReplace.Visibility = Visibility.Collapsed;
            txtboxReplace.Visibility = Visibility.Collapsed;
            btnReplace.Visibility = Visibility.Collapsed;
            btnAll.Visibility = Visibility.Collapsed;
        }
    }
}
