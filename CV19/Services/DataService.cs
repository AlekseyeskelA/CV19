﻿using CV19.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Services
{
    internal class DataService
    {
        /* Метод: выдаёт информацию по всем странам. При вызове этого метода он должен будет провести все манипуляции в файле Programs.cs.
        Путём извлечения данных с удалённого сервера он должен будет получить информацию, а дальше создать на основе каждого пункта, полученного в
        var russia_data = GetData().First(v => v.Country.Equals("Russia", StringComparison.OrdinalIgnoreCase)) он должен будет создать элементы CountryInfo.
        Перенесём сюда код из CV19Console/Programs:*/

        // Сохраняем адрес с данными о COVID19 из GitHub института Джона Хопкинса:
        private const string _DataSourceAddress = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        // Метод 1. Формирует поток байт данных. В его основе лежит специальный класс HttpClient, который позволяет делать удалённые запросы к серверам по указанным адресам.
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();                                                                          // Создаём клиент.
            var response = await client.GetAsync(_DataSourceAddress, HttpCompletionOption.ResponseHeadersRead);     // Получаем ответ от удалённого сервера. Заибираем информацию с сервера.
                                                                                                                    // HttpCompletionOption.ResponseHeadersRead - конфигурируем таким образом, чтобы не скачивалось
                                                                                                                    // всё содержимое: нам интересно изначально знать только заголовки http-запроса, а всё тело
                                                                                                                    // запроса пусть остаётся пока в сетевой карте, и она пусть его пока не трогает.
                                                                                                                    // В этом заголовке будет написана длина дередаваемых данных, параметры, и другая тех-информация.
            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);    //!!!!!!!!!!!!!!!!                                                  // Возвращаем поток, из которого мы сможем читать текстовые данные.
                                                                                                                    // response - ответ, Content - его содержимое, ReadAsStreamAsync() - просим выдать содержимое
                                                                                                                    // в виде потока данных объекта Stream, который мы будем читать, извлекая оттуда байт а байтом,
                                                                                                                    // и скачивая их сетевого соединения. Тем самым мы обеспечим незамусоренность паяти.
        }


        // Метод 2. Разбивает поток на последовательность строк:
        // Разбиваем текстовые данные на строки (чтобы каждая строка извлекалась отдельно)
        // Для этого заведём метод, который будет возвращать перечисление строк.
        // Сделаем его синхронным, чтобы не перегружать... мозги? Будем возвращать интерфайс IEnumerable строк:
        private static IEnumerable<string> GetDataLines()                                              // В месте вызова этого метода мы можем в любой момент прервать процесс чтения,
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
            // В данной строчке мы получили Deadlock. Deadlock - это когда два потока, один поток запускает другой поток, и ждёт, пока второй поток завершится, второй поток что-то делает, а потом
            // начинает ждать, пока первый поток освободится. И в результате они начинают ждать друг друга до завершения всего приложения аварийным способом. Deadlock обычно ничего хорошего не несёт,
            // и в процессе отладки приложения с ними бороться очень трудно. Проблема в том, что здесь есть смешение асинхронного (private static async Task<Stream> GetDataStream()) и синхронного
            // (private static IEnumerable<string> GetDataLines()) кода, нужно от этого избавиться.
            using var data_stream = GetDataStream().Result;                                            // Внутри метода мы должны получить поток. Здесь произойдет запрос с сервера. Сервер ответит,
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
                var line = data_reader.ReadLine();                                                     // Пока мы не дошли до конца потока, мы извлекаем из data_reader очередную строку и помещаем её в переменную.                
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


        // Метод 4. Получи данные по заражённым по каждой стране и каждой провинции:
        //// Заменим кортежи на объект PlaceInfo
        //private static IEnumerable<PlaceInfo> GetContriesData()   // int[] Counts - количество заражённых в каждый момент времени.
        // Бкудем использовать кортежи. Кортежи появились в .NetCore, и позволяют быстро на лету в нужном нам месте определить структуру данных с нужным набором свойств.
        // При этом, кортеж отличается от анонимного класса тем, что это структура (Она создаётся на стеке нашего вызова и не тредует работы сборщика мусора. Есть и ещё ряд преимуществ (не сказал))
        private static IEnumerable<(string Province, string Country, (double Lat, double Lon) Place, int[] Counts)> GetCountriesData()   // int[] Counts - количество заражённых в каждый момент времени.
        {
            var lines = GetDataLines()
                .Skip(1)                                                            // Отбрасываем заголовки.
                .Select(line => line.Split(','));                                   // Берём каждую строку и вызываем для неё Split. Получаем перечисление массивов строк, где каждый элемент - это ячейка таблицы в троковом виде.

            // Теперь полученные данные нужно преобразовать в нужный нам кортеж. Выделим сперва все данные в переменные, а потом сгруппируем их в кортеж и вернём его:
            foreach (var row in lines)
            {
                var province = row[0].Trim();                                       // Провинция. У каждой строки вызываем метод Trim, который будет обрезать всё лишнее (пробелы, нечитаемые спец-символы).
                var country_name = row[1].Trim(' ', '"');                           // Для названия страны нужно указать, что конкретно мы хотим удалить (пробелы и ковычки).
                var latitude = double.Parse(row[2]);                                // Добавим информацию о широте и долготе.
                var longitude = double.Parse(row[3]);
                var counts = row.Skip(4).Select(int.Parse).ToArray();               // Первые 4 параметра (страна, провинция, широта, долгота) пропускаем, после чего каждый из элементов превращаем в целое число.

                yield return (province, country_name, (latitude, longitude), counts);
            }
        }


        public IEnumerable<CountryInfo> GetData()
        {
            var dates = GetDates();                                                 // Получаем даты каждого из элементов данных.
            var data = GetCountriesData().GroupBy(d => d.Country);                  // Получаем данные которые группируем по названию страны.

            foreach (var country_info in data)                                      // Перебираем полученные группы
            {
                var country = new CountryInfo
                {
                    Name = country_info.Key,
                    ProvinceCounts = country_info.Select(c => new PlaceInfo
                    {
                        Name = c.Province,
                        Location = new System.Windows.Point(c.Place.Lat, c.Place.Lon),
                        Counts = dates.Zip(c.Counts, (date, count) => new ConfirmedCount { Date = date, Count = count })
                    })
                };
                yield return country;
            }
        }
        // Далее нужно протестировать, как это будет работать. Поэтому запустим тест в App.xaml.cs. В этом месте это делать не правильно, но всё же...
    }
}
