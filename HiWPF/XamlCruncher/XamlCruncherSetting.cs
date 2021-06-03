using NotepadClone; //添加项目引用NotepadClone
using System.Windows.Controls;

namespace XamlCruncher
{
    public class XamlCruncherSetting : NotepadCloneSetting
    {
        public Dock Orientation = Dock.Left;
        public int TabSpaces = 4;
        public string StartupDocument = "<Button xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n"
                                              + "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">\r\n"
                                              + "Hello, XAML!\r\n"
                                        + "</Button>\r\n";

        public XamlCruncherSetting()
        {
            FontFamily = "Lucida Console";
            FontSize = 10 / 0.75;
        }
    }
}
