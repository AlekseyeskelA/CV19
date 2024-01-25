using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace CV19.Infrastructure.Behaviors
{
    /* : Behavior<UIElement> - наделим этот класс поведением для любого элемента, чтобы его можно было привязать к любому визуальному элементу */
    class WindowsTitleBarBehavior : Behavior<UIElement>
    {
        private Window _Window;

        /* Присосединение: */
        protected override void OnAttached()
        {
            /* Рассматриваем текущий метод как окно. Если это не окно. то  */
            _Window = AssociatedObject as Window ?? AssociatedObject.FindLagicalParent<Window>();

            /* Реализуем перемещение окна. для этого нам нужно добавить обработчики события: */
            AssociatedObject.MouseLeftButtonDown += OnMouseDown;
        }

        /* Отсосединение: */
        protected override void OnDetaching()
        {
            /* Отпишемся от события, если оно нам больше не нужно. Зааодно обнулим ссылку на окно: */
            AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
            _Window = null;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1:
                    DragMove();
                    break;
                default:
                    Maximize();
                    break;
            }
        }

        private void DragMove()
        {
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;
            window.DragMove();
        }

        private void Maximize()
        {
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;
            window.WindowState = window.WindowState switch
            {
                WindowState.Normal => WindowState.Maximized,
                WindowState.Maximized => WindowState.Normal,
                _ => window.WindowState
            };
        }
    }
}
