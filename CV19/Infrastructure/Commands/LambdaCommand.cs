using System;
using CV19.Infrastructure.Commands.Base;

namespace CV19.Infrastructure.Commands
{
    // После того, как создана базовая команда, создадим команду, которой легко пользоваться и которая будет основной рабочей лошадкой.
    // Назовём её LambdaCommand, так как она внутри использует Lambda-синтаксис. Также она может носить название RelayCommand или ActionCommand.
    internal class LambdaCommand : Command
    {
        // Члены класса:
        // Поля, помеченные readonly, будут работать быстрее, чем поля, не помеченные readonly, поэтому, если есть возможность добавить readonly,
        // то лучше делать это, так как это немножко ускорит работу приложения.
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        // Конструктор:
        // В конструкторе надо получить два делегата: один, который будет выполняться методом CanExecute, а второй - методом Execute,
        // то есть указать два действия, которые команда может выполнять.
        // Команды могут и разметки получать параметры. В качестве параметра может быть что угодно, поэтому делегат Action<> получает параметр типа object.
        // В последствии мы должны его преобразовывать к нужному нам виду, либо будет null, если ничего не было передано.
        // CanExecute = null, чтобы можно было не указывать второй параметр: Func<object, bool> CanExecute = null.
        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            // Сделаем дополнительную проверку. Ругаемся если нам не переали ссылку в Action<object> Execute на делегат:
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }

        // Переопределяем методы:
        // Реализация метода CanExecute. Вызываем метод CanExecute, подразумевая, что там может быть пустая ссылка, и если нет этого делегата,
        // то считаем, что команду можно выполнить:
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;

        // Реализация метода Execute. Вызываем метод Execute и передаём в него параметр:
        public override void Execute(object parameter) => _Execute(parameter);
    }
}
