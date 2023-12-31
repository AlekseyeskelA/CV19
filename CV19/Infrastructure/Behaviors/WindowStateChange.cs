using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace CV19.Infrastructure.Behaviors
{
    class WindowStateChange : Behavior<Button>
    {
        protected override void OnAttached() => AssociatedObject.Click += OnButtonClick;
        protected override void OnDetaching() => AssociatedObject.Click -= OnButtonClick;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            //var window = AssociatedObject.FindVisualRoot() as Window;
            //if (window is null) return;

            // Упростим:
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;

            //switch (window.WindowState)
            //{
            //    case WindowState.Normal:
            //        window.WindowState = WindowState.Maximized;
            //        break;
            //    case WindowState.Maximized:
            //        window.WindowState = WindowState.Normal;
            //        break;
            //}

            // Упростим:
            window.WindowState = window.WindowState switch
            {
                WindowState.Normal => WindowState.Maximized,
                WindowState.Maximized => WindowState.Normal,
                _ => window.WindowState
            };
        }

        // Теперь внедрим этот класс в наши кнопки в файле CV19WindowStyle.xaml...
    }
}
