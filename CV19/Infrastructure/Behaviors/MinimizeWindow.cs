using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace CV19.Infrastructure.Behaviors
{
    // Сделаем поведение, которое будет минимизировать окно при нажатии на соответствующую кнопку:
    class MinimizeWindow : Behavior<Button>
    {
        protected override void OnAttached() => AssociatedObject.Click += OnButtonClick;
        protected override void OnDetaching() => AssociatedObject.Click -= OnButtonClick;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;
            window.WindowState = WindowState.Minimized;
        }

        // Теперь внедрим этот класс в наши кнопки в файле CV19WindowStyle.xaml...
    }
}
