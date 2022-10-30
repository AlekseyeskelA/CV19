using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    // Имеет смысл писать несколько таких конвертеров, которые позволяют умножать на коэффициент, прибавлять значение т.д.
    internal class Ratio : Converter
    {
        // Укажем в разметке имя аргумента второго конструктора с помощью атрибута:
        [ConstructorArgument("K")]

        // Введём свойство, на кооторое будем домножать, и по умолчанию установим значение 1:
        public double K { get; set; } = 1;

        // Добавим пару конструкторов для удобства использования:
        // Пустой конструктор будет самым востребованным:
        public Ratio() { }

        // Конструктор, который позволяет устанавливать значение K:
        public Ratio(double K) => this.K = K;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is null) return null;
            
            /* value может быть чем угодно: целым, вещественным, объектом или сторкой, поэтому воспользуемся универсальным классом конвертеров Convert.
            Если это будут строки, то они также будут конвертироваться в вещественные числа. И тут мы можем дополнительно указать культуру, которую
            мы получает в качестве параметра на входе в метод Convert*/
            var x = System.Convert.ToDouble(value, c);

            return x * K;
        }

        public override object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            if (value is null) return null;
            
            var x = System.Convert.ToDouble(value, c);

            return x / K;
        }
    }
}
