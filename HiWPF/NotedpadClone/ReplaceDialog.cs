using System.Windows;

namespace NotepadClone
{
    class ReplaceDialog : FindReplaceDialog
    {
        public ReplaceDialog(Window owner) : base(owner)
        {
            this.Title = "Replace";
        }
    }
}
