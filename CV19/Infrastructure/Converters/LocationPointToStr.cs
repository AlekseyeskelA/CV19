using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CV19.Infrastructure.Converters
{
    // Так как мы всё делаем в одном проекте, то можно здесь писать internal. Если имеется много проектов, то целесообразно такие инструменты вынести в отдельную сборку,
    // и тогда класс должен быть публичным.
    internal class LocationPointToStr : Converter
    {
        /* Конвертер представляет собой класс, в котором реализуются два метода: Convert и ConvertBack. То есть, когда привязка узнает о том, что источник изменился,
        и необходимо изменить целевое свойство, к которому выполнена привязка, она берет значение из източника, далее смотрит, указан ли у него конвертер.
        Если указан, то она у конвертера вызывается метод Convert и передаёт в object value значение, полученное от источника. Цель работы конвертера - получить
        новое значение public object, которое привязкой установит как как результат своей работы для целевого свойства.
        Если привязка обнаруживает, что изменилось целевое свойство, то она извлекает значение из целевого свойства и вызывает у конвертера мотод ConvertBack,
        задачей которого является выполнить обратное преобразование и получить исходное значение. При этом метод обаметода (Convert и ConvertBack) могут сгенерировать исключение
        NotSupportedException, и в этом случае привязка не установит значение и ничего не сделает, то есть это никак не повлияет на роботоспособность прилоджения,
        но при этом можно запретить выполнение передачи данных привязкой в одну или в другую сторону.*/
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        // Немного подкорректируем стандартные методы:
        /* Дополнительные параметры, укоторые передаются (Type t, object p, CultureInfo c), можно зачастую совсем не анализировать.
        Type t - тип целевого свойства, к которому выполнена привязка. Можно проанализировать, посмотреть, во что конвертировать данные.
        object p - этот параметр передаётся в привязке. У привязки в xaml-разметке есть ещё ConverterParanetr, через который можно что-то передать внутрь конвертера.
        CultureInfo c - текущая культура интерфейса, которая позволяет узнать, например, формат строк при конвертации из чисел или в числа, т.е. если используется 
        английская культура, то разделителем целой и вещественной части является ".", если русская культура, то это ",", и всё это можно учесть в процессе конвертации значений,
        просто передав параметр в метод коныертации, и он сам сделает всё остальное автоматически.*/

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            // Здесь мы знаем, что типом является класс Piont, поэтому сделаем такое преобразование:
            if (!(value is Point point)) return null;
            return $"Lat:{point.X};Lon:{point.Y}";
        }

        /*Второq метод ConvertBack - это обратное преобразование. Здесь типом Type t является тип свойства источника. Опять-таки всё остальное (Type t, object p, CultureInfo c)
         можно не анализировать, а использовать только значение object value*/
        public override object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            //// В обратную сторону у нас преобразование не будет поддерживаиться, поэтому даём исключение:
            //throw new NotSupportedException();
            if (!(value is string str)) return null;

            var components = str.Split(';');
            var lat_str = components[0].Split(':')[1];
            var lon_str = components[1].Split(':')[1];

            var lat = double.Parse(lat_str);
            var lon = double.Parse(lon_str);

            return new Point(lat, lon);
        }
    }
}
