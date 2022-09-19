using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Models.Decanat;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CV19.ViewModels
{
    // Основная модель представления для главного окна.
    internal class MainWindowViewModel : ViewModel
    {
        //// При желании можно переопределить метод Dispose и освободить какие-то ресурсы, которые модель захватит вдруг:
        //protected override void Dispose(bool Disposing) { base.Dispose(Disposing); }

        /*------------------------------------------------------------------------------------------------------------------------------------------*/



        #region SelectedGroupStudents
        // Реализуем второй фильтр для студентов через модель-представления.
        // Для этого создадим свойство, которое будет возвращать тип (интерфейс) ICollectionView, то есть представдления.
        private readonly CollectionViewSource _SelectedGroupStudents = new CollectionViewSource();  // СОздаём источник данных.
        // Устанавливать значение объекта _SelectedGroupStudents будем в Set-ере свойства SelectedGroup (расположено выше).
        //public ICollectionView SelectedGroupStudents { get { return _SelectedGroupStudents?.View; } }
                
        private void OnStudentFiltered(object sender, FilterEventArgs e)
        {
            //throw new NotImplementedException();
            //if (!(e.Item is Student student)) return;           // Если фильтруетс студент, то нас это не интересует.
            // Либо:
            if (!(e.Item is Student student))                   // Для всех записей, которые не являются студентами.
            {
                e.Accepted = false;                             // Срываем все записи, не являющиеся студентами.
                return;
            }

            var filter_text = _StudentFilterText;               // Захватываем текст фильтра
            if (string.IsNullOrWhiteSpace(filter_text))
                return;

            if (student.Name is null || student.Surename is null || student.Patronymic is null)  // Если хоть одно данное поле у студента пусто.
            {
                e.Accepted = false;
                return;
            }

            // Если хотя бы одно поле софпадает с текстом фильтра, то не отбраковываем такого студента:
            if (student.Name.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if(student.Surename.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if (student.Patronymic.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;                                 // Если все три проверки не увенчались успехом, то студент не принимается.
        }
              
        public ICollectionView SelectedGroupStudents => _SelectedGroupStudents?.View;
        // Выполним привязку к этому свойству в разметке.

        // ПОсле того, как мы настроили представление списка CollectionViewSource _SelectedGroupStudents, в него необходимо добавить фильтр. Сделаем это через конструктор.

        // В фильтре нужно реалтзовать ту же самую логику, что была в файле "MainWindow.xaml.cs". Поэтому, нам понадобится ещё одно свойство: StudentFilterText.

        #endregion


        #region StudentFilterText : string - Текст фильтра студентов

        /// <summary>Текст фильтра студентов</summary>
        private string _StudentFilterText;

        /// <summary>Текст фильтра студентов</summary>
        public string StudentFilterText
        {
            get => _StudentFilterText;
            set
            {
                if (!Set(ref _StudentFilterText, value)) return;
                _SelectedGroupStudents.View.Refresh();
            }            
        }
        #endregion

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
        private int _SelectedPageIndex = 1;

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

        // Для тестирования виртуализации:
        //public IEnumerable<Student> TestStudents =>   // Создадим свойство, которое будет возвращать перечисление (то есть нам не нужен весь массив на пару миллионов студентов)
        //                                                                        // При этом будем динамически их (студентов) генерировать при каждом запросе этого свойства спомощью класса Enumerable,
        //                                                                        // создавая перечисление от 1 до 10000 элементов.
        //    .Select(i => new Student                                            // для каждого элемента создаём Selectи создаём студентов.
        //    {
        //        Name = $"Имя {i}",
        //        Surename = $"Фамилия {i}"

        //    });
        
        // Надо заметить. что дизайнер не предназначан для больших нагрузок по отображению огромного количества студентов. Поэтому, чтобы он не сломался, добавим в приложение в App.xaml.cs
        // специальное свойство, которое будет опрпделять, работаем ли мы в дизайнере, или запущен exe-файл. Здесь же спеределаем код и сделвем выбор:
        public IEnumerable<Student> TestStudents =>
            Enumerable.Range(1, App.IsDesignMode ? 10 : 100_000)                 // Если находимся в режиме DesignMode, то отобразим 10 студентов. Если запущена программа, то 100000
                .Select(i => new Student
                {
                    Name = $"Имя {i}",
                    Surename = $"Фамилия {i}"

                });




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

        private bool CanChangeTabIndexCommandExecute(object p) => _SelectedPageIndex >= 0;

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
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecute);

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
        }
        /*------------------------------------------------------------------------------------------------------------------------------------------*/
    }
}
