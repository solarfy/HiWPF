﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChooseFont
{
    public class FontDialog : Window
    {
        TextBoxWithLister boxFamily, boxStyle, boxWeight, boxStretch, boxSize;
        Label lblDisplay;
        bool isUpdateSuppressed = true;

        public Typeface Typeface
        {
            set 
            {
                if (boxFamily.Contains(value.FontFamily))
                    boxFamily.SelectedItem = value.FontFamily;
                else
                    boxFamily.SelectedIndex = 0;

                if (boxStyle.Contains(value.Style))
                    boxStyle.SelectedItem = value.Style;
                else
                    boxStyle.SelectedIndex = 0;

                if (boxWeight.Contains(value.Weight))
                    boxWeight.SelectedItem = value.Weight;
                else
                    boxWeight.SelectedIndex = 0;

                if (boxStretch.Contains(value.Stretch))
                    boxStretch.SelectedItem = value.Stretch;
                else
                    boxStretch.SelectedIndex = 0;
            }

            get 
            {
                return new Typeface((FontFamily)boxFamily.SelectedItem, (FontStyle)boxStyle.SelectedItem, (FontWeight)boxWeight.SelectedItem, (FontStretch)boxStretch.SelectedItem);
            }
        }

        public double FaceSize
        {
            set 
            {
                double size = 0.75 * value;
                boxSize.Text = size.ToString();

                if (!boxSize.Contains(size))
                    boxSize.Insert(0, size);
            }

            get 
            {
                double size;

                if (!Double.TryParse(boxSize.Text, out size))
                    size = 8.25;

                return size / 0.75;
            }
        }

        public FontDialog()
        {
            this.Title = "Font";
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;

            Grid gridMain = new Grid();
            this.Content = gridMain;

            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = new GridLength(200, GridUnitType.Pixel);
            gridMain.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            gridMain.RowDefinitions.Add(rowdef);

            ColumnDefinition coldef = new ColumnDefinition();
            coldef.Width = new GridLength(650, GridUnitType.Pixel);
            gridMain.ColumnDefinitions.Add(coldef);

            Grid gridBoxes = new Grid();
            gridMain.Children.Add(gridBoxes);

            rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            gridBoxes.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition();
            rowdef.Height = new GridLength(100, GridUnitType.Star);
            gridBoxes.RowDefinitions.Add(rowdef);

            //第一列用作FontFamily
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(175, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            //第二列用作FontStyle
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(100, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            //第三列用作FontWeight
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(175, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            //第四列用作FontStretch
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(175, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            //第五列用作Size
            coldef = new ColumnDefinition();
            coldef.Width = new GridLength(75, GridUnitType.Star);
            gridBoxes.ColumnDefinitions.Add(coldef);

            Label lbl = new Label();
            lbl.Content = "Font Family";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 0);

            boxFamily = new TextBoxWithLister();
            boxFamily.IsReadOnly = false;   //允许输入
            boxFamily.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxFamily);
            Grid.SetRow(boxFamily, 1);
            Grid.SetColumn(boxFamily, 0);

            lbl = new Label();
            lbl.Content = "Style";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 1);

            boxStyle = new TextBoxWithLister();
            boxStyle.IsReadOnly = true;
            boxStyle.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxStyle);
            Grid.SetRow(boxStyle, 1);
            Grid.SetColumn(boxStyle, 1);

            lbl = new Label();
            lbl.Content = "Weight";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 2);

            boxWeight = new TextBoxWithLister();
            boxWeight.IsReadOnly = true;
            boxWeight.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxWeight);
            Grid.SetRow(boxWeight, 1);
            Grid.SetColumn(boxWeight, 2);

            lbl = new Label();
            lbl.Content = "Stretch";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 3);

            boxStretch = new TextBoxWithLister();
            boxStretch.IsReadOnly = true;
            boxStretch.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxStretch);
            Grid.SetRow(boxStretch, 1);
            Grid.SetColumn(boxStretch, 3);

            lbl = new Label();
            lbl.Content = "Size";
            lbl.Margin = new Thickness(12, 12, 12, 0);
            gridBoxes.Children.Add(lbl);
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 4);

            boxSize = new TextBoxWithLister();
            boxSize.Margin = new Thickness(12, 0, 12, 12);
            gridBoxes.Children.Add(boxSize);
            Grid.SetRow(boxSize, 1);
            Grid.SetColumn(boxSize, 4);

            lblDisplay = new Label();
            lblDisplay.Content = "AaBbCc XxYyZz 012345";
            lblDisplay.HorizontalContentAlignment = HorizontalAlignment.Center;
            lblDisplay.VerticalContentAlignment = VerticalAlignment.Center;
            gridMain.Children.Add(lblDisplay);
            Grid.SetRow(lblDisplay, 1);

            Grid gridButton = new Grid();
            gridMain.Children.Add(gridButton);
            Grid.SetRow(gridButton, 2);

            for (int i = 0; i < 5; i++)
                gridButton.ColumnDefinitions.Add(new ColumnDefinition());

            Button btn = new Button();
            btn.Content = "OK";
            btn.IsDefault = true;   //Enter键 执行
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.MinWidth = 60;
            btn.Margin = new Thickness(12);
            btn.Click += OkOnClick;
            gridButton.Children.Add(btn);
            Grid.SetColumn(btn, 1);

            btn = new Button();
            btn.Content = "Cancel";
            btn.IsCancel = true;    //ESC键 关闭
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.MinWidth = 60;
            btn.Margin = new Thickness(12);            
            gridButton.Children.Add(btn);
            Grid.SetColumn(btn, 3);

            foreach (FontFamily fam in Fonts.SystemFontFamilies)
            {
                boxFamily.Add(fam);
            }
            //此处根据选中的字体，决定字体的拉伸，加粗，倾斜
            //foreach (PropertyInfo property in typeof(FontStyles).GetProperties())
            //{
            //    boxStyle.Add((FontStyle)property.GetValue(null));
            //}
            //foreach (PropertyInfo property in typeof(FontWeights).GetProperties())
            //{
            //    boxWeight.Add((FontWeight)property.GetValue(null));
            //}
            //foreach (PropertyInfo property in typeof(FontStretches).GetProperties())
            //{
            //    boxStretch.Add((FontStretch)property.GetValue(null));
            //}

            double[] ptsizes = new double[] { 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (double ptsize in ptsizes)
                boxSize.Add(ptsize);

            boxFamily.SelectionChanged += FamilyOnSelectionChanged;
            boxStyle.SelectionChanged += StyleOnSelectionChanged;
            boxWeight.SelectionChanged += StyleOnSelectionChanged;
            boxStretch.SelectionChanged += StyleOnSelectionChanged;
            boxSize.TextChanged += SizeOnTextChanged;

            Typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            FaceSize = FontSize;

            boxFamily.Focus();

            isUpdateSuppressed = false;
            UpdateSample();
        }

        void FamilyOnSelectionChanged(object sender, EventArgs args)
        {
            FontFamily fntfam = (FontFamily)boxFamily.SelectedItem;

            FontStyle? fntstyPrevious = (FontStyle?)boxStyle.SelectedItem;
            FontWeight? fntwtPrevious = (FontWeight?)boxWeight.SelectedItem;
            FontStretch? fntstrPrevious = (FontStretch?)boxStretch.SelectedItem;

            isUpdateSuppressed = true;

            boxStyle.Clear();
            boxWeight.Clear();
            boxStretch.Clear();

            foreach(FamilyTypeface ftf in fntfam.FamilyTypefaces)
            {
                if (!boxStyle.Contains(ftf.Style))
                {
                    if (ftf.Style == FontStyles.Normal)
                        boxStyle.Insert(0, ftf.Style);
                    else
                        boxStyle.Add(ftf.Style);
                }

                if (!boxWeight.Contains(ftf.Weight))
                {
                    if (ftf.Weight == FontWeights.Normal)
                        boxWeight.Insert(0, ftf.Weight);
                    else
                        boxWeight.Add(ftf.Weight);
                }

                if (!boxStretch.Contains(ftf.Stretch))
                {
                    if (ftf.Stretch == FontStretches.Normal)
                        boxStretch.Insert(0, ftf.Stretch);
                    else
                        boxStretch.Add(ftf.Stretch);
                }               
            }

            if (boxStyle.Contains(fntstyPrevious))
                boxStyle.SelectedItem = fntstyPrevious;
            else
                boxStyle.SelectedIndex = 0;

            if (boxWeight.Contains(fntwtPrevious))
                boxWeight.SelectedItem = fntwtPrevious;
            else
                boxWeight.SelectedIndex = 0;

            if (boxStretch.Contains(fntstrPrevious))
                boxStretch.SelectedItem = fntstrPrevious;
            else
                boxStretch.SelectedIndex = 0;

            isUpdateSuppressed = false;
            UpdateSample();
        }

        void StyleOnSelectionChanged(object sender, EventArgs args)
        {
            UpdateSample();
        }

        void SizeOnTextChanged(object sender, TextChangedEventArgs args)
        {
            UpdateSample();
        }

        void UpdateSample()
        {
            if (isUpdateSuppressed)
                return;

            lblDisplay.FontFamily = (FontFamily)boxFamily.SelectedItem;
            lblDisplay.FontStyle = (FontStyle)boxStyle.SelectedItem;
            lblDisplay.FontWeight = (FontWeight)boxWeight.SelectedItem;
            lblDisplay.FontStretch = (FontStretch)boxStretch.SelectedItem;

            double size;
            if (!Double.TryParse(boxSize.Text, out size))
                size = 8.25;
            lblDisplay.FontSize = size / 0.75;
        }

        void OkOnClick(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
        }
    }
}
