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
    }
}
