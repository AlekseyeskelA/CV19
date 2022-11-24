using CV19.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.Services.Interfaces
{
    internal interface IDataService
    {
        IEnumerable<CountryInfo> GetData();
    }
}
