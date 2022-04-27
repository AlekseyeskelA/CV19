using System;
using System.Net;               // Для WebClient
using System.Net.Http;          // Для HttpClient

namespace CV19Console
{
    // Прежде, чем начинать делать визуальный интерфейс, имеет смысл обкатать логику в управляемых условиях тестового стенда, который будет реализован в виде консоли:
    internal class Program
    {
        // Сохраняем адрес с данными о COVID19 из GitHub института Джона Хопкинса:
        private const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        
        static void Main(string[] args)
        {
            // Нужно скачать эти данные, убедиться, что они выгружаются с сервера, и что их можно как-то обработать, например, разобрать по строчкам, распарсить в CSV-файл.
            // Для этой цели понадобится класс, который может отправлять http-запросы. В старых версиях .Net Framework был класс WebClient. Он до сих пор есть, так как поддерживается
            // обратная совместимость версий. С его помощью можно скачивать содержимое с любых сайтов. Он поддерживает cookies, сеансы и пр.:
            //WebClient client = new WebClient();

            // Но в современном .NetCore он был переписан, и выла создана более эффективная его версия, соторая называется HttpClient():
            var client = new HttpClient();

            // Сперва просто скачает текст:
            var response = client.GetAsync(data_url).Result;                // Заибираем информацию с сервера.
            var csv_str = response.Content.ReadAsStringAsync().Result;      // Читаем контент и переводим в строку.

            // Разберёмся со структурой файла. Для этого его скачаем и откроем в Visual Studio Code и нажать в нём внизу Align.
            // Далее попробуем распарсить данные.

            Console.ReadLine();
        }
    }
}
