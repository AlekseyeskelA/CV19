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

            return services;
        }
    }
}
