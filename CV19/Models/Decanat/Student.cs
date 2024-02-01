using CV19.Models.Interfaces;
using System;

namespace CV19.Models.Decanat
{
    /* У нас есть модель студента и модель группы. Создадим сервисы, которые позволяют хранить где-то этих студентов, хранить группы и объединять их между собой, т.е.
     * добавляь студентов в группы и редактироватьб параметры студентов, параметры групп. Для этого создадим новый интерфейс IRepository. */

    /* 31.01.2024 - Наделим студентов и группу интерфейсом сущности и разделим их по разный  файлам.  А затем создадим сервисы для хранения студентов и для хранения групп
     * и студентов StudentsStore: */
    internal class Student : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }        
    }
}
