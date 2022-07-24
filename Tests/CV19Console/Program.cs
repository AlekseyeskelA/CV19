﻿using System;
using System.Collections.Generic;   // Для IEnumerable. Основная идея интерфейса перечисления в том, что можно создавать ленивые методы, которые не делают всю работу, если она не нужна
using System.IO;                    // Для Stream
using System.Net;                   // Для WebClient
using System.Linq;                  // Мы используем интерфейс IEnumerable для этого проастранства имён. То есть мы будем обрабатывать csv-файл при помощи команд языка Linq. Тем самым создадим конвеер для обработки данных.
using System.Net.Http;              // Для HttpClient
using System.Threading.Tasks;       // Для Task
using System.Globalization;         // Для CultureInfo

namespace CV19Console
{
    // Прежде, чем начинать делать визуальный интерфейс, имеет смысл обкатать логику в управляемых условиях тестового стенда, который будет реализован в виде консоли:
    internal class Program
    {
        // Сохраняем адрес с данными о COVID19 из GitHub института Джона Хопкинса:
        private const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        static void Main(string[] args)
        {
            //    // Нужно скачать эти данные, убедиться, что они выгружаются с сервера, и что их можно как-то обработать, например, разобрать по строчкам, распарсить в CSV-файл.
            //    // Для этой цели понадобится класс, который может отправлять http-запросы. В старых версиях .Net Framework был класс WebClient. Он до сих пор есть, так как поддерживается
            //    // обратная совместимость версий. С его помощью можно скачивать содержимое с любых сайтов. Он поддерживает cookies, сеансы и пр.:
            //    //WebClient client = new WebClient();

            //    // Но в современном .NetCore он был переписан, и выла создана более эффективная его версия, соторая называется HttpClient():
            //    var client = new HttpClient();                                  // Создаётся клиент.

            //    // Сперва просто скачает текст:
            //    var response = client.GetAsync(data_url).Result;                // Заибираем информацию с сервера.
            //    var csv_str = response.Content.ReadAsStringAsync().Result;      // Читаем контент и переводим в строку.

            //    Console.ReadLine();
            //    // Разберёмся со структурой файла. Для этого его скачаем и откроем в Visual Studio Code и нажать в нём внизу Align.
            //    // Далее попробуем распарсить данные. Поэтому переделаем наш код: разобьём его на структурные части и будем обрабатывать данные поэтапам:
            //    // Создадим метод, который будет возвращать поток, из которого можно будет читать данные, т.е. нужно сделать так, что когда мы отправляем запрос к серверу,
            //    // а сервер нам отвечает и если файл огромный, мы его весь к себе не скачивали, а извлекали те данные, которые необходимы. И если нам понадобится,
            //    // чтобы мы могли прервать процесс извлечения данных из сети, и при этом память наша не засорилась. Для этого создадим асинхронный метод GetDataStream(), который вернёт нам поток Stream,
            //    // из которого можно считать текстовые данны (см. далее метод private static async Task<Stream> GetDataStream()).

            //foreach (var data_line in GetDataLines())
            //    Console.WriteLine(data_line);

            
            // Проверим. Данные доолжны извекаться и выводиться на консоль.

            // Далее делаем метод 3.

            // Проверим, как работает получение данных:
            //var dates = GetDates();
            //Console.WriteLine(String.Join("\r\n", dates));

            // Далее делаем метод 4, извлекающий данные по всем странам. Т.е. мы на сервер будем отправлять два запроса и получать два ответа. 
        
            // Далее будем делать запросы к этим данным:
            var russia_data = GetData()
                .First(v => v.Country.Equals("Russia", StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (date, count) => $"{date:dd:MM} - {count}")));

            // Итак, ядро бизнес-логики определено. Не будем сейчас включать эту логику в наши программу, а просто посмотрим с тестовыми данными, как их можно визуализировать простыми способами
            // и создадим модель данных.
            // Для этого в папке Models создаём структуру моделей данных, из которых у нас всё будет устроено: ConfirmedCount.cs, CountryInfo.cs, DataPoint.cs, PlaceInfo.cs, ProvinceInfo.cs.
            // А для построения графиков установим пакет OxyPlot.WPF в проект CV19.

            Console.ReadLine();
        }


        // Метод 1. Формирует поток байт данных:
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();                                                              // Создаём клиент.
            var response = await client.GetAsync(data_url, HttpCompletionOption.ResponseHeadersRead);   // Получаем ответ от удалённого сервера. Заибираем информацию с сервера.
                                                                                                        // HttpCompletionOption.ResponseHeadersRead - конфигурируем таким образом, чтобы не скачивалось
                                                                                                        // всё содержимое: нам интересно изначально знать только заголовки http-запроса, а всё тело
                                                                                                        // запроса пусть остаётся пока в сетевой карте, и она пусть его пока не трогает.
            return await response.Content.ReadAsStreamAsync();                                          // Возвращаем поток, из которого мы сможем читать текстовые данные.
                                                                                                        // response - ответ, Content - контент, ReadAsStreamAsync() - поток.
        }


        // Метод 2. Разбивает поток на последовательность строк:
        // Разбиваем текстовые данные на строки (чтобы каждая строка извлекалась отдельно)
        // Для этого заведём метод, который будет возвращать перечисление строк.
        // Сделаем его синхронным, чтобы не перегружать... мозги? Будем возвращать интерфайс IEnumerable строк:
        private static  IEnumerable<string> GetDataLines()                                              // В месте вызова этого метода мы можем в любой момент прервать процесс чтения,
                                                                                                        // этом весь хвост, оставшийся непрочитанным в память не попадет, и процесс скачивания данных
                                                                                                        // из сети прервётся. То есть если бы мы внутри завели переменную типа массив, туда извлекли
                                                                                                        // все строки, а потом вернули бы этот массив, у нас бы выделилась вся память целиком под хранение
                                                                                                        // всех данных, которые мне собрался отправить сервер. А если он не прекратит это делать,
                                                                                                        // или файл будет на несколько гигабайт, то вся оперативка будет забита, и приложение может упасть,
                                                                                                        // если, например, объём выделяемой памяти превысит 2 Гб (нельзя выделить во Framework
                                                                                                        // массив объёмом > 2 Гб).
                                                                                                        // При таком подходе мы создаём не обычный метод. Этот метод компилятором будет преобразован в
                                                                                                        // специальный объект-генератор (yield return line) который мы сможем использовать в Main (см. foreach (var data_line in GetDataLines())):
        {

            using var data_stream =  GetDataStream().Result;                                            // Внутри метода мы должны получить поток. Здесь произойдет запрос с сервера. Сервер ответит,
                                                                                                        // и HttpClien скачает только заголовок ответа. ПРи этом всё тело ответа останется пока не принятым.
                                                                                                        // То есть оно либо зависнет в буфере сетевой карты, либо сервер просто остановит передачу данных.
                                                                                                        // После этого ответ "response" вернёт нам поток "Stream", из которого мы сможем читать данные
                                                                                                        // буквально побайтно, и здесь мы его захватываем.
            using var data_reader = new StreamReader(data_stream);                                      // На его основе создать объект, который будет читать из этого потока строковые данные (построчно),
                                                                                                        // и начнёт читать этот поток байт за байтом. При этом мы считываем одну строчку и возвращаем
                                                                                                        // её как результат (строка: yield return line;)

            // После чего читаем данные, пока не встретится конец потока:
            while (!data_reader.EndOfStream)
            {                
                var line =  data_reader.ReadLine();                                                     // Пока мы не дошли до конца потока, мы извлекаем из data_reader очередную строку и помещаем её в переменную.                
                if (string.IsNullOrWhiteSpace(line)) continue;                                          // После чего проверим, не пуста ли строка, и если пуста, то переходим на слкдующий шаг цикла.
                yield return line
                    .Replace("Korea,", "Korea -")
                    .Replace("Bonaire,", "Bonaire -")
                    .Replace("Saint Helena,", "Saint Helena -");                                        // Данный метод будет у нас генератором. Так как Северная корея и ещё ряд названий даны в файле с зяпятой, а запятая у нас далее является разделителем, то сделаем локальную замену.
            }            
        }


        // Метод 3. Выделяет необходимые нам данные (получает все даты, по которым будут разбиты данные):
        // Распарсим первую строку, которую мы получаем, извлечём из неё все даты и сделаем массив дат, по которому мы будем работать.
        private static DateTime[] GetDates() => GetDataLines()                      // Получим перечисление строк всего запроса.
            .First()                                                                // Нас интересуе только перва cтрока с заголовками. Получаем массив строк, где каждая строка содержит заголовок колонки csv-файла.
            .Split(',')                                                             // Разбиваем строку.
            .Skip(4)                                                                // Отбросим первые 4 заголовка с названиями провинции, страны, широты и долготы.
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))           // Оставшиеся строки преобразуем в формат DateTime (троку s - в формат DateTime пут1м её разбора и
                                                                                    // указываем культуру CultureInfo.InvariantCulture (она помогает правильно преобразовать)).
            .ToArray();                                                             // Результат преобразуем в массив.

        // Проверим, что всё работает (см. var dates = GetDates(); в Main)


        // Метод 4. Получи данные по заражённым по каждой стране и каждой провинции:
        // Бкудем использовать кортежи. Кортежи появились в .NetCore, и позволяют быстро на лету в нужном нам месте определить структуру данных с нужным набором свойств.
        // При этом, кортеж отличается от анонимного класса тем, что это структура (Она создаётся на стеке нашего вызова и не тредует работы сборщика мусора. Есть и ещё ряд преимуществ (не сказал))
        private static IEnumerable<(string Country, string Province, int[] Counts)> GetData()   // int[] Counts - количество заражённых в каждый момент времени.
        {
            var lines = GetDataLines()
                .Skip(1)                                                            // Отбрасываем заголовки.
                .Select(line => line.Split(','));                                   // Берём каждую строку и вызываем для неё Split. Получаем перечисление массивов строк, где каждый элемент - это ячейка таблицы в троковом виде.
                
            // Теперь полученные данные нужно преобразовать в нужный нам кортеж. Выделим сперва все данные в переменные, а потом сгруппируем их в кортеж и вернём его:
            foreach (var row in lines)
            {
                var province = row[0].Trim();                                       // У каждой строки вызываем метод Trim, который будет обрезать всё лишнее (пробелы, нечитаемые спец-символы).
                var country_name = row[1].Trim(' ', '"');                           // Для названия страны нужно указать, что конкретно мы хотим удалить (пробелы и ковычки).
                var counts = row.Skip(4).Select(int.Parse).ToArray();               // Первые 4 параметра (страна, провинция, широта, долгота) пропускаем, после чего каждый из элементов превращаем в целое число.

                yield return (country_name, province, counts);                      // Возвращаем все эти данные в виде кортежа.
            }
        }
    }
}
