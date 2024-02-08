﻿using CV19.Infrastructure.Commands;
using CV19.Models.Decanat;
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
        public StudentsManagementViewModel(StudentsManager StudentsManager) => _StudentsManager = StudentsManager;

        #region Команды
        #region Command EditStudentCommand - Команда редактирования студента			
        private ICommand _EditStudentCommand;
        /// <summary>Команда редактирования студента</summary>
        public ICommand EditStudentCommand => _EditStudentCommand
            ??= new LambdaCommand(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);
        private static bool CanEditStudentCommandExecute(object p) => p is Student;
        private void OnEditStudentCommandExecuted(object p)
        {
            var student = (Student)p;   // Приводим параметр команды к студенту.

            var dlg = new StudentEditorWindow
            {
                FirstName = student.Name,
                LastName = student.Surename,
                Patronymic = student.Patronymic,
                Rating = student.Rating,
                Birthday = student.Birthday
            };

            if (dlg.ShowDialog() == true)
                MessageBox.Show("Пользователь выполнил редактирование");
            else
                MessageBox.Show("Пользователь отказался");
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
