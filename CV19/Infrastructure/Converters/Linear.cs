using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    /// <summary>Реализация линейнгго преобразования f(x) = k*x + b</summary>

    [ValueConversion(typeof(double), typeof(double))]

    /*В ряде случаев при применении расширения раметки, когда оно объявлено в базовом классе (: MarkupExtension), и при этом есть класы-наследники, которые возвращают также
    сами себя (=> this) через базовый класс, то в этом случае имеет смысл в классе-наследнике прописать, что быдет являться возвращаемым типом значения (с помощь атрибута).
    В этом случае в разметке будут видны свойства:*/
    [MarkupExtensionReturnType(typeof(Linear))]

    internal class Linear : Converter
    {
        // Свойства:
        // Зададим значение коэффициентов по умолчанию.
        // Укажем системе, что эти свойства связаны с аргументами конструкторов K и B:

        [ConstructorArgument("K")]
        public double K { get; set; } = 1;

        [ConstructorArgument("B")]
        public double B { get; set; }



        // Конструкторы (установим каскадным образом):
        /*Данные конструкторы просто так сейчас вызвать невозможно при формированиии объекта App.xaml в разметке.*/
        public Linear() { }         // Этот конструктор ничего делать не будет, позволив нам создавать объекты без параметров.

        public Linear(double K) => this.K = K;

        public Linear(double K, double B) : this(K) => this.B = B;



        // Методы:
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is null) return null;

            var x = System.Convert.ToDouble(v, c);
            return K * x + B;
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            if (v is null) return null;

            //var type = TypeDescriptor.AddAttributes(t);

            var y = System.Convert.ToDouble(v, c);

            return (y - B) / K;
        }
    }
}
