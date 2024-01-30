using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CV19WPFTest.Behaviors
{
    /*Это необычны класс. Необычным его делает наследование от класса Behavior из пространства имён Microsoft.Xaml.Behaviors, который и наделиет его магическими
    возможностями. При этом можно взять либо нетипизированный (не шаблонный) класс Behavior, который можно прикрепить к чему угодно, а можно ограничить область
    его действия конкретным элементом, указав в параметрах шаблона тот тип элемента, к которому его можно будет привязывать Behavior<Ellipse> или
    Behavior<Button>. Если мы хотим использовать типизированную версию, но взять более общий класс, то идём вниз по дереву наследования и используем базовый
    элемент пользовательского интерфейса Behavior<UIElement>:*/
    public class DragInCanvas : Behavior<UIElement>
    {
        private Point _StartPoint;
        private Canvas _Canvas;
        /*Самое важное - это переопределить два метода OnAttached() и OnDetaching(). В этом случае мы сможем отреагировать на ниже описанные два момента времени,
        и выполнить, например, подписку или отписку от событий визуального элемента. Т.е., для того, чтобы реализовать поведение, необходимо реализовать эти
        два метода, которые подписываются и отписываются от системы событий визуального элемента:*/
        /*Этот метод вызывается в момент, когда поведение добавляется в коллекцию <i:Interaction.Behaviors>:*/
        protected override void OnAttached()
        {
            //base.OnAttached();  // Этот базовый метод снутри себя никакой реализации не имеет, поэтому его можно удалить.
            /* Сам визуальный элемент, к которому происходит подключение поведения хранится в свойстве AssociatedObject, и это свойство имеет тип, который мы
             * указали в параметрах шаблона Behavior<UIElement>. В этом случае мы получаем всю систему событий того класса, к которому мы хотим добавить это
             * самое поведение. Например, мы хотим обеспечить реакцию на нажатие кнопки мыши, на перемещение мыши и дальше на отпускание мышки, т.е. у нас должна
             * быть реакция на три события:*/
            AssociatedObject.MouseLeftButtonDown += OnButtonDown;
        }

        // Этот метод вызывается в момент, когда поведение удаляется из коллекции <i:Interaction.Behaviors>:
        protected override void OnDetaching()
        {
            //base.OnDetaching(); // Этот базовый метод снутри себя никакой реализации не имеет, поэтому его можно удалить.
            /* Раз мы подписались на событие AssociatedObject.MouseLeftButtonDown += OnLeftButtonDown, то нужно и отписаться от него в момент, когда поведение
             * изымается из элемента, иначе могут быть различные казусы:*/
            AssociatedObject.MouseLeftButtonDown -= OnButtonDown;

            /* Если произошло нажатие кнопки мыши, а после этого кто-то изъял поведение из визуального элемента, то в этом случае мы должны полностью очистить
             * список всех событий от наших обработчиков: */
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseUp -= OnMouseUp;
        }

        private void OnButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* В момент, когда мы зажимаем кнопку, нам необходимо определить родительский элемент текущего визуального элемента. Для этого сделаем следующее:
             * Используем специальный класс VisualTreeHelper, который предназначен для перемещений по визуальному дереву интерфейса WPF. Мы вызываем метод
             * GetParent(), который позволяет определить визуального предка текущего элемента, и если этот предок не является Canvas, то  есть мы берём
             * предка, пытаемся привести его к Canvas и присвоить переменной _Canvas, то если у на это не получилось, и мы получили null, следовательн
             * элемент у нас лежит не в Canvas, и дальше какая-либо работа бесполезна, такнашеповедениепредназначено для перетаскивания элмента толь в Canvas: */
            if ((_Canvas ??= VisualTreeHelper.GetParent(AssociatedObject) as Canvas) is null)
                return;

            /* Кроме того, нам надо зафиксировать стартовую точку, где произошёл щелчок мышкой. Для этого заведём переменную Point _StartPoint, и извлечём значение
               этой точки из параметром события. Т.е. заяиксируем точку, где произошло первое нажатие мышкой, чтобы дальше расчитывать дельту перемещения
               относительно этой точки, и смещать визуальный элемент на соответствующее значение: */
            _StartPoint = e.GetPosition(AssociatedObject);

            /* Помимо этого, необходимо захватить мышку, т.е. объявить системе, что теперь указателем мышки владеет именно этот визуальный элемент:*/
            AssociatedObject.CaptureMouse();

            /* В момент, когда кнопка мышы нажата, нам нужно подписаться на событие MouseMove: */
            AssociatedObject.MouseMove += OnMouseMove;

            /* Также необходимо подписаться на событие MouseUp, т.е. когда мы отпускаем мышку, мы должны прекратить перетаксивание: */
            AssociatedObject.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            /* Отпишемся от события MouseMove и события MouseUp: */
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseUp -= OnMouseUp;
            /* Освободим мышку: */
            AssociatedObject.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            /* Запишем наш объект в переменную, чтобы не лазить за ним в наше свойство: */
            var obj = AssociatedObject;

            /* Извлекаем текущее положение мышки: */
            /*var current_pos = e.GetPosition(obj);*/   // Это неправильно. Здесь мы на каждый момент перемещения мышки берём координаты из пространства координат
                                                        // самого лемента. Нам же нужно брать координаты из родительского элемента. Для этого захватывает родительский
                                                        // элемент private Canvas _Canvas (см.  в начале), и сделаем это в OnButtonDown;

            var current_pos = e.GetPosition(_Canvas);   // Высчитаем координаты относитель родительского элемента.

            /* Расчитываем смещение: */
            //var delta_x = current_pos.X - _StartPoint.X;
            //var delta_y = current_pos.Y - _StartPoint.Y;

            /* Можно расчитать проще: */
            var delta = current_pos - _StartPoint;

            /* Выполняем установку значения свойств. По сути мы устанавливаем значения свойств Canvas.Left= Canvas.Top= в элементе Ellipse вручную: */
            obj.SetValue(Canvas.LeftProperty, delta.X);
            obj.SetValue(Canvas.TopProperty, delta.Y);

            PositionX = delta.X;
            PositionY = delta.Y;
        }

        #region PositionX : double - Горизонтальное положение
        /// <summary>Горизонтальное смещение</summary>
        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.Register(
                nameof(PositionX),
                typeof(double),
                typeof(DragInCanvas),
                new PropertyMetadata(default(double)));

        /// <summary>Горизонтальное положение</summary>
        //[Category("")]
        [Description("Горизонтальное положение")]
        public double PositionX { get => (double)GetValue(PositionXProperty); set => SetValue(PositionXProperty, value); }
        #endregion

        #region PositionY : double - Вертикальное положение
        /// <summary>Вертикальное положение</summary>
        public static readonly DependencyProperty PositionYProperty =
            DependencyProperty.Register(
                nameof(PositionY),
                typeof(double),
                typeof(DragInCanvas),
                new PropertyMetadata(default(double)));

        /// <summary>Вертикальное положение</summary>
        //[Category("")]
        [Description("Вертикальное положение")]
        public double PositionY { get => (double)GetValue(PositionYProperty); set => SetValue(PositionYProperty, value); }
        #endregion
    }
}
