using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    /*В ряде случаев при применении расширения раметки, когда оно объявлено в базовом классе (: MarkupExtension), и при этом есть класы-наследники, которые возвращают также
    сами себя (=> this) через базовый класс, то в этом случае имеет смысл в классе-наследнике прописать, что быдет являться возвращаемым типом значения (с помощь атрибута).
    В этом случае в разметке будут видны свойства:*/
    [MarkupExtensionReturnType(typeof(Add))]
    // Имеет смысл писать несколько таких конвертеров, которые позволяют умножать на коэффициент, прибавлять значение т.д.
    internal class Add : Converter
    {
        // Укажем в разметке имя аргумента второго конструктора с помощью атрибута:
        [ConstructorArgument("K")]

        // Введём свойство, на кооторое будем домножать, и по умолчанию установим значение 1:
        public double B { get; set; } = 1;

        // Добавим пару конструкторов для удобства использования:
        // Пустой конструктор будет самым востребованным:
        public Add() { }

        // Конструктор, который позволяет устанавливать значение K:
        public Add(double K) => this.B = K;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is null) return null;

            /* value может быть чем угодно: целым, вещественным, объектом или сторкой, поэтому воспользуемся универсальным классом конвертеров Convert.
            Если это будут строки, то они также будут конвертироваться в вещественные числа. И тут мы можем дополнительно указать культуру, которую
            мы получает в качестве параметра на входе в метод Convert*/
            var x = System.Convert.ToDouble(value, c);

            return x + B;
        }

        public override object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            if (value is null) return null;

            var x = System.Convert.ToDouble(value, c);

            return x / B;
        }
    }
}
