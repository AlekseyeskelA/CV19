﻿using CV19.Services;
using CV19.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        
        

        protected override void OnStartup(StartupEventArgs e)               // Если выполнится этот метод, то это будет означать, что приложение будет запущено.
        {
            IsDesignMode = false;
            base.OnStartup(e);

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
