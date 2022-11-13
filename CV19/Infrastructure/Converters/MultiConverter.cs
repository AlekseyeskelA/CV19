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
    [MarkupExtensionReturnType(typeof(MultiConverter))]

    // Мультиконвертеры используются со специальным видом привязки MultiBinding. Это специальный вид привязки, который позволяет выполнить привязку к нескольким значениям.
    // Используем этот класс для преобразования данных в массив:
    internal abstract class MultiConverter : IMultiValueConverter
    {
        // Методы отличаются от обычных тем, что в методе Convert мы принимаем массив значений object[] values и один целевой тип Type targetType, и должны вернуть
        // один целевой тип.
        // А в методе ConvertBack мы принимаем одно значение object value, множество типов Type[] targetTypes и должны вернуть множество объектов.
        public abstract object Convert(object[] vv, Type targetType, object p, CultureInfo c);

        public virtual object[] ConvertBack(object v, Type[] tt, object p, CultureInfo c) =>
            throw new NotSupportedException("Обратное преобразование не поддерживается");
    }
    /* Часто мультиконвертер можно увидеть, когда нужно из неснольких параметров скомпоновать одну сущность. Например, у неё может быть несколько свойств, и каждое войство
    жнопривязать к чему-то в интерфейсе с помощью мутьлибиндинга, и дальше с помощью конвертера конвертировать этот набор свойств в какой-то объект (массив или что-то другое). */
}
