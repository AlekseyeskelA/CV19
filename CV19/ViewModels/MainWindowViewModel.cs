using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Models.Decanat;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        /*------------------------------------------------------------------------------------------------------------------------------------------*/

        // Создадим студентов. Для этого создадим коллекцию групп и заполним её в конструкторе:
        public ObservableCollection<Group> Groups { get; }

        // Создадим данные для построенияя графика:
        // Нам потребуется свойство, которое возвращает перечисление точек данных, которые быдем строить на графике. Если в последствии не планируется добавлять или удалять точки,
        // то можно просто вернуть перечисление EInumerable. Если планируется добавлять или удалять, то нужно возвращать ObservableCollection:

        #region TestDataPoints : IEnumerable<DataPoint> - Тестовый набор для визуализации
        /// <summary>Тестовый набор для визуализации</summary>
        private IEnumerable<DataPoint> _TestDataPoints;

        /// <summary>Текстовый набор для визуализации</summary>
        public IEnumerable<DataPoint> TestDataPoints
        {
            get => _TestDataPoints; 
            set => Set(ref _TestDataPoints, value); 
        }    // Возвращение такого типа будет достаточно для визуализации графиков OxyPlot (сгенерируем его в конструкторе).
             // Далее идём в разметку окна и проверяем, что в свойстве TestDataPoints что-то есть... Выберем для этого вкладку 2.
        #endregion

        #region SelectedPageIndex : int - Номер выбранной вкладки
        /// <summary>Номер выбранной вкладки</summary>
        private int _SelectedPageIndex;

        /// <summary>Номер выбранной вкладки</summary>
        public int SelectedPageIndex
        {
            get => _SelectedPageIndex;
            set => Set(ref _SelectedPageIndex, value);
        }    // Возвращение такого типа будет достаточно для визуализации графиков OxyPlot (сгенерируем его в конструкторе).
        // Далее идём в разметку окна и проверяем, что в свойстве TestDataPoints что-то есть... Выберем для этого вкладку 2.
        #endregion

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

        /*------------------------------------------------------------------------------------------------------------------------------------------*/

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


        #region ChangeTabIndexCommand  
        // Команда, чередующая вкладки по нажатию на кнопки со стрелками:
        public ICommand ChangeTabIndexCommand { get; }

        private bool CanChangeTabIndexCommandExecuted(object p) => _SelectedPageIndex >= 0;

        private void OnChangeTabIndexCommandExecuted(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }
        #endregion

        #endregion

        /*------------------------------------------------------------------------------------------------------------------------------------------*/

        // Конструктор
        public MainWindowViewModel()
        {
            // Создаём команды (объекты коменд) внутри конструктора:
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecuted);
            #endregion

            // Сгенерируем данные для тестового графика:
            var data_points = new List<DataPoint>((int) (360 / 0.1));
            for (var x = 0d; x <=360; x+=0.1)
            {
                const double to_rad = Math.PI / 180;            // Константа - это не переменная. Её можно писать там, где это необходимо.
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }    

            TestDataPoints = data_points;

            // Создаём объект ObservableCollection. Есть два способа, как набить его данными:
            // 1. СОздавать по одной группе и добавлять. Но это долго. На каждую новукю группу ObservableCollection будет вызывать у себя системe событий, что будет сильно тормозить работу.
            // Поэтому, если нужно создать большое количесво данных и тут же поместить их в объект ObservableCollection, то лучше сперва создать массив или список, а потом его передать в конструктор
            // (в круглые скобки). В этом случае всё пройдёт гораздо быстрее. Сделаем это через Лямбда-выражение:
            // Для групп создадим перечисление целых чисел от 1 в количестве 20 штук, и дальше для каждого числа сделаем преобразование (возьмём число и на его основе создадим группу).
            // Тоже самое сделаем для студентов (их будет 10):

            var student_index = 1;
            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name {student_index}",
                Surename = $"Surename {student_index}",
                Patronymic = $"Patronymic {student_index++}",   // таким образом переменная student_index будет инкрементироваться для каждого студента.
                Birthday = DateTime.Now,
                Rating = 0
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group            
            {
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students)
            });

            //Полученное перечисление скормим ObservableCollection в результате получится по 10 студентов в группе:
            Groups = new ObservableCollection<Group>(groups);

        }

        /*------------------------------------------------------------------------------------------------------------------------------------------*/

    }
}
