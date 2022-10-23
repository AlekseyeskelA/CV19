using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        // Здесь будет жить сервис, который будет извлекать данные. Создаём его в конструкторе. В последствии это выльется в инверсию управоения:
        private DataService _DataService;

        // Захватываем информацию о том, что является главной Вью-моделью. Связываем Вью-модели между собой (при этом в главной Вью-модели создаём вторичную).
        // Слделаем это в виде свойства, чтобы в случае чего можно было достучаться из представления вторичной модели до главной модели.
        private MainWindowViewModel MainModel { get; }

        #region Countries : IEnumerable<CountryInfo> - Статистика по странам

        /// <summary>Статистика по странам</summary>
        private IEnumerable<CountryInfo> _Countries;

        /// <summary>Статистика по странам</summary>
        // Свойство, которое содержит информацию по странам:
        public IEnumerable<CountryInfo> Countries
        {
            get => _Countries;
            private set => Set(ref _Countries, value);      // сделаем приватным, чтобы свойство могло быть установлено только внутри вью-модели, а с наружи было только для чтения (на всякий случай).
        }

        #endregion

        // Создадим команды, которые позволят загрузить данные:
        #region Команды

        public ICommand RefreshDataCommand { get; }

        // Метод выполнения команды:
        private void OnRefreshDataCommandExecuted(object p)
        {
            Countries = _DataService.GetData();             // Загружаем данные с сервиса.
        }

        #endregion


        // Конструкторы:

        /// <summary>Отладочный конструктор, используемый в процессе разработки в визуальном дизайнере</summary>
        /* Мы можем создать здесь дополнительный конструктор и использовать его в процессе разработки. Данный конструктор создаст объект и бутед выдавать данные
        по этому объекту в дизайнере. А во ViewModel в ссоответствующую строку в заголовке добавим код: d:IsDesignTimeCreateble=True:*/
        public CountriesStatisticViewModel() : this(null)
        {
            // Сделаем так, чтобы его не возможно было использовать в процессе запуска рограммы:
            if (!App.IsDesignMode)
                throw new InvalidOperationException("Вызов конструктора, не предназначенного для использования в обычном режиме");

            // Создадми несколько стран, для того, чтобы увидеть их в дизайнере:
            _Countries = Enumerable.Range(1, 10)
                .Select(i => new CountryInfo
                {
                    Name = $"Country {i}",
                    ProvinceCounts = Enumerable.Range(1, 10).Select(j => new PlaceInfo
                    {
                        Name = $"Province {j}",
                        Location = new Point(i, j),
                        Counts = Enumerable.Range(1, 10).Select(k => new ConfirmedCount
                        {
                            Date = DateTime.Now.Subtract(TimeSpan.FromDays(100 - k)),
                            Count = k
                        }).ToArray()
                    }).ToArray()
                }).ToArray();

        }


        // Классический конструктор:
        public CountriesStatisticViewModel(MainWindowViewModel MainModel)   // Данная вью-модель будет создаваться, захватывая информаци о том, что является главной вью-моделью.
                                                                            // Связываем вью-модели между собой. А в главной вью-модели создаём вторичную.
        {
            this.MainModel = MainModel;

            _DataService = new DataService();

            #region Команды

            RefreshDataCommand = new LambdaCommand(OnRefreshDataCommandExecuted);

            #endregion
        }
    }
}
