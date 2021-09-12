using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace TextGeometryDemo
{
    public class TextGeometry
    {
        string txt = "";
        FontFamily fntfam = new FontFamily();
        FontStyle fntstyle = FontStyles.Normal;
        FontWeight fntwt = FontWeights.Normal;
        FontStretch fntstr = FontStretches.Normal;
        double emsize = 24;
        Point ptOrigin = new Point(0, 0);

        public string Text
        {
            set { txt = value; }
            get { return txt; }
        }

        public FontFamily FontFamily
        {
            set { fntfam = value; }
            get { return fntfam; }
        }
        
        public FontStyle FontStyle
        {
            set { fntstyle = value; }
            get { return fntstyle; }
        }

        public FontWeight FontWeight
        {
            set { fntwt = value; }
            get { return fntwt; }
        }

        public FontStretch FontStretch
        {
            set { fntstr = value; }
            get { return fntstr; }
        }

        public double FontSize
        {
            set { emsize = value; }
            get { return emsize; }
        }

        public Point Origin
        {
            set { ptOrigin = value; }
            get { return ptOrigin; }
        }

        public Geometry Geometry
        {
            get
            {
                FormattedText formtxt = new FormattedText(Text
                    , CultureInfo.CurrentCulture
                    , FlowDirection.LeftToRight
                    , new Typeface(FontFamily, FontStyle, FontWeight, FontStretch)
                    , FontSize
                    , Brushes.Black);

                return formtxt.BuildGeometry(Origin);
            }
        }

        public PathGeometry PathGeometry
        {
            get
            {
                return PathGeometry.CreateFromGeometry(Geometry);
            }
        }
    }
}
