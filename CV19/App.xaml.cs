using CV19.Services;
using CV19.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CV19
{
    public partial class App : Application
    {
        // Надо заметить. что дизайнер не предназначан для больших нагрузок по отображению огромного количества студентов. Поэтому, чтобы он не сломался, добавим в приложение в App.xaml.cs
        // специальное свойство, которое будет опрпделять, работаем ли мы в дизайнере, или запущен exe-файл.
        public static bool IsDesignMode { get; private set; } = true;       // По умолчанию установим true.


        // Запуск Хоста при старте приложения и остановка его при выходе.
        // Внутри приложения надо создать хост, который и будет работать:
        private static IHost __Host;
        public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        /* Теперь, имея класс App, мы можем через статическое свойство Host всегда обратиться к этому самому хосту. При первом обращении он будет создан (__Host ??= Program...),
        будут сконфигурированы все его сервисы, и мы сможем им пользоваться.*/
        /* Теперь нам нужно его запустить. Для этого на понадобятся методы OnStartup (был создан ранее) и метод OnExit. В методе OnStartup мы должны стартовать хост,
         а в методе OnExit должны его остановить и разрушить:*/
        protected override async void OnStartup(StartupEventArgs e)          // Если выполнится этот метод, то это будет означать, что приложение будет запущено.
        {
            IsDesignMode = false;
            var host = Host;                                                // Забираем host из нашего свойства.
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);                        // Запускаем host. Host запускается в асинхронном режиме, поэтому и метод делаем асинхронным.
                                                                            // .ConfigureAwait(false) - для того, чтобы не получить м1ртвую блокировку.

            //// Запустим тест для проверки кода в DataService.cs В этом месте это делать не правильно, но всё же...:
            //var service_test = new DataService();

            //var countries = service_test.GetData().ToArray();               // Извлечём все данные и сложим их в массив стран.

            //// Пример заморозки объекта:
            //var brush = new SolidColorBrush(Colors.White);
            //brush.Freeze();
            //// Только после клонирования объекта можно будет изменять его свойства:
            //brush.Clone();

            //brush.IsFrozen

            // Проверим в xaml-разметке, работает ли флажок IsDesignMode.
        }

        protected override async void OnExit(ExitEventArgs e)
        { 
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);                   // Останавливаем host.
            host.Dispose();                                                 // Разрушаем host.
            __Host = null;                                                  // Почистим переменную.
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            // Добавим все сервисы, которые нам потребуются:
            services.AddSingleton<DataService>();
            services.AddSingleton<CountriesStatisticViewModel>();

            /* После того, как контейнер сервисов будет набит под завязку теми сервисами, которые нам понадобятся, коллекция сервисов IServiceCollection services будет скомпилирована
            уже в сервис-менеджера, который позволит получать сервисы из него просто по их типам. Т.е. мы укажем, что нам нужен сервис, например, с типом DataServices, и нам будет
            выдан объект этого типа.*/
        }

    }
}
