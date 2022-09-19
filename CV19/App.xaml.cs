using CV19.Services;
using System.Linq;
using System.Windows;

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
        }
        // Проверим в xaml-разметке, работает ли флажок IsDesignMode:
    }
}
