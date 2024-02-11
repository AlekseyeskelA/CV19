using CV19.Infrastructure.Commands;
using CV19.Models.Decanat;
using CV19.Services.Interfaces;
using CV19.Services.Students;
using CV19.ViewModels.Base;
using CV19.Views.Windows;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    class StudentsManagementViewModel : ViewModel
    {
        // Конструктор:
        private readonly StudentsManager _StudentsManager;
        private readonly IUserDialogService _UserDialog;
        //public StudentsManagementViewModel(StudentsManager StudentsManager) => _StudentsManager = StudentsManager;

        /* Запросим сервис диалога с пользователем: */
        public StudentsManagementViewModel(StudentsManager StudentsManager, IUserDialogService UserDialog)
        {
            _StudentsManager = StudentsManager;
            _UserDialog = UserDialog;
        }

        #region Команды
        #region Command EditStudentCommand - Команда редактирования студента			
        private ICommand _EditStudentCommand;
        /// <summary>Команда редактирования студента</summary>
        public ICommand EditStudentCommand => _EditStudentCommand
            ??= new LambdaCommand(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);
        private static bool CanEditStudentCommandExecute(object p) => p is Student;
        private void OnEditStudentCommandExecuted(object p)
        {
            //var student = (Student)p;   // Приводим параметр команды к студенту.

            //var dlg = new StudentEditorWindow
            //{
            //    FirstName = student.Name,
            //    LastName = student.Surename,
            //    Patronymic = student.Patronymic,
            //    Rating = student.Rating,
            //    Birthday = student.Birthday
            //};
            /* Теперь у нас появилась проблема: Модель-представления тесно связана с представлением, т.е. она завязана на конкретный тип окна редактирования
             * студентов. И, если в последствии мы окно заменим, например, на web-сервис, то пр идётся много чего изменять. Избавимся от этой проблемы.
             * Если у нас стоит задача редактирования какой-то модели, значит нам просто нужно создать сервис по редавтированию, а как он будет это делать - 
             * это уже его головная боль. Создадим интерфейс, предназначенный для диалога с пользователем IUserDialogService. Потом создадим класс на
             * основе этого интерфейса и перенесём туда код редактирования. */

            //if (dlg.ShowDialog() == true)
            //    MessageBox.Show("Пользователь выполнил редактирование");
            //else
            //    MessageBox.Show("Пользователь отказался");

            /* Если редактирование прошло успешно: */
            if (_UserDialog.Edit(p))
            {
                /* То необходимо выполнить сохранение изменений в менеджере: */
                _StudentsManager.Update((Student)p);

                _UserDialog.ShowInformation("Студент отредактирован", "Менеджер студентов");
            }
            else
                _UserDialog.ShowWarning("Отказ от редактирования", "Менеджер студентов");
        }
        #endregion

        #region Command CreateNewStudentCommand - Создание нового студента			
        private ICommand _CreateNewStudentCommand;
        /// <summary>Создание нового студента</summary>
        public ICommand CreateNewStudentCommand => _CreateNewStudentCommand
            ??= new LambdaCommand(OnCreateNewStudentCommandExecuted, CanCreateNewStudentCommandExecute);
        private static bool CanCreateNewStudentCommandExecute(object p) => p is Group; // Можно создаватьтудента только выбраннуюгруппу.
        private void OnCreateNewStudentCommandExecuted(object p)
        {
            var group = (Group)p;

            var student = new Student();

            if (!_UserDialog.Edit(student) || _StudentsManager.Create(student, group.Name))
            {
                OnPropertyChanged(nameof(Students));

                /* В нашем случае не смотря на уведомление OnPropertyChanged(nameof(Students)) есть проблемы с обноселением интеряейса, потому что
                 * модели не поддерживают уведомления о том, что их свойства изменились. Но в этих целях можно создать объекты ViewModel, обёртки для 
                 * соответствующих моделей, и просто выводить на экран не модели в чистом виде, а ViewModel-и, которые просто будут оборочивать 
                 * модели и польностью копировать их свойства. И, таким образом, при редактировании изменений параметров можно будет уведомлять
                 * интерыейс о том, что произошли изменения в свойствах. */
                return;
            }

            if (_UserDialog.Confirm("Не удалось создать студента. Повторить?", "Менеджер студентов"))
                OnCreateNewStudentCommandExecuted(p);
        }
        #endregion
        #endregion

        /* Проверим, связь View и ViewModel при помощи зоголовка: */
        #region Title : string - Заголовок окна
        /// <summary>Заголовок окна</summary>
        private string _Title = "Управление студентами";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }
        #endregion

        /* Создадим пару свойств, которые пробрасываем через менеджер для отображения студентов и групп: */
        public IEnumerable<Student> Students => _StudentsManager.Students;
        public IEnumerable<Group> Groups => _StudentsManager.Groups;

        #region SelectedGroup : Group - Выбранная группа студентов
        /// <summary>Выбранная группа студентов</summary>
        private Group _SelectedGroup;

        /// <summary>Выбранная группа студентов</summary>
        public Group SelectedGroup { get => _SelectedGroup; set => Set(ref _SelectedGroup, value); }
        #endregion

        #region SelectedStudent : Student - Выбранный студент
        /// <summary>Выбранный студент</summary>
        private Student _SelectedStudent;

        /// <summary>Выбранный студент</summary>
        public Student SelectedStudent { get => _SelectedStudent; set => Set(ref _SelectedStudent, value); }
        #endregion
    }
}
