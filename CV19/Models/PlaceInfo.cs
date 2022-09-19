using System.Collections.Generic;
using System.Windows;

namespace CV19.Models
{
    internal class PlaceInfo                                        // Создадим класс PlaceInfo.
    {
        // C полями уже давно не работают, а работают исключительно со свойствами, когда работаем с WPF!
        public string Name { get; set; }
        public virtual Point Location { get; set; }                         // Point - структура, обладающая X и Y вещественного типа.
        public IEnumerable<ConfirmedCount> Counts { get; set; }     // По каждой стране должна быть информация по количеству подтверждённых случаев.
        public override string ToString() => $"{Name}({Location})";
    }
}
