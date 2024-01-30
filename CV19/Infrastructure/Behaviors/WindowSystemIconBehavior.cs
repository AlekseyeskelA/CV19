using CV19.Infrastructure.Extentions;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CV19.Infrastructure.Behaviors
{
    class WindowSystemIconBehavior : Behavior<Image>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += OnMouseDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(AssociatedObject.FindVisualRoot() is Window window)) return;

            /* Говорим, что мы обработали событие, и его больше никому показывать не надо. В этом случае все остальные обработчики событий прекратят свою работу с событием
             * MouseLeftButtonDown:*/
            e.Handled = true;

            /* Теперь анализируем количество щелчков мышки: */
            if (e.ClickCount > 1)
                window.Close();
            else
            {
                /* Если клик только один, то вызываем системное меню, а его мы сможем отобразить только с помощью windows API. Для этого напишем ещё один метод расширения
                 * в классе WindowsExtentions в папке Extentions, который будет называться SendMessage: */
                window.SendMessage(WM.SYSCOMMAND, SC.KEYMENU); // WM.SYSCOMMAND - сообщение, SC.KEYMENU - параметр сообщения.
            }

        }
    }
}
