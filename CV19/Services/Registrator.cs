using CV19.Models.Decanat;
using CV19.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CV19.Services
{
    /* Добавим немного синтаксического для упрощения процесса регистрации всех сервисов всех Вью-Моделей. Для этого возьмём интерфейс IServiceCollection ,
     * и для него напишем методы расширения (изначально методы возьмём из из файла App.xaml.cs) в двух новых местах (в данном новом классе и в классе Registrator 
     * в папке ViewModels, в котором зарегистрируем все Вью-Модели), в которых выполним регистрацию всех сервисов (пока сервис у нас один), необходимых нашему приложению.
     * Это позволит упростить нам код в файле App.xaml.cs*/
    internal static class Registrator
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDataService, DataService>();             // Теперь любой, кто запросит интерфейс IDataService будет иметь дело с реализацией сервиса DataService.
            //services.AddTransient<IDataService, DataService>();
            //services.AddScoped<IDataService, DataService>();
            
            services.AddTransient<IAsyncDataService, AsyncDataService>();   // Регистрируем новый сервис и подключим его во главной Вью-Модели.
            services.AddTransient<IWebServerService, HttpListenerWebServer>();

            /* Так как у нас хранидище данных в памяти, то создаём сервис Singlton, который будет создан для всего приложения как один объект: */
            services.AddSingleton<StudentsRepository>();
            services.AddSingleton<GroupsRepository>();

            return services;
        }
    }
}
