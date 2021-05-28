using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Printing;
using System.Collections.Generic;
using System.Globalization;

namespace NotepadClone
{
    class PlainTextDocumentPaginator : DocumentPaginator
    {
        class PrintLine
        {
            public string String;
            public bool Flag;

            public PrintLine(string str, bool flag)
            {
                String = str;
                Flag = flag;
            }
        }

        char[] charsBreak = new char[] { ' ', '-' };
        string txt = "";
        string txtHeader = null;
        Typeface face = new Typeface("");
        double em = 11;
        Size sizePage = new Size(8.5 * 96, 11 * 96);
        Size sizeMax = new Size(0, 0);
        Thickness margins = new Thickness(96);
        PrintTicket prntkt = new PrintTicket();
        TextWrapping txtwrap = TextWrapping.Wrap;

        List<DocumentPage> listPages;

        public string Text
        {
            set => txt = value;
            get => txt;
        }

        public TextWrapping TextWrapping
        {
            set => txtwrap = value;
            get => txtwrap;
        }

        public Thickness Margins
        {
            set => margins = value;
            get => margins;
        }

        public Typeface Typeface
        {
            set => face = value;
            get => face;
        }

        public double FaceSize
        {
            set => em = value;
            get => em;
        }

        public PrintTicket PrintTicket
        {
            set => prntkt = value;
            get => prntkt;
        }

        public string Header
        {
            set => txtHeader = value;
            get => txtHeader;
        }

        public override bool IsPageCountValid
        {
            get
            {
                if (listPages == null)
                    Format();
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                if (listPages == null)
                    return 0;

                return listPages.Count;
            }
        }

        public override Size PageSize
        {
            set => sizePage = value;
            get => sizePage;
        }

        public override IDocumentPaginatorSource Source
        {
            get => null;
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            return listPages[pageNumber];
        }

        void Format()
        {
            List<PrintLine> listLines = new List<PrintLine>();
            FormattedText formtxSample = GetFormattedText("W");

            double width = PageSize.Width - Margins.Left - Margins.Right;

            if (width < formtxSample.Width)
                return;

            string strLine;
            Pen pn = new Pen(Brushes.Black, 2);
            StreamReader reader = new StreamReader(txt);

            while (null != (strLine = reader.ReadLine()))
                ProcessLine(strLine, width, listLines);
            reader.Close();

            double heightLine = formtxSample.LineHeight + formtxSample.Height;
            double height = PageSize.Height - Margins.Top - Margins.Bottom;
            int linesPerPage = (int)(height / heightLine);

            if (linesPerPage < 1)
                return;

            int numPages = (listLines.Count + linesPerPage - 1) / linesPerPage;
            double xStart = Margins.Left;
            double yStart = Margins.Top;

            listPages = new List<DocumentPage>();

            for (int iPage = 0, iLine = 0; iPage < numPages; iPage++)
            {
                DrawingVisual vis = new DrawingVisual();
                DrawingContext dc = vis.RenderOpen();

                //页眉
                if (Header != null && Header.Length > 0)
                {
                    FormattedText formtxt = GetFormattedText(Header);
                    formtxt.SetFontWeight(FontWeights.Bold);
                    Point ptText = new Point(xStart, yStart - 2 * formtxt.Height);
                    dc.DrawText(formtxt, ptText);
                }

                //页脚
                if (numPages > 1)
                {
                    FormattedText formtxt = GetFormattedText("Page " + (iPage + 1) + " of " + numPages);
                    formtxt.SetFontWeight(FontWeights.Bold);
                    Point ptText = new Point((PageSize.Width + Margins.Left - Margins.Right - formtxt.Width) / 2
                        , PageSize.Height - Margins.Bottom + formtxt.Height);
                    dc.DrawText(formtxt, ptText);
                }

                for (int i = 0; i < linesPerPage; i++, iLine++)
                {
                    if (iLine == listLines.Count)
                        break;

                    string str = listLines[iLine].String;
                    FormattedText formtxt = GetFormattedText(str);
                    Point ptText = new Point(xStart, yStart + i * heightLine);
                    dc.DrawText(formtxt, ptText);

                    if (listLines[iLine].Flag)
                    {
                        double x = xStart + width + 6;
                        double y = yStart + i * heightLine + formtxt.Baseline;
                        double len = face.CapsHeight * em;

                        dc.DrawLine(pn, new Point(x, y), new Point(x + len, y - len));
                        dc.DrawLine(pn, new Point(x, y), new Point(x, y - len / 2));
                        dc.DrawLine(pn, new Point(x, y), new Point(x + len / 2, y));
                    }
                }

                dc.Close();
                DocumentPage page = new DocumentPage(vis);
                listPages.Add(page);
            }

            reader.Close();
        }

        void ProcessLine(string str, double width, List<PrintLine> list)
        {
            str = str.TrimEnd(' ');

            if (TextWrapping == TextWrapping.NoWrap)
            {
                do
                {
                    int length = str.Length;

                    while (GetFormattedText(str.Substring(0, length)).Width < width)
                        length--;
                    list.Add(new PrintLine(str.Substring(0, length), length < str.Length));
                    str = str.Substring(length);
                }
                while (str.Length > 0);
            }
            else //TextWrapping.Wrap Or TextWrapping.WrapWithOverflow
            {
                do
                {
                    int length = str.Length;
                    bool flag = false;

                    while (GetFormattedText(str.Substring(0, length)).Width > width)
                    {
                        int index = str.LastIndexOfAny(charsBreak, length - 2);

                        if (length != -1)
                            length = index + 1; //包含尾随空格和破折号
                        else
                        {
                            index = str.IndexOfAny(charsBreak);

                            if (index != -1)
                                length = index + 1;
                            
                            if (TextWrapping == TextWrapping.Wrap)
                            {
                                while (GetFormattedText(str.Substring(0, length)).Width > width)
                                    length--;
                                flag = true;
                            }
                            break;
                        }
                    }
                    list.Add(new PrintLine(str.Substring(0, length), flag));
                    str = str.Substring(length);
                } while (str.Length > 0);
            }
        }

        FormattedText GetFormattedText(string str)
        {
            return new FormattedText(str, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, face, em, Brushes.Black);
        }
    }   
}
