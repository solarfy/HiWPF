using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Reflection;

namespace ListColorNames
{
    class NamedBrush
    {
        static NamedBrush[] nbrushes;
        Brush brush;
        string str;

        static NamedBrush()
        {
            PropertyInfo[] props = typeof(Brushes).GetProperties();
            nbrushes = new NamedBrush[props.Length];

            for (int i = 0; i < props.Length; i++)
            {
                nbrushes[i] = new NamedBrush(props[i].Name, (Brush)props[i].GetValue(null, null));
            }
        }

        private NamedBrush(string str, Brush brush)
        {
            this.str = str;
            this.brush = brush;
        }

        public static NamedBrush[] All
        {
            get => nbrushes;
        }

        public Brush Brush
        {
            get => brush;
        }

        public string Name
        {
            get 
            {
                string strSpace = str[0].ToString();
                for (int index = 1; index < str.Length; index++)        //从第二个字符开始
                {
                    strSpace += (char.IsUpper(str[index]) ? " " : "") + str[index].ToString();
                }
                return strSpace;
            }
        }

        public override string ToString()
        {
            return str;
        }
    }
}
