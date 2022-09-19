using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        // Здесь будет жить сервис, который будет извлекать данные:
        private DataService _DataService;

        // Захватываем информацию о том, что является главной Вью-моделью. Связываем Вью-модели между собой (при этом в главной Вью-модели создаём вторичную):
        private MainWindowViewModel MainModel { get; }
        public CountriesStatisticViewModel(MainWindowViewModel MainModel)
        { 
            MainModel = MainModel;

            _DataService = new DataService();
        }
    }
}
