﻿using System;
using System.Net;

namespace CV19.Web
{
    /* Класс, который был создан при добавлении решения CV19.Web мы удалили. Вместо него создали новый. Будем по возможности использовать netstandard2.0.
     * Чем ниже цифру поставить, тем более широкий охват аудитории мы получим в плане платформ, на которых можно использовать нашу библиотеку.*/    

    public class WebServer
    {
        // Событие, которое мы будем генерировать, передавая в него объект RequestRecieverEventArgs:
        private event EventHandler<RequestRecieverEventArgs> RequestReciever;

        // Web-сервер можно реализовать двумя способами: простым и сложным.

        /* Сложный способ заключается в использовании класса TcpListener. Это непосредственная реализация протокола TCP для входящих соединений.
         * С его помощью можно реализовать взаимодействие с удалённой состемой как в текстовом виде, так и в бинарном виде. Для его создания надо указать
         * диапазон адресов, которые он будет прослушивать и порт, для того, чтобы он его смог открыть. Использование TcpListener с одной стороны
         * трудоёмко в плане обвязки, т.е. нам придётся самостоятельно дописать кучу дополнительного кода, который анализирует получаемые данные в сыром
         * виде. Это сообщения протокола TCP (/IP будет отбрасываться), и мы будем получать только набор данных. Т.е. если нам нужно реализовать
         * HTTP-протокол, то нам нужно будет вручную извлекать все заголовки, отдельно тело сообщения, и их придётся анализировать вручную. Однако,
         * при этом мы получаем полный контроль над всем этим процессом, и из плюсов - у нас остаётся только проблема с файер-воллом. Т.е. файер-волл
         * будет уведомлять, что кто-то пытается открыть порт на прослушивание, и спрашивать: разрешить ему или нет? Это проблема не очень большая, если
         * только мы не работаем под ограниченной учётной записью, в которой администратор долден всё это разрешать.*/

        //private TcpListener _Listener = new TcpListener(new IPEndPoint(IPAddress.Any, 8080));

        /* Простоя способ заключается в использовании готового класса, который работает уже не на прямую с драйвером сетевой карты, а с драйвером
         * http.sys операционной системы. Но с данным классом появляется ряд трудностей. Для того, чтобы наше приложение получило разрешение на
         * использование http-моединения, смало гото, что надо правильно установить файер-волл, но ещё нужно и разрешение на доступ к самому
         * драйверу http.sys. Если работать из-под учётной записи администратора, то проблем никаких не будет (кроме файер-волл). Если мы пишем
         * службу Windows, которая обычно запускается с правами администратора, то также проблем нет (кроме файер-волл). Но если нам нужно создать
         * для десктопного приложения возможность работы через объект HttpListener, то необходимо прописывать правила, разрешать порты.*/

        private HttpListener _Listener;
        private readonly int _Port;                         // Порт, который будем прослушивать.
        private bool _Enabled;
        private readonly object _SyncRoot = new object();   // Объект блокировки. Readonly - чтобы никто не смог изменить значение этого поля.

        public WebServer(int Port) => _Port = Port;

        /* Нам понадобится обеспечить возможность запуска сервера и его остановки любое количнство раз, чтобы в любой момент времени можно было его
         * запустить и остановить, причём, чтобы можно было сделать это из нескольких потоков, т.е., если одновременно два потока попытаются запустить
         * этот сервер или остановить его, чтобы проблем не возникало.*/

        public int Port => _Port;                           // Порт менять будет нельзя.

        // Если значение Enabled будет установлено в ИСТИНУ, то буде вызван метод Start(); если в ложь - то метод Stop():
        public bool Enabled { get => _Enabled; set { if (value) Start(); else Stop(); } }

        public void Start()                                 /* Запуск сервера. Нужно учитывать, что данный метод может быть запущен, когда сервер
                                                             * уже запущен, и сервер не должен при этом пострадать.*/
        {
            /* Первая проверка. Если сервер уже включён, то мы просто выходим. Выполняется для того, чтобы лишний раз не обращаться к директиве lock,
             * потому, что lock замедляет ход выполнения программы. То есть елси сервер гарантированно запущен, и  мы снова пытаемся его запустить,
             * то это условие нас тут же выкинет из метода:*/
            if (_Enabled) return;

            if (_Enabled) return;

            /* Блокируемся на объекте блокировки критической секции, и весь внутренний код в скобках выполняется уже как критическая секция
             * В ряде случаев можно видеть, что для блокировки используется сам объект (this). Но в этом случае может возникнуть проблема:
             * наш объект кто-то может использовать для блокировки точно-также, и в этом случае у нас может возникнуть deadlock, когда
             * снаружи кто-то заблокировал наш объект в одном потоке, а в другом потоке у нас происходит процесс запуска, и он также заблокируется,
             * и могут быть проблемы. Поэтому специально создаём внутри скрытый объект и выполняем блокировку на нём:*/

            //lock (this)            
            lock (_SyncRoot)
            {
                /* Вторая проверка. Необходимо для того, что у нас двва потока могут одновременно войти в метод Start(). В этот момент флаг может
                 * быть сброшен, и они оба пройдут к директиве lock. После этого один из потоков провалится в критическую секцию, а второй метод
                 * застрянет на директиве lock. Первый поток начнёт выполнение запуска сервера, включит флаг _Enabled в значение true и
                 * выйдет из критической секции. После этого второй метод провалится внутрь, и если не будет второй блокировки, то он повторно
                 * запустит наш сервер, что, скорее всего, приведёт к ошибке:*/
                if (_Enabled) return;

                _Listener = new HttpListener();

                /* Необходимо задать префексы адресов, которые он зарегистрирует в драйвере http.sys, и будет прослушивать в последствии.
                 * http://*:{_Port} - если нам не разрешено использовать в драйвере соответствующий порт и соответствующий адрес для нашего приложения
                 * либо для нашего пользователя, то получим здесь исключение об ограничении доступа. Зашита от многократного запуска: если сервер уже
                 * запущен, то повторный вызов метода Start() нам заного запустит второй процесс прослушивания, и скорее всего он завершится
                 * падением, потому что два раза открыть один и тот же порт операционная система не даст.*/

                _Listener.Prefixes.Add($"http://*:{_Port}");
                _Listener.Prefixes.Add($"http://+:{_Port}");    // Что это значит, преподаватель не помнит.

                _Enabled = true;                            // Говорим, что вервер включён.
                ListenAsync();                              // Запускаем процесс прослушивания.

            }
        }

        public void Stop()                                  // Остановка сервера.
        {
            if (!_Enabled) return;

            lock (_SyncRoot)
            {
                if (!_Enabled) return;

                _Listener = null;                           // Обнулим ссылку на _Listner.
                _Enabled = false;                           // Остановим. Это мягкая остановка.
            }
        }

        private async void ListenAsync()
        {
            /* Захватываем внутри метода поле в локальную переменную для того, чтобы, если вдруг снаружи этого метода кто-то изменит состояние
             * этого поля, чтобы мы не потеряли ссылку на него и смогли с ним продолжить работать:*/

            var listener = _Listener;
            listener.Start();                               // Запускаем процесс прослушивания.

            // Цикл, который будет выполняться до тех пор, пока ы методе public void Stop() не будет сброшен флаг _Enabled:
            while (_Enabled)
            {
                /* Объект _Listener реализован асинхронно. Его основной метод - GetContext. Есть две вариации: это метод GetContext и GetContextAsync.
                 * Также есть две старые реализации DeginGetContext и EndGetContext с использованием тап-подхода. Воспользуемся асинхронным методом
                 * GetContextAsync() и поэтому сам метод Listen() сделаем асинхронным. На каждой итерации цикла мы извлекаем контекст из
                 * соединения listener:*/

                var context = await listener.GetContextAsync().ConfigureAwait(false);

                ProcerssRequest(context);
            }

            listener.Stop();                                // После того, как сервер будет остановлен, вызовем метод Stop() для закрытия порта.
        }

        //Метод, в котором будет выполняться обработка контекста:
        private void ProcerssRequest(HttpListenerContext context)
        {
            RequestReciever?.Invoke(this, new RequestRecieverEventArgs(context));
        }
    }

    public class RequestRecieverEventArgs : EventArgs 
    {
        public HttpListenerContext Context { get; }

        public RequestRecieverEventArgs(HttpListenerContext context) => Context = context;
        
    }
}
