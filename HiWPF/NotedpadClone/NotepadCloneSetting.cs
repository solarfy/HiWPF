using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace NotepadClone
{
    public class NotepadCloneSetting
    {
        public WindowState WindowState = WindowState.Normal;
        public Rect RestoreBounds = Rect.Empty;
        public TextWrapping TextWrapping = TextWrapping.NoWrap;
        public string FontFamily = string.Empty;
        public string FontStyle = new FontStyleConverter().ConvertToString(FontStyles.Normal);      //FontStyle，FontWeight，FontStretch等是一个结构，没有公开的字段或属性，无法进行序列化与反序列化，只能定义成字符串对象
        public string FontWeight = new FontWeightConverter().ConvertToString(FontWeights.Normal);   //FontStyleConverter，FontWeightConverter，FontStretchConverter等类常常用来解析XAML
        public string FontStretch = new FontStretchConverter().ConvertToString(FontStretches.Normal);
        public double FontSize = 11;        //实际字体1磅 = 96/72 像素
        
        //保存配置至文件（文件全名：路径+文件名）
        public virtual bool Save(string strAppData)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(strAppData));
                StreamWriter write = new StreamWriter(strAppData);
                XmlSerializer xml = new XmlSerializer(this.GetType());  
                xml.Serialize(write, this); //反序列化的类须有无参构造函数，序列化只会序列public的字段和属性，略过只读和只写的成员；如果成员是复杂的数据类型，这些类和结果也必须是public，且需有无参构造函数。
                write.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        //加载配置文件
        public static object Load(Type type, string strAppData)
        {
            StreamReader reader;
            object settings;
            XmlSerializer xml = new XmlSerializer(type);

            try
            {
                reader = new StreamReader(strAppData);
                settings = xml.Deserialize(reader);
                reader.Close();
            }
            catch
            {
                settings = type.GetConstructor(System.Type.EmptyTypes).Invoke(null);    //用反射的方法获取类中空参数的构造函数，Invoke生成该对象的实例
            }

            return settings;
        }

    }
}
