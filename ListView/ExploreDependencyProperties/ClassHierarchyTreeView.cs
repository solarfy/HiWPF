using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;

namespace ExploreDependencyProperties
{
    class ClassHierarchyTreeView : TreeView
    {
        public ClassHierarchyTreeView(Type typeRoot)
        {
            UIElement dummy = new UIElement();

            List<Assembly> assemblies = new List<Assembly>();

            AssemblyName[] anames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();  //获取正在执行的程序集-获取引用的程序集

            foreach (AssemblyName aname in anames)
            {
                assemblies.Add(Assembly.Load(aname));   //加载其给定名称的程序集合
            }

            //SortedList 表示一系列按照键来排序的键值对，这些键值可以通过键和索引来访问；
            //SortedList 表示数组和哈希表的组合，它包含一个可以使用的键或索引访问各项的列表。如果使用索引访问各项，则它是一个动态数组；如果使用键访问各项，则它是一个哈希表。
            SortedList<string, Type> classes = new SortedList<string, Type>();
            classes.Add(typeRoot.Name, typeRoot);   //加入根类信息

            //获取程序集中指定的类
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type typ in assembly.GetTypes())
                {
                    if (typ.IsPublic && typ.IsSubclassOf(typeRoot))     //添加Public类并且是子类的类
                    {
                        classes.Add(typ.Name, typ);
                    }
                }
            }

            //先创建根节点 
            TypeTreeViewItem item = new TypeTreeViewItem(typeRoot);
            this.Items.Add(item);

            //递归填充
            CreateLinkedItems(item, classes);
        }

        void CreateLinkedItems(TypeTreeViewItem itemBase, SortedList<string, Type> list)
        {
            foreach (KeyValuePair<string, Type> kvp in list)
            {
                if (kvp.Value.BaseType == itemBase.Type)    //需事先创建根节点，该语句会过滤掉根节点
                {
                    TypeTreeViewItem item = new TypeTreeViewItem(kvp.Value);
                    itemBase.Items.Add(item);
                    CreateLinkedItems(item, list);
                }
            }
        }
    }
}
