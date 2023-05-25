using CV19.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CV19.ViewModels
{
    internal static class Registrator
    {
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            // Здесь зарегистрируем все Вью-Модели:
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<CountriesStatisticViewModel>();
            services.AddSingleton<WebServerViewModel>();

            return services;
        }
    }
}
