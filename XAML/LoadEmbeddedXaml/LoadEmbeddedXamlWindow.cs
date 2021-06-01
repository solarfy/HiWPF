using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace LoadEmbeddedXaml
{
    class LoadEmbeddedXamlWindow : Window
    {        
        public LoadEmbeddedXamlWindow()
        {
            this.Title = "Load Embedded Xaml";

            string strXaml = "<Button xmlns = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation' Foreground = 'LightSeaGreen' FontSize='24pt'> Click me! </Button>";

            StringReader strreader = new StringReader(strXaml);
            XmlTextReader xmlreader = new XmlTextReader(strreader);
            object obj = XamlReader.Load(xmlreader);    //Load需要一个Stream对象或者XmlReader对象

            //另一种写法
            //MemoryStream memory = new MemoryStream(strXaml.Length);
            //StreamWriter writer = new StreamWriter(memory);
            //writer.Write(strXaml);
            //writer.Flush();
            //memory.Seek(0, SeekOrigin.Begin);   //重新指向文件流位置
            //obj = XamlReader.Load(memory);

            this.Content = obj;
        }
    }
}
