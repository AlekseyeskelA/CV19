using System.Collections.Generic;

namespace CV19.Models
{
    // Наследник класса PlaceInfo. Информация по стране:
    internal class CountryInfo : PlaceInfo 
    {
        public IEnumerable<PlaceInfo> ProvinceCounts { get; set; }
    }
}
