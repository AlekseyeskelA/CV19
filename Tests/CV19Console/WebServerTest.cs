using CV19.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CV19Console
{
    internal static class WebServerTest
    {
        public static void Run()
        {
            var server = new WebServer(8080);
            //server.RequestRecieved += OnRequestRecieved;

            server.Start();

            Console.WriteLine("Сервер запущен!");
            Console.ReadLine();
        }

        //private static void OnRequestRecieved(object? Sender, RequestRecieverEventArgs E)
        //{
        //    var context = E.Context;            // Получаем наш контекст.

        //    Console.WriteLine("Connection {0}", context.Request.UserHostAddress);

        //    /* Внутри контекста, коороый мы получаем есть два основных объекта: Request и Response. Request - это то, что нам прислали. Response - объект,
        //     * который мы формируем для того, чтобы отправить его клиенту. Внутри Response есть OutputStream, в который мы можем что-то написать*/

        //    using var writer = new StreamWriter(context.Response.OutputStream);
        //    writer.WriteLine("Hello from Test Web Server!!!");
        //}
    }
}
