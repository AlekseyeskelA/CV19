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
            if (e.ClickCount > 1) return; // Это условие нужно для того, чтобы обработать развёртывание окна двойным кликом по заголовку.
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;
            window.DragMove();
        }
    }
}
