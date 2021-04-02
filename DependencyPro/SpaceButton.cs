using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DependencyPro
{
    class SpaceButton : Button
    {
        private string txt;

        public string Text
        {
            get { return txt; }
            set 
            { 
                txt = value;
                this.Content = SpaceOutText(txt);
            }
        }

        //Space的个数
        public static readonly DependencyProperty SpaceProperty;
        public int Space
        {
            set => SetValue(SpaceProperty, value);
            get => (int)GetValue(SpaceProperty);
        }

        static SpaceButton()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            metadata.DefaultValue = 2;
            metadata.AffectsMeasure = true;     //支持尺寸重新调整
            metadata.Inherits = true;   //支持继承
            metadata.PropertyChangedCallback += OnSpacePropertyChanged;

            SpaceProperty = DependencyProperty.Register(nameof(Space), typeof(int), typeof(SpaceButton), metadata, ValidateSpaceValue);
        }

        static void OnSpacePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SpaceButton btn = obj as SpaceButton;
            btn.Content = btn.SpaceOutText(btn.Text);
        }

        static bool ValidateSpaceValue(object obj)
        {
            int i = (int)obj;
            return i >= 0;
        }

        string SpaceOutText(string str)
        {
            if (str == null)
                return null;

            StringBuilder build = new StringBuilder();

            foreach (char ch in str)
            {
                build.Append(ch + new string(' ', Space));
            }

            return build.ToString();
        }


        //附加属性
        public static int GetSpaceCount(DependencyObject obj)
        {
            //return (int)obj.GetValue(SpaceCountProperty);
            return (int)obj.GetValue(SpaceProperty);
        }

        public static void SetSpaceCount(DependencyObject obj, int value)
        {
            //obj.SetValue(SpaceCountProperty, value);
            obj.SetValue(SpaceProperty, value);
        }
        
        public static readonly DependencyProperty SpaceCountProperty =
            DependencyProperty.RegisterAttached("SpaceCount", typeof(int), typeof(SpaceButton), new PropertyMetadata(0));




    }

}
