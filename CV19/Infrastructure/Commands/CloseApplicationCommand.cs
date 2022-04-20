using System.Windows;
using CV19.Infrastructure.Commands.Base;

namespace CV19.Infrastructure.Commands
{
    // Пример создания команды (например, закрытия приложения) в отдельном классе:
    // Команды можно описывать как внутри View-Model, так и отдельными классами, которые реализуют свою функциональность.
    // Для того, чтобы эта команда работала, нужно добавить соответствующее пространство имён в Xaml-разметку окна.
    // Перед этим нужно обязательно сделать компиляцию, так как иначе xaml-разметка не найдёт и не предложит данную команду.
    internal class CloseApplicationCommand : Command
    {
        // Реализуем базовый класс:
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
