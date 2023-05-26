using CV19.Services.Interfaces;
using CV19.Web;
using System;
using System.IO;

namespace CV19.Services
{
    internal class HttpListenerWebServer : IWebServerService
    {
        /* Для возможности запуска CV19.Web/WebServer из нашего проекта, предварительно нужно в в папке CV19/Зависимости
         * добавить ссылку на проект и поставить галочку напротив CV19Web*/

        private WebServer _Server = new WebServer(8080);
        
        public bool Enabled { get => _Server.Enabled; set => _Server.Enabled = value; }

        public void Start() => _Server.Start();

        public void Stop() => _Server.Stop();

        // Для того, чтобы сервер нам что-то отвечал при его запуске, добавляем конструктор и подписываемся на событие RequestRecieved:
        public HttpListenerWebServer() => _Server.RequestRecieved += OnRequestRecieved;

        private static void OnRequestRecieved(object sender, RequestRecieverEventArgs e)
        {
            using var writer = new StreamWriter(e.Context.Response.OutputStream);
            writer.WriteLine("CV-19 Application - " + DateTime.Now);
        }
    }
}
