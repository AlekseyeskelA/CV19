using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace CV19.Infrastructure.Behaviors
{
    /* : Behavior<UIElement> - наделим этот класс поведением для любого элемента, чтобы его можно было привязать к любому визуальному элементу */
    class WindowsTitleBarBehavior : Behavior<UIElement>
    {
        private Window _Window;

        /* Присосединение: */
        protected override void OnAttached()
        {
            _Window = AssociatedObject as Window ?? AssociatedObject.Fi;
        }

        /* Отсосединение: */
        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
