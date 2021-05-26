using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media;

namespace ListColorNames
{
    class NamedColor
    {
        static NamedColor[] nclrs;
        Color clr;
        string str;

        static NamedColor()
        {
            PropertyInfo[] props = typeof(Colors).GetProperties();
            nclrs = new NamedColor[props.Length];

            for (int i = 0; i < props.Length; i++)
            {
                nclrs[i] = new NamedColor(props[i].Name, (Color)props[i].GetValue(null, null));
            }
        }

        private NamedColor(string str, Color clr)
        {
            this.str = str;
            this.clr = clr;
        }

        public static NamedColor[] All
        {
            get { return nclrs; }
        }

        public Color Color
        {
            get { return clr; }
        }

        public string Name
        {
            get 
            {
                string strSpaced = str[0].ToString();
                for (int i = 1; i < str.Length; i++)    //i = 1，忽略第一个大写字符
                {
                    strSpaced += (char.IsUpper(str[i]) ? " " : "") + str[i].ToString();
                }

                return strSpaced;
            }
        }

        public override string ToString()
        {
            return str;
        }
    }
}
