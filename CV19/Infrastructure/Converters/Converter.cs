using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CV19.Infrastructure.Converters
{
    // Так как конвертеров у нас много, то берём и создаём базовый класс конвертера:
    internal abstract class Converter : IValueConverter
    {
        // Сделаем верхний метод абстрактным, а нижний - виртуальным.
        public abstract object Convert(object v, Type t, object p, CultureInfo c);

        public virtual object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotSupportedException("Обратное преобразование не поддеривается");
        // Теперь, если назледник этого класса не переопределит метод ConvertBack, то автоматически обратное преобразование поддерживаться не будет.
    }
}
