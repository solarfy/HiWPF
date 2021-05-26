using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TemplateTreeView
{
    class TypeTreeViewItem : TreeViewItem
    {
        Type typ;

        public Type Type
        {
            set
            {
                this.typ = value;

                if (typ.IsAbstract)
                {
                    this.Header = typ.Name + " (abstract)";
                }
                else
                {
                    this.Header = typ.Name;
                }
            }

            get
            {
                return typ;
            }
        }


        public TypeTreeViewItem()
        {

        }

        public TypeTreeViewItem(Type typ)
        {
            this.Type = typ;
        }
    }
}
