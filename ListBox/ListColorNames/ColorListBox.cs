using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Media;

namespace ListColorNames
{
    class ColorListBox : ListBox
    {
        public ColorListBox()
        {
            PropertyInfo[] props = typeof(Colors).GetProperties();
            
            foreach (PropertyInfo prop in props)
            {
                ColorListBoxItem item = new ColorListBoxItem();
                item.Text = prop.Name;
                item.Color = (Color)prop.GetValue(null, null);
                this.Items.Add(item);
            }

            this.SelectedValuePath = nameof(Color);
        }

        public Color SelectedColor
        {
            set => this.SelectedValue = value;
            get => (null != this.SelectedValue) ? (Color)this.SelectedValue : Colors.Transparent;
        }
    }
}
