using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;
using System.Windows.Data;

namespace ExploreDependencyProperties
{
    class DependencyPropertyListView : ListView
    {
        public static DependencyProperty TypeProperty;

        public Type Type
        {
            set { SetValue(TypeProperty, value); }
            get { return (Type)GetValue(TypeProperty); }
        }

        static DependencyPropertyListView()
        {
            TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Type), typeof(DependencyPropertyListView), new PropertyMetadata(null, new PropertyChangedCallback(OnTypePropertyChanged)));
        }

        static void OnTypePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DependencyPropertyListView lstvue = obj as DependencyPropertyListView;

            Type type = args.NewValue as Type;

            lstvue.ItemsSource = null;

            if (type != null)
            {
                SortedList<string, DependencyProperty> list = new SortedList<string, DependencyProperty>();
                FieldInfo[] infos = type.GetFields();   //获取类型中的所有字段信息                

                //提取字段类型是DependencyProperty类型的字段
                foreach (FieldInfo info in infos)
                {
                    if (info.FieldType == typeof(DependencyProperty))   
                    {
                        list.Add(info.Name, (DependencyProperty)info.GetValue(null));
                    }
                }

                lstvue.ItemsSource = list.Values;
            }
        }

        public DependencyPropertyListView()
        {
            GridView grdvue = new GridView();
            this.View = grdvue;

            GridViewColumn col = new GridViewColumn();
            col.Header = "Name";
            col.Width = 150;
            col.DisplayMemberBinding = new Binding(nameof(DependencyProperty.Name));
            grdvue.Columns.Add(col);

            col = new GridViewColumn();
            col.Header = "Owner";
            col.Width = 100;
            grdvue.Columns.Add(col);

            DataTemplate template = new DataTemplate();
            col.CellTemplate = template;
            FrameworkElementFactory elTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            template.VisualTree = elTextBlock;
            Binding bind = new Binding(nameof(DependencyProperty.OwnerType));
            bind.Converter = new TypeToString();
            elTextBlock.SetBinding(TextBlock.TextProperty, bind);

            col = new GridViewColumn();
            col.Header = "Type";
            col.Width = 100;
            grdvue.Columns.Add(col);

            template = new DataTemplate();
            col.CellTemplate = template;
            elTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            template.VisualTree = elTextBlock;
            bind = new Binding(nameof(DependencyProperty.PropertyType));            
            bind.Converter = new TypeToString();
            elTextBlock.SetBinding(TextBlock.TextProperty, bind);

            col = new GridViewColumn();
            col.Header = "Default";
            col.Width = 75;
            col.DisplayMemberBinding = new Binding(nameof(DependencyProperty.DefaultMetadata.DefaultValue));            
            grdvue.Columns.Add(col);

            col = new GridViewColumn();
            col.Header = "Read-Only";
            col.Width = 75;
            col.DisplayMemberBinding = new Binding(nameof(DependencyProperty.ReadOnly));
            grdvue.Columns.Add(col);

            col = new GridViewColumn();
            col.Header = "Usage";
            col.Width = 75;
            col.DisplayMemberBinding = new Binding("DefaultMetadata.AttachedPropertyUsage");            
            grdvue.Columns.Add(col);

            col = new GridViewColumn();
            col.Header = "Flags";
            col.Width = 250;
            grdvue.Columns.Add(col);

            template = new DataTemplate();
            col.CellTemplate = template;
            elTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            template.VisualTree = elTextBlock;
            bind = new Binding(nameof(DependencyProperty.DefaultMetadata));
            bind.Converter = new MetadataToFlags();
            elTextBlock.SetBinding(TextBlock.TextProperty, bind);
        }
    }
}
