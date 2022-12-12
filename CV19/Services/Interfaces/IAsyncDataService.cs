using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.Services.Interfaces
{
    internal interface IAsyncDataService
    {
        // Сделаем имитацию какой-то длительной деятельности. Будем возвращать строку, как результат этой деятельности:
        string GetResult(DateTime Time);    // Это синхронный метод с блокировкой текущего потока (читает из файла и долго записывает в базу данных).
        // Добавим реализацию этого метода в AsyncDataService.
    }
}
