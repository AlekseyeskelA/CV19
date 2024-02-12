using CV19.Models.Decanat;
using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.Services.Students
{
    /* Создадим сервис (менеджер) для добавления и изменения студентов, выставления им оценок и т.д. Для него можно было бы выделить отдельный интерфейс, 
     * но, так как он у нас один, и мы не собираемся переписывать его как-то по другому, то просто создаём обычный класс и зарегистрируем просто как
     * отдельный класс без интерфейса. */
    class StudentsManager
    {
        /* Менеджер должен уметь делать какие-то комплексные операции над сущностями студентов и над сущностями групп, поэтому ему нужны оба охранилища.
         * Через конструктор затягивам оба хранилища и сохраняем в приватны поля: */
        private readonly StudentsRepository _Students;
        private readonly GroupsRepository _Groups;

        /* Сделаем возможность извлечения данных по всем студентам и по всем группам: */
        public IEnumerable<Student> Students => _Students.GetAll();
        public IEnumerable<Group> Groups => _Groups.GetAll();

        public StudentsManager(StudentsRepository Students, GroupsRepository Groups)
        {
            _Students = Students;
            _Groups = Groups;
        }

        // Метод создания студента для указанной по её имени группы:
        public bool Create(Student Student, string GroupName)
        {
            if (Student is null) throw new ArgumentNullException(nameof(Student));
            if (string.IsNullOrWhiteSpace(GroupName)) throw new ArgumentException("Некорректное имя группы", nameof(GroupName));

            var group = _Groups.Get(GroupName);
            if (group is null)
            {
                group = new Group { Name = GroupName };
                _Groups.Add(group);
            }
            /* добавляем в список студентов нового студента для группы: */
            group.Students.Add(Student);
            /* Добавляем студента в репозиторий: */
            _Students.Add(Student);

            return true;
        }

        /* Шаблон UnitOfWork подразумевает создание некоторой сущности, которая выполняет какой-то набор операций, нацеленный на какую-то конкретную сущность.
         * В нашеим случае это - студенты. Но, для выполнения операций над студентами может понадобиться управление информацией по гркппам. Т.е. таким образом
         * шаблон UnitOfWork внутри себя может использовать какие-то более атомарные сервисы для выполнения возложенных на него обязанностей.*/

        public void Update(Student Student) => _Students.Update(Student.Id, Student);
    }
}
