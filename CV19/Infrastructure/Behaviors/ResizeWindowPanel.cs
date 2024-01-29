using CV19.Infrastructure.Extentions;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace CV19.Infrastructure.Behaviors
{
    class ResizeWindowPanel : Behavior<Panel>
    {
        protected override void OnAttached() => AssociatedObject.MouseLeftButtonDown += OnMouseDown;

        protected override void OnDetaching() => AssociatedObject.MouseLeftButtonDown -= OnMouseDown;

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            /* Нам нужно окно: */
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;

            /* Выясним, кто является источником события (линия или прямоугольник). Источник события хранится в параметре самого события e.OriginalSource: */
            switch (e.OriginalSource)
            {
                default: return;    // Если это не линия и не прямоугольник, то мы просто выходим.
                case Line line:
                    ResizeLine(line, window);
                    break;

                case Rectangle rect:
                    ResizeRect(rect, window);
                    break;
            }
        }

        public enum SizingAction
        {
            West = 1,
            East = 2,
            North = 3,
            NorthWest = 4,
            NorthEast = 5,
            South = 6,
            SouthWest = 7,
            SouthEast = 8,
        }

        private static void ResizeLine(Line line, Window window)
        {
            switch (line.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.North), IntPtr.Zero);
                    break;
                case VerticalAlignment.Bottom:
                    window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.South), IntPtr.Zero);
                    break;
                default:
                    switch (line.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.West), IntPtr.Zero);
                            break;
                        case HorizontalAlignment.Right:
                            window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.East), IntPtr.Zero);
                            break;
                    }
                    break;

            }
        }

        private static void ResizeRect(Rectangle rect, Window window)
        {
            switch (rect.VerticalAlignment)
            {
                /* when rect.HorizontalAlignment - дополнительное условие: */
                case VerticalAlignment.Top when rect.HorizontalAlignment == HorizontalAlignment.Left:
                    window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.NorthWest), IntPtr.Zero);
                    break;
                case VerticalAlignment.Top when rect.HorizontalAlignment == HorizontalAlignment.Right:
                    window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.NorthEast), IntPtr.Zero);
                    break;
                case VerticalAlignment.Bottom when rect.HorizontalAlignment == HorizontalAlignment.Left:
                    window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.SouthWest), IntPtr.Zero);
                    break;
                case VerticalAlignment.Bottom when rect.HorizontalAlignment == HorizontalAlignment.Right:
                    window.SendMessage(WM.SYSCOMMAND, (IntPtr)((int)SC.SIZE + SizingAction.SouthEast), IntPtr.Zero);
                    break;
            }
        }
    }
}
