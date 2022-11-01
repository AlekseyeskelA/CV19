using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CV19.Infrastructure.Converters
{
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
}
