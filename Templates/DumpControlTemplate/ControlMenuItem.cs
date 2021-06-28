using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace DumpControlTemplate
{
    class ControlMenuItem : MenuItem
    {
        public ControlMenuItem()
        {
            Assembly asbly = Assembly.GetAssembly(typeof(Control));
            Type[] atype = asbly.GetTypes();        //获取Control类中所有的类型

            SortedList<string, MenuItem> sortlst = new SortedList<string, MenuItem>();

            this.Header = "Control";
            this.Tag = typeof(Control);
            sortlst.Add(nameof(Control), this);

            //枚举所有的类型，为Control类及继承自Control的类创建一个MenuItem，并将其添加到SortedList中；将该类的Type与MenuItem相关联
            foreach (Type typ in atype)
            {
                if (typ.IsPublic && (typ.IsSubclassOf(typeof(Control))))
                {
                    MenuItem item = new MenuItem();
                    item.Header = typ.Name;
                    item.Tag = typ;
                    sortlst.Add(typ.Name, item);
                }
            }

            //遍历SortedList并设置相关MenuItem的Parent
            foreach (KeyValuePair<string, MenuItem>kvp in sortlst)
            {
                if (kvp.Key != nameof(Control)) //根据上文过滤，如果Key不为Control则表示该类继承自Control
                {
                    string strParent = ((Type)kvp.Value.Tag).BaseType.Name;
                    MenuItem itemParent = sortlst[strParent];
                    itemParent.Items.Add(kvp.Value);
                }
            }

            //再次遍历SortedList，如果是抽象项并且无子项的则禁用；如果不是抽象并且有子项的，则创建一个MenuItem
            foreach (KeyValuePair<string, MenuItem>kvp in sortlst)
            {
                Type typ = (Type)kvp.Value.Tag;

                if (typ.IsAbstract && kvp.Value.Items.Count == 0)
                    kvp.Value.IsEnabled = false;

                if (!typ.IsAbstract && kvp.Value.Items.Count > 0)
                {
                    MenuItem item = new MenuItem();
                    item.Header = kvp.Value.Header as string;
                    item.Tag = typ;
                    kvp.Value.Items.Insert(0, item);
                }
            }
        }
    }
}
