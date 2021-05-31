using System.Windows;
using System.Windows.Controls;

namespace NotepadClone
{
    class WordWrapMenuItem : MenuItem
    {
        public static DependencyProperty WordWrapProperty = DependencyProperty.Register(nameof(WordWrap), typeof(TextWrapping), typeof(WordWrapMenuItem));
        public TextWrapping WordWrap
        {
            set => SetValue(WordWrapProperty, value);
            get => (TextWrapping)GetValue(WordWrapProperty);
        }

        public WordWrapMenuItem()
        {
            this.Header = "_Word Wrap";

            MenuItem item = new MenuItem();
            item.Header = "_No Wrap";
            item.Tag = TextWrapping.NoWrap;
            item.Click += MenuItemOnClick;
            this.Items.Add(item);

            item = new MenuItem();
            item.Header = "_Wrap";
            item.Tag = TextWrapping.Wrap;
            item.Click += MenuItemOnClick;
            this.Items.Add(item);

            item = new MenuItem();
            item.Header = "Wrap With _Overflow";
            item.Tag = TextWrapping.WrapWithOverflow;
            item.Click += MenuItemOnClick;
            this.Items.Add(item);
        }

        //打开子项目时，勾选选中的TextWrapping
        protected override void OnSubmenuOpened(RoutedEventArgs e)
        {
            base.OnSubmenuOpened(e);

            foreach (MenuItem item in this.Items)
                item.IsChecked = ((TextWrapping)item.Tag == WordWrap);
        }

        void MenuItemOnClick(object sender, RoutedEventArgs args)
        {
            WordWrap = (TextWrapping)(args.Source as MenuItem).Tag;
        }
    }
}
