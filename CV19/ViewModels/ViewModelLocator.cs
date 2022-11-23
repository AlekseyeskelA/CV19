using Microsoft.Extensions.DependencyInjection;

namespace CV19.ViewModels
{
    // Данный класс MVVM-Light, если им пользоваться, создаёт на нас автоматически.
    // Этот класс должен представлять собой сборище свойств, через которые мы будем осуществлять доступ к конкретным Вью-Моделям.
    internal class ViewModelLocator
    {
        /* Свойство для получения MainWindowViewModel. Получать её мы будем так: Мы обращаемся к хосту App.Host внутри нашего приложения, берём сервисы (.Services),
        и просим выдать нам класс сервиса MainWindowViewModel (.GetRequiredService<MainWindowViewModel>())*/
        public MainWindowViewModel MainWindowModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
    }
    // Далее компилируемся, и размещаем объект этого класса в ресурсах на уровне всего приложения в файле App.Xaml.
}
