﻿using CV19.Models.Decanat;
using CV19.Services.Students;
using CV19.ViewModels.Base;
using System.Collections.Generic;

namespace CV19.ViewModels
{
    class StudentsManagementViewModel : ViewModel
    {
        // Конструктор:
        private readonly StudentsManager _StudentsManager;
        public StudentsManagementViewModel(StudentsManager StudentsManager) => _StudentsManager = StudentsManager;

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
    }
}
