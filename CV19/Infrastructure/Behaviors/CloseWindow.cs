using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CV19.Infrastructure.Behaviors
{
    internal class CloseWindow : Behavior<Button>
    {
        ///* Напишем поведение для кнопки, которое будет подписываться на событие Click: */
        //protected override void OnAttached() => AssociatedObject.Click += OnButtonClick;
        //protected override void OnDetaching() => AssociatedObject.Click -= OnButtonClick;

        //private void OnButtonClick(object sender, RoutedEventArgs e)
        //{
        //    /* У нас есть ассоциированный объект - наша кнопка: */
        //    var button = AssociatedObject;

        //    var window = FindVisualRoot(button) as Window;

        //    /* Теперь для окна мы можем вызвать метод Close. Если окно не найдём, то кнопка работать не будет:*/
        //    window?.Close();
        //}

        //private static DependencyObject FindVisualRoot(DependencyObject obj)
        //{
        //    /* Надо найти окно, в котором содержится эта самая кнопка. Для этого используем класс VisualTreeHelper: */

        //    do
        //    {
        //        var parent = VisualTreeHelper.GetParent(obj);
        //        /* Если родительский жлемент - пустая ссылка, то мы нашли кореь? и возвращаем текущий объект. Иначе перемещаемся к родительскому элементу и проходим для него
        //         * цикл. Таким образом, цикл за циклом мы будем подниматься вверх по визуальному дереву, и рано или поздно дойдём до того, когда для очередного элемента
        //         * родительский элемент будет отсутствовать, и текущий элемент станет корнем дерева, и в этом случае мы скорее всего наткнёмся на окно: */
        //        if (parent is null) return obj;
        //        obj = parent;
        //    }
        //    while (true);
        //}
        ///* Сделаем метод расширения, который позволит для любого DependencyObject найти его  корень, т.е. окно. Для этого в папке Infrastructure добавим ещё одну папку
        // Extentions, и в неё добавим класс DependencyObjectExtentions*/


        //После того, как мы создали расширение для DependencyObject в папке Extentions, данный код можно сократить:
        /* Напишем поведение для кнопки, которое будет подписываться на событие Click: */
        protected override void OnAttached() => AssociatedObject.Click += OnButtonClick;
        protected override void OnDetaching() => AssociatedObject.Click -= OnButtonClick;

        //private void OnButtonClick(object sender, RoutedEventArgs e)
        //{
        //    /* У нас есть ассоциированный объект - наша кнопка: */
        //    var button = AssociatedObject;

        //    var window = button.FindVisualRoot() as Window;

        //    ///* Теперь для окна мы можем вызвать метод Close. Если окно не найдём, то кнопка работать не будет:*/
        //    //window?.Close();

        //    //Упростим:
        //    (button.FindVisualRoot() as Window)?.Close();
        //}

        //Упростим:
        private void OnButtonClick(object sender, RoutedEventArgs e) =>
            (AssociatedObject.FindVisualRoot() as Window)?.Close();

        //Анлоично сделаем класс WinowStatChage.
    }
}
