using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    // Так как конвертеров у нас много, то берём и создаём базовый класс конвертера:
    /*Для использования конвертеров вместе с расширением разметки дополнитеьно наследуем его от MarkupExtension. Далее мы идём в разметку, и в том месте, где нужно вставить
    конвертер, мы прямо его и используем в фигурных скобках. В этом случае конвертер создаётся прямо на месте и используется. И таким образом конвертер становится частью разметки,
    и его нет необходимости добавлять в ресурсы. Но при этом надо понимать, что конвертер создается всякий раз, когда применяется расширение разметки.
    И в этом нет необхоимосьи, если конвертеров у нас много.*/
    /*В ряде случаев при применении расширения раметки, когда оно объявлено в базовом классе (: MarkupExtension), и при этом есть класы-наследники, которые возвращают также
     сами себя (=> this) через базовый класс, то в этом случае имеет смысл в классе-наследнике прописать, что быдет являться возвращаемым типом значения (с помощь атрибута).
    В этом случае в разметке будут видны свойства.*/
    internal abstract class Converter : MarkupExtension, IValueConverter
    {
        // реализация MarkupExtension (возвращаем сами себя => this):
        public override object ProvideValue(IServiceProvider sp) => this;

        // Сделаем верхний метод абстрактным, а нижний - виртуальным.
        public abstract object Convert(object v, Type t, object p, CultureInfo c);

        public virtual object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotSupportedException("Обратное преобразование не поддеривается");
        // Теперь, если назледник этого класса не переопределит метод ConvertBack, то автоматически обратное преобразование поддерживаться не будет.
    }
}
