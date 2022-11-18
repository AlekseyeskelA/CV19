﻿using System;

namespace CV19
{
    /* Проблема создания нормальной полноценной системы инверсии управления и внедрения зависимостей (у преподавателя) назрела, когда понадобилось написат очередной проект
     * на .Net Core в WPF. В результате появилось много проблем, связанных особенно с базой данных: когда речь доходит до Entity Framework, то оказывается, что она не хочет
     подчиняться и создавать миграции потому, что она не видит основных сервисов в нашем приложении, и надо было настроить основное приложение так, чтобы системы, котрые
    полагаются на подобную структуру, с ним работали нормально.*/

    /*Система инверсии управления и внедрения зависимостей (ИУВЗ). Сейчас у нас есть модель-представления, которая внутри себя хранит сервис DataService, через который она собирает
     данные с сервера. Этот сервис мы создаём вручную в конструкторе CountriesStatisticViewModel внутри самой модели-представления. Это называется скрытой зависимостью, и теперь в процессе развития приложения,
    если понадобится создать новую реализацию данного сервиса, либо если понадобится протестировать модель-представления, то её придется тестировать в связке с данным
    сервисом. И могут возникнуть продбелы, так как сервис обращается к внешним серверам, а сети может не быть и т.п. Кроме того, при попытке заменить сам серви придётся
    заменить логику самой модели-представления. Это говорит о том, что у нас получился сильно связанный код, т.е. М-П четко знает, какой сервис она использует. Они связаны
    тесно и замена потребует больших усилий.  Шаблон ИУВЗ позволяет писать слабо связанный код, который полагается не на конкретные реализации функуиональности
    (в виде сервисов, в виде готовых классов, например), а на интерфейсы. Т.е. нам понадобится переписать сервис DataService, добавив ему интерфейс данного сервиса,
    и вместо сервиса DataService мы будем использовать не конкретный класс, а интерфейс, и таким образом мы получим возможность положить в эту переменную любую реализацию этого
    интерфейса, т.е. всё, что наделено этим интерфейсом, сможет здесь использоваться. Развязывание М-П и сервиса внутри нашего приложения позволит их развивать независимо
    друг от друга.
    
    Вторая проблема заключается в том, как создавать М-П и сервис и знакомить друг с другом, когда приложение запускается.
    
    Чтобы уйти от скрытой зависимости нам понабодится посредник, который будет знать, кому что надо, и что у него есть, и в последствии мы будем у него просить выдать нам М-П,
    а он будет автоматически её создавать и следить за тем, что, напимер, этой модели нужен сервис с таким-то интерфейсом. Т.е., появляется внешняя сила, которая будет выдавать
    выдавать эти самые сервисы.  Она имеет разные названия: контейнер сервисов, менеджер сервисов и т.д.
    
    Есть различные способы создания подобной функционаьности. Это библиотеки MVVM-Light и Prizm.
    С выходом .Net Cor Microsoft переработали многие шаблоны, которые использовались в сторонних библиотеках и написали свои собственные, а также свои собственные инструменты.
    Например, это коснулось системы логигирования, системы конфигурации и системы ИУВЗ.
    
    По умолчанию, когда создаётся проект на WPF или WinForms, там этого ничего нет (ни конфигурации, ни ИУВЗ). Однако, можно подключить в данные проекты эти отдельные элементы.
    Можно добавить всё одним пакетом Microsoft.Extensions.Hosting. Этот пакет входит в поставку шаблона приложения ASP.Net Core. Данный пакет можно добавит не только в .Net Core,
    но и в .Net Framework в версиях начиная с 4.6.1 или 4.7.

    Нам понадобится создать своё хост-приложение. Это то, что лежит внутри памяти приложения и реализует такие понятия, как сервисы. К нему можно обратиться и оттуда их достать.
    Для того, стобы создать этот хост, нам придётся создать свою точку входа. По умолчанию  в WPF-приложении точка входа есть, но она спрятана по адресу:
    obj\Debug\netcoreapp3.1\App.g.cs. Создадим новый класс Program и метод Main(). Теперь у  нас есть две точки входа, и нужно выбрать эту. Делается это открытием папки Properties
    через клик правой кнопкой мыши и выбором точки входа CV19.Program. После чего в проекте CV19.csproj появляетчся запись: <StartupObject>CV19.Program</StartupObject>,
    которую можно было бы добавить и руками. При этом вызывающий (главный) поток приложения должен быть помечен атрубутом STA. И теперь мы контролируем факт запуска
    прилодения с самой его начальной точки:*/
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}