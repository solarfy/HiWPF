using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;    //for ContentPropertyAttribute

namespace DumpContentPropertyAttributes
{
    class Program
    {
        /* 特别属性Content；特征ContentProperty 
         * 
         * [ContentProperty("Content")]
         * Public class Button : ButtonBase { ... }
         * 
         * [ContentProperty("Chidren")]
         * Public class StackPanel : Panel { ... }
         * 
         * LinerGradientBrush与RadialGradientBrush的Content属性是GradientStops
         * TexBlock的Content是Inlines集合；Run派生自Inline，Run的Content是Text，Text的类型是string；Italic和Bold都是从Span继承到Text，所以有如下的写法          
         * <TextBlock>
         *   This is <Italic Text="italic"/> text and this <Bold Text="Bold"/> text. 
         * </TextBlock>
         */

        [STAThread]
        public static void Main()
        {
            //确保已加载PresentationCore和PresentationFramework程序集
            UIElement dummy1 = new UIElement();
            FrameworkElement dummy2 = new FrameworkElement();

            SortedList<string, string> listClass = new SortedList<string, string>();

            string strFormat = "{0, -35}{1}";   //复合格式字符串

            foreach (AssemblyName asmblyname in Assembly.GetExecutingAssembly().GetReferencedAssemblies())  //获取此程序集引用的所有程序集对象
            {
                foreach (Type type in Assembly.Load(asmblyname).GetTypes()) //获取该程序集中所有的类型
                {
                    foreach (object obj in type.GetCustomAttributes(typeof(ContentPropertyAttribute), true))    //获取特征为ContentPropertyAttribute的属性
                    {
                        if (type.IsPublic && obj is ContentPropertyAttribute cpat)
                        {
                            listClass.Add(type.Name, cpat.Name);
                        }
                    }
                }
                
            }

            Console.WriteLine(strFormat, "Class", "Content Property");
            Console.WriteLine(strFormat, "-----", "----------------");

            foreach (string strClass in listClass.Keys)
                Console.WriteLine(strFormat, strClass, listClass[strClass]);

            Console.Read();
        }
    }
}
