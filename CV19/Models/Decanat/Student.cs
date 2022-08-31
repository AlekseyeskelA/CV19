using System;
using System.Collections.Generic;

namespace CV19.Models.Decanat
{
    internal class Student
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
    }

    internal class Group
    {
        public string Name { get; set; }

        // Свойство Students зададим как ICollection, чтобы можно было получить свободу выбора того, какую коллекцию сюда добавлять (List, Array, Hesh-таблица, Observablecollection и т.д.) (Queue нельзя добавить):
        //public ICollection<Student> Students { get; set; }
        public IList<Student> Students { get; set; }
    }
}
