using CV19.Services.Interfaces;
using System;
using System.Threading;

namespace CV19.Services
{
    internal class AsyncDataService : IAsyncDataService
    {
        private const int __SleepTime = 7000;
        public string GetResult(DateTime Time)
        {
            Thread.Sleep(__SleepTime);                 // Перед возвратом строки отправим поток спать на 7 секунд.

            return $"Result value {Time}";
        }
    }
}
