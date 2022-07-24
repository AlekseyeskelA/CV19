using CV19.Infrastructure.Commands;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    // Основная модель представления для главного окна.
    internal class MainWindowViewModel : ViewModel
    {
        //// При желании можно переопределить метод Dispose и освободить какие-то ресурсы, которые модель захватит вдруг:
        //protected override void Dispose(bool Disposing) { base.Dispose(Disposing); }

        #region Заголовок окна
        // В каждом свойстве должен быть такой код:
        private string _Title = "Анализ статистики CV19";

        // Описание с тегами <summary> (нажать три раза "///"). Таким обрвзом в самом окне в разметке xaml при наведении мышки на Binding Свойство будет видно, что это такое:
        /// <summary>Заголовок окна</summary>
        public string Title
        {            
            get => _Title;
            //set
            //{
            //    if (Equals(_Title, value)) return;
            //    _Title = value;
            //    OnPropertyChanged();
            //}

            // Так как у нас в ViewModel есть специальный метод Set, то код можно сократить:
            //set
            //{
            //    Set(ref _Title, value);
            //}

            // Или ещё проще:
            set => Set(ref _Title, value); // если бы во ViewModel не было бы написано "[CallerMemberName] ... = null", то третьим аргументом здесь пришлось бы написать "Title".
        }
        #endregion


        #region Status : string - Статус программы
        /// <summary>Статус программы</summary>
        private string _Status = "Готов!";

        /// <summary>Статус программы</summary>
        public string Status
        { 
            get => _Status; 
            set => Set(ref _Status, value); 
        }
        #endregion

        #region Команды


        #region CloseApplicationCommand        
        // Команда, закрывающая программу:
        // Название свойства должно отображать его функционал, а в конце нужно написать "Command", чтобы отличать команду от обычного свойства.
        public ICommand CloseApplicationCommand { get; }





        // Методы (что команда должна делать):
        //  Данный метод будет выполняться в момент, когда команда выполняется:
        private void OnCloseApplicationCommandExecuted(object p)
        {
            // Обращаемся к классу Application из пространсва имён System.Windows, внутри вызываем текущее наше приложение (Current), у которого вызываем метод Shutdown():
            Application.Current.Shutdown();
            // Далее идём в разметру и привязываемся везде, где нужна функциональность закрытия окна.
        }
        // Данная команда будет доступна для выполнения всегда, поэтому возвращаем "true":
        private bool CanCloseApplicationCommandExecute(object p) => true;
        #endregion





        #endregion
        // Конструктор
        public MainWindowViewModel()
        {
            // Создаём команды (объекты коменд) внутри конструктора:
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion
        }
    }
}
