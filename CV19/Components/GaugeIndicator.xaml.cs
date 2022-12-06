using System;
using System.ComponentModel;
using System.Windows;

namespace CV19.Components
{
    public partial class GaugeIndicator
    {
        #region ValueProperty

        /* Для привязки стрелки к данным используем свойсовто-зависимости, так как свойство Angle нельзяя связать через Binding: C# пытается double перевести в Binding.
        Объявим статическое поле DependencyProperty ValueProperty (слово Property необходимо в названии ValueProperty), возьмём класс DependencyProperty и вызвать
        метод Register(), который создаст нам этот объект, зарегистрирует свойство зависимости.
        - В скобках зарегистрируем имя свойства-зависимости. Можно написать просто "Value", но лучше использовать конструкцию, которая потом позволяет использовать
        рефакторинг (nameof(Value)).
        - Далее необходимо указать, какого типа будет свойство-зависимости.
        - Далее указываем тип, к которому принадлежит это свойство.
        - Далее добавляем информацию о том, как этот объект будет работать (PropertyMetadata). В этом объекте можно указать значение свойства по умолчанию,
        причём того же типа, что прописан во втором члене. Обычно здесь пишут дефолтное значение этого же типа.
        Также здесь можно указать два мотода:
          Первый метод OnValuePropertyChanged будет вызываться всякий раз, когда свойство меняется (любым средством: с помощью привязок или вызова
        свойства public double Value). Т.е., если требуется обработать изменение этого свойства, то нужно добавть этот метод.
          Второй метод OnCoerceValue позволяет скорректировать знечение свойства. Т.е., если нам передали некорректное значение, например, значение > 100,
        то мы сможем вернуть корректное.
        - Также можно указать ещё один метод OnValidateValue.
        Есть ещё два дополнительных наследника (FrameworkPropertyMetadata и UIPropertyMetadata), которые позволяют конкретизировать какие-то особенности этого свойства.*/
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(GaugeIndicator),
                new PropertyMetadata(
                    default(double),
                    OnValuePropertyChanged,
                    OnCoerceValue),
                OnValidateValue);

        /*Этот метод получает новое установленное значение object value и возвращает либо ИСТИНУ, любо ЛОЖЬ. Если мотод возвращает ЛОЖЬ, то привязка не срабатывает,
         и свойство не устанавливается. Если возвращается ИСТИНА, то новое значение становится значением этого свойства. Т.е. в этом методе можно контролировать
         возможность установки очередного нового значения. Здесь мы будем возвращать всегда ИСТИНУ*/
        private static bool OnValidateValue(object value)
        {
            return true;
        }

        /*object baseValue - получеам установленное значение, а возвращаем - object - скорректированное значение. Для начала будем возвращать baseValue*/
        private static object OnCoerceValue(DependencyObject d, object baseValue)
        {
            //return baseValue;

            // Сделаем небольшую коррекцию значений:
            var value = (double)baseValue;
            return Math.Max(0, Math.Min(100, value));           // Теперь все значения больше сотни и меньше нуля будут отсекаться и устанавливаться граничные значения
                                                                // для интервала. Впринципе, можно было бы метод OnValidateValue настроить таким же способом.
        }

        /* DependencyObject d - объект, для которого изменяется свойство.
        DependencyPropertyChangedEventArgs e - объект, который содержит информацию о том, как это свойство изменяется, т.е., в нём быдет старое и новое значение,
        что бывает очень удобно.*/
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Решим задачу пока в лоб, безо всяких привязок:
            //var gauge_indicator = (GaugeIndicator)d;

            // Установим значение:
            //gauge_indicator.ArrowRotator.Angle = (double)e.NewValue;
        }

        // Далее ннам понадобится само свойство. Через это свойство можно общаться со свойством-зависимости напрямую через объект, а привязки будут работать через
        // объект ValueProperty:
        public double Value
        {
            // в get нужно вызвать метод GetValue() и указать, откуда мы хотим его получить ValueProperty.
            get => (double)GetValue(ValueProperty);
            // в set указываем ValueProperty и указываем устанавливаемое значение value.
            set => SetValue(ValueProperty, value);
        }

        /* Однако, такая реализация свойства-зависимости нехорошая. Мы не должны внутри свойства-зависимости реализовывать методы (OnValidateValue, OnCoerceValue,
         * OnValuePropertyChanged), которые как-то обновляют интерфейс при установке новых значений. Потому, что мы заставляем систему выполнять код
         * метода OnValuePropertyChanged и вручную устанавливать свойство Angle, но зачем так делать, если у нас есть привязки. Мы можем взять и привязать
        свойство ValueProperty к свойству Angle объекта ArrowRotator, и в таком случае строки  var gauge_indicator... и gauge_indicator... мотода
        OnValuePropertyChanged нам не понадобятся. Убираем их и далее в разметке GaugeIndicator, и у него приязываемся с его свойству Value.
        Теперь всё работает исключительно по средствам привязок к данным, т.е. всё связано теперь у нас теперь внутри системы свойств-зависимости.*/

        #endregion

        //Упровтим код до самого необходимого, при котором он уже может работать:
         //Не нужно никакой логики дополнительно прописывать в PropertyMetadata(). Всю логику нужно свести к максимальному использованию привязок к данным.
        #region Angle : double - Какой-то угол

        /// <summary>Какой-то угол</summary>
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(
                nameof(Angle),
                typeof(double),
                typeof(GaugeIndicator),
                new PropertyMetadata(default(double)));

        /// <summary>Какой-то угол</summary>        
        // К свойству зависимостей можно прикреплять описание, которое в последствии можно будет видеть в панели свойств. И можно указать категорию, куда размещать это свойство:
        [Category("Моя категория")]
        [Description("Какой - то угол")]
        // Таким образом мы получаем новое свойство нашего GaugeIndicator-а, которое видно в панели свойств, если в xaml-разметке в MainWindow, если его выделить.
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        #endregion

        public GaugeIndicator() => InitializeComponent();
    }
}
