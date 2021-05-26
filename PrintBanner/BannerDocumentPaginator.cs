using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Globalization;

namespace PrintBanner
{
    class BannerDocumentPaginator : DocumentPaginator       //分页器
    {
        private string txt = "";
        private Typeface face = new Typeface("");
        private Size sizePage;
        private Size sizeMax = new Size(0, 0);

        public string Text
        {
            set => txt = value;
            get => txt;
        }

        public Typeface Typeface
        {
            set => face = value;
            get => face;
        }

        private FormattedText GetFormattedText(char ch, Typeface face, double em)
        {
            return new FormattedText(ch.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, face, em, Brushes.Black);
        }

        public override bool IsPageCountValid
        {
            get 
            {
                //决定“100个em大小”的字母最大尺寸
                foreach (char ch in txt)
                {
                    FormattedText formtxt = GetFormattedText(ch, face, 100);
                    sizeMax.Width = Math.Max(sizeMax.Width, formtxt.Width);
                    sizeMax.Height = Math.Max(sizeMax.Height, formtxt.Height);
                }

                return true;
            }            
        }

        public override int PageCount
        {
            get { return txt == null ? 0 : txt.Length; }
        }

        public override Size PageSize 
        { 
            get => sizePage; 
            set => sizePage = value; 
        }

        public override IDocumentPaginatorSource Source
        {
            get => null;
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            //当计算em尺寸因子时，假设有半英寸的边界
            double factor = Math.Min((PageSize.Width - 96) / sizeMax.Width, (PageSize.Height - 96) / sizeMax.Height);
            FormattedText formtxt = GetFormattedText(txt[pageNumber], face, factor * 100);

            //找到可以让字符放置在页面中心的点
            Point ptText = new Point((PageSize.Width - formtxt.Width) / 2, (PageSize.Height - formtxt.Height) / 2);
            dc.DrawText(formtxt, ptText);
            dc.Close();

            return new DocumentPage(vis);
        }
    }
}
