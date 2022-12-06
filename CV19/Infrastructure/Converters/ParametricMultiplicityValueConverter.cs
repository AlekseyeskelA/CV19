using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CV19.Infrastructure.Converters
{
    /* На принципах DependencyObject создадим конвертер, к которому можно создавать привязку извне, т.е. у него будет параметр, который можно будет привязывать.
     * Проблена в том, что у нас уже есть конвертеры, но они наследуются от базового класса MarkupExtension, т.е. они являются расширениями разметки, а расширение
     * разметки не является DependencyObject-объектом, и следовательно в них нельзя описывать свойства-зависимости. Поэтому создадим альтернативную ветвь конвертеров
     * DependencyObject, т.е. мы наследуемся от другого класса (DependencyObject) и реализуем интерфейс IValueConvehter:*/

    /* Привязка к мультипликатору не работает потому, что в строке <converters:ParametricMultiplicityValueConverter... в сам элемент не попадает DataContext.
     * DataContext не передается от базового элемента Grid, дочернего элемента GaugeIndicator в элемент ParametricMultiplicityValueConverter. Это связано с тем,
     * что недостаточно просто использовать DependencyObject в качетсве наследника. Если нам нужно работать с контекстом данных, то потребуется объект-наследник
     * класса Freezable и реализовать метод CreateInstanceCore(), в котором создать новый экземпляр ParametricMultiplicityValueConverter.*/
    internal class ParametricMultiplicityValueConverter : /*DependencyObject*/ Freezable , IValueConverter
    {
        #region Value : double : - Прибавляемое значение

        /// <summary>Прибавляемое значение</summary>
        /* Еще один способ, как поймать изменение свойства-зависимости, это в new PropertyMetadata(1.0) дополнительно прписать делегат (d,e) => {}, в  котором
         * поставить точку остановки:*/
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(ParametricMultiplicityValueConverter),
                new PropertyMetadata(1.0/*, (d,e) => { }*/));

        /// <summary>Прибавляемое значение</summary>
        //[Category("")]
        [Description("Прибавляемое значение")]
        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        #endregion

        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            var value = System.Convert.ToDouble(v, c);

            return value * Value;
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            var value = System.Convert.ToDouble(v, c);

            return value / Value;
        }

        // { Value = Value } - скомпилируем свойство у объекта ParametricMultiplicityValueConverter.
        protected override Freezable CreateInstanceCore() => new ParametricMultiplicityValueConverter { Value = Value };
    }
}
