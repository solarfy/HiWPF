using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CalculateInHex
{
    class RoundedButton : Control
    {
        RoundedButtonDecorator decorator;

        public static readonly RoutedEvent ClickEvent;

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public UIElement Child
        {
            set { decorator.Child = value; }
            get { return decorator.Child; }
        }

        public bool IsPressed
        {
            set { decorator.IsPressed = value; }
            get { return decorator.IsPressed; }
        }

        bool IsMouseReallyOver
        {
            get
            {
                Point pt = Mouse.GetPosition(this);
                return (pt.X >= 0 && pt.X < this.ActualWidth && pt.Y >= 0 && pt.Y < this.ActualHeight);
            }
        }

        static RoundedButton()
        {
            ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RoundedButton));
        }

        public RoundedButton()
        {
            decorator = new RoundedButtonDecorator();
            this.AddVisualChild(decorator);
            this.AddLogicalChild(decorator);
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index > 0)
                throw new ArgumentOutOfRangeException("index");

            return decorator;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            decorator.Measure(constraint);
            return decorator.DesiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            decorator.Arrange(new Rect(new Point(0, 0), arrangeBounds));
            return arrangeBounds;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsMouseCaptured)
                IsPressed = IsMouseReallyOver;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            CaptureMouse();            
            IsPressed = true;
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (IsMouseCaptured)
            {
                if (IsMouseReallyOver)
                    OnClick();

                Mouse.Capture(null);    //同ReleaseMouseCapture()方法
                IsPressed = false;
                e.Handled = true;
            }
        }

        protected virtual void OnClick()
        {
            RoutedEventArgs argsEvent = new RoutedEventArgs();
            argsEvent.RoutedEvent = RoundedButton.ClickEvent;
            argsEvent.Source = this;
            RaiseEvent(argsEvent);
        }
    }
}
