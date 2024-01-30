using System.Windows.Media;

namespace System.Windows
{
    //Класс должен быть статическим статическим. И поместим его в простанство имён using System.Windows (добавим в заголовке):
    static class DependencyObjectExtentions
    {
        //У нас будет два метода, которые позволяют для любого визуального объекта DependencyObject найти его корневой элемент:
        public static DependencyObject FindVisualRoot (this DependencyObject obj)
        {
            do
            {
                var parent = VisualTreeHelper.GetParent(obj);
                if (parent is null) return obj;
                obj = parent;
            }
            while (true);
        }

        public static DependencyObject FindLogicalRoot(this DependencyObject obj)
        {
            do
            {
                var parent = LogicalTreeHelper.GetParent(obj);
                if (parent is null) return obj;
                obj = parent;
            }
            while (true);
        }

        /* Добавим третий метод (шаблонный) для поиска родительского элемента нужного типа. Будем указывать тип элемента, который мы ищем, вверх по дереву.
         * Тип ограничен тем, что это будет любой объект-наследник DependencyObject: */
        public static T FindVisualParent<T>(this DependencyObject obj) 
            where T : DependencyObject
        {
            if (obj is null) return null;
            var target = obj;
            /* Далее в цикле с помощью класса VisualTreeHelper вызываем метод GetParent, т.е. поднимаемся вверх до тех пор пока target не равен null (т.е. пустая ссылка), либо target
               не является указанным типом:*/
            do
            {
                target = VisualTreeHelper.GetParent(target);
            }
            while (target != null && !(target is T));
            return target as T;
        }

        /* Сделаем копию метода для логического дерева: */
        public static T FindLagicalParent<T>(this DependencyObject obj) 
            where T : DependencyObject
        {
            if (obj is null) return null;
            var target = obj;
            /* Далее в цикле с помощью класса VisualTreeHelper вызываем метод GetParent, т.е. поднимаемся вверх до тех пор пока target не равен null (т.е. пустая ссылка), либо target
               не является указанным типом:*/
            do
            {
                target = LogicalTreeHelper.GetParent(target);
            }
            while (target != null && !(target is T));
            return target as T;
        }
    }
}
