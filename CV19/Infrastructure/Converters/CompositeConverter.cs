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
    [MarkupExtensionReturnType(typeof(CompositeConverter))]

    /* Данный конвертер можно применять для композиции, т.е., когда нужно выполнить не одно преобразование, а несколько. С помощью этого конвертера можно комбинировать
    любое количество других конвертеров между собой */
    internal class CompositeConverter : Converter
    {
        // У данного конвертера будет два свойства-конвертера, которые мы поочереди будем вызывать:
        [ConstructorArgument("First")]
        public IValueConverter First { get; set; }
        
        [ConstructorArgument("Second")]
        public IValueConverter Second { get; set; }


        // Добавим три конструктора:
        public CompositeConverter() { }
        public CompositeConverter(IValueConverter First) => this.First = First;
        public CompositeConverter(IValueConverter First, IValueConverter Second) : this(First) => this.Second = Second;


        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            // Вызываем первый конвертер, подразумевая возможность, что его может не быть "?", и вызываем метод Convert, передавая ему параметры v, t, p, c, и если его нет "?"",
            // то берём значение v, которое было:
            var result1 = First?.Convert(v, t, p, c) ?? v;
            // После этого вызываем второй конвертер:
            var result2 = Second?.Convert(result1, t, p, c) ?? result1;

            return result2;
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            var result2 = Second?.ConvertBack(v, t, p, c) ?? v;
            var result1 = First?.ConvertBack(result2, t, p, c) ?? result2;

            return result1;
        }
    }
}
