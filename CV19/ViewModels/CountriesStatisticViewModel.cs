using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Services;
using CV19.Services.Interfaces;
using CV19.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IDataService _DataService;

        
        // Свойства:
        
        // Захватываем информацию о том, что является главной Вью-моделью. Связываем Вью-модели между собой (при этом в главной Вью-модели создаём вторичную).
        // Слделаем это в виде свойства, чтобы в случае чего можно было достучаться из представления вторичной модели до главной модели.
        //private MainWindowViewModel MainModel { get; }
        public MainWindowViewModel MainModel { get; internal set; }

        #region SelectedCountry : CountryInfo - Выбранная страна

        /// <summary>Выбранная страна</summary>
        private CountryInfo _SelectedCountry;

        /// <summary>Выбранная стран</summary>
        public CountryInfo SelectedCountry { get => _SelectedCountry; set => Set(ref _SelectedCountry, value); }

        #endregion


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




        #region Команды
        // Создадим команды, которые позволят загрузить данные:
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
        //public CountriesStatisticViewModel() : this(null)
        //{
        //    // Сделаем так, чтобы его не возможно было использовать в процессе запуска рограммы:
        //    if (!App.IsDesignMode)
        //        throw new InvalidOperationException("Вызов конструктора, не предназначенного для использования в обычном режиме");

        //    // Создадми несколько стран, для того, чтобы увидеть их в дизайнере:
        //    _Countries = Enumerable.Range(1, 10)
        //        .Select(i => new CountryInfo
        //        {
        //            Name = $"Country {i}",
        //            Provinces = Enumerable.Range(1, 10).Select(j => new PlaceInfo
        //            {
        //                Name = $"Province {j}",
        //                Location = new Point(i, j),
        //                Counts = Enumerable.Range(1, 10).Select(k => new ConfirmedCount
        //                {
        //                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(100 - k)),
        //                    Count = k
        //                }).ToArray()
        //            }).ToArray()
        //        }).ToArray();

        //}


        // Классический конструктор:
        //public CountriesStatisticViewModel(MainWindowViewModel MainModel)   // Данная вью-модель будет создаваться, захватывая информаци о том, что является главной вью-моделью.
                                                                            // Связываем вью-модели между собой. А в главной вью-модели создаём вторичную.
        //public CountriesStatisticViewModel()
        // Перестанем создавать DataService вручную, а будем требовать выдать нам его:
        public CountriesStatisticViewModel(IDataService DataService)
        {
            //this.MainModel = MainModel;

            //_DataService = new DataService();
            _DataService = DataService;

            ///*При обращении к сервисам мы можем у контейнера сервисов создать область видимости сервисов, и запрашивать сервисы уже не у самого контейнера, а у этой области:*/
            //var scope = App.Host.Services.CreateScope();

            //var data = scope.ServiceProvider.GetRequiredService<IDataService>();

            ///*В этом случае, как только область схлопнется, т.е. у неё будет вызван метод Dispose, то в этом случае все объекты, которые выдавались контейнером сервиса,
            // * будут уничтоженыю. У тех классов, у которых определён интерфейс IDisposable, у них будет вызван метод Dispose соответственно, :*/
            //scope.Dispose();

            // Работать можно в следующем режиме:
            //using (var scope = App.Host.Services.CreateScope())
            //{
            //    var data = scope.ServiceProvider.GetRequiredService<IDataService>();
            //}
            /*Т.е., мы создаём область (using (var scope = App.Host.Services.CreateScope())), в которой хотим поарботать с какими-то сервисами, которые потом будут больше не нужны.
             После этого мы извлекаем ныжные нам сервисы (var data = ) из контернера, раьботаем в ними, дальше закрываем область using. Она автоматически удаляет всё,
            что нами было извлечено из контейнера*/

            /*Когда мы напрямую создаём объекты, ты мы должны заботиться о том, чтобы освободить их ресурсы, чтобы вызвать у них метод Dispose. Но если мы работаем с контейнером
             сервисов, то обязанность и ответственность за освобождение ресурсов полностью берёт на себя контейнер, т.е. если мы достали какой-то ресурс из этого контейнера,
            который подразумевает необходимость освобождения ресурсов, то после использования этого объекта мы не должны освобождать эти ресурсы самостоятельно. За это отвечает
            именно контейнер сервисов*/

            // Проверка на то, создаются ли разные объекты, или используются ранее созданные (Урок 5, Время 1:58:40) :
            //var data = App.Host.Services.GetRequiredService<IDataService>();
            //var are_ref_equal = ReferenceEquals(DataService, data);

            //using (var scope = App.Host.Services.CreateScope())
            //{
            //    var data2 = scope.ServiceProvider.GetRequiredService<IDataService>();
            //    var are_ref_equal2 = ReferenceEquals(DataService, data2);
            //    var are_ref_equal3 = ReferenceEquals(data, data2);
            //}

            #region Команды

            RefreshDataCommand = new LambdaCommand(OnRefreshDataCommandExecuted);

            #endregion
        }
    }
}
