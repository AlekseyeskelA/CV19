using CV19.Models.Interfaces;
using System.Collections.Generic;

namespace CV19.Models.Decanat
{
    internal class Group : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Свойство Students зададим как ICollection, чтобы можно было получить свободу выбора того, какую коллекцию сюда добавлять (List, Array, Hesh-таблица, Observablecollection и т.д.) (Queue нельзя добавить):
        //public ICollection<Student> Students { get; set; }
        public IList<Student> Students { get; set; }
        public string Description { get; set; }        
    }
}
