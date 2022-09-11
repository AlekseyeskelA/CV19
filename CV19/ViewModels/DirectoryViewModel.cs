using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CV19.ViewModels
{
    /* Посмотрим, как работает IerarhicalDataTemplate. Для этого во ViewModel введём понятие рабочего каталога, и создадим данный файл с классами.
     * В иерархическом плане проще всего отображать структуру каталогов, т.е. директории файлов в них. Поэтому введём понятие директории, файла.
     И то и другое будет создаваться на основе пути к соответствующим элементам файловой системы. */

    /* Когда мы делаем модель-представления, и будет необходимость использовать её внутри разметки, особенно для привязки к данным, то крайне желательно, чтобы эти модели
     реализовывали интерфейс INotifyPropertyChanged. В этом случае будет минимизирована вероятность появления утечек памяти, которая может случиться.
    Т.е. если класс модели-представления не реализяет этого интерфейса, то он обрабатывается системой привязок несколько иначе, и в этом случае фиксируется
    ссылка на этот объект, и она не освобождается при освобождении визуальных элементов на экране. В резульитате можно получить утечку памяти на этих ViewModel-ах,
    что очень нехорошо. Поэтому добавляем сюда базовый класс ViewModel с данным интерфейсом.*/
    class DirectoryViewModel : ViewModel
    {
        private readonly DirectoryInfo _DirectoryInfo;



        // Свойство: Выдаёт перечисление директорий (т.е. самих же объектов DirectoryViewModel)
        //public IEnumerable<DirectoryViewModel> SubDirectories => _DirectoryInfo
        //    .EnumerateDirectories()
        //    .Select(dir_info => new DirectoryViewModel(dir_info.FullName));
        public IEnumerable<DirectoryViewModel> SubDirectories
        {
            get
            {
                try
                {
                    return _DirectoryInfo
                        .EnumerateDirectories()
                        .Select(dir_info => new DirectoryViewModel(dir_info.FullName));
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<DirectoryViewModel>();
            }
        }

        // Свойство: Выдаёт перечисление файлов:
        //public IEnumerable<FileViewModel> Files => _DirectoryInfo
        //    .EnumerateFiles()
        //    .Select(file => new FileViewModel(file.FullName));
        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    var files = _DirectoryInfo
                        .EnumerateFiles()
                        .Select(file => new FileViewModel(file.FullName));
                    return files;
                }
                catch(UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<FileViewModel>();
            }
        }

        // Свойство: Объединённое перечисление. Сперва выведем перечисление директорий, а затем файлов внутри директорий.
        // Дочерние элементы директории:
        //public IEnumerable<object> DirectoryItems => SubDirectories.Cast<object>().Concat(Files);
        public IEnumerable<object> DirectoryItems
        {
            get
            {
                try
                {
                    return SubDirectories.Cast<object>().Concat(Files);
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return Enumerable.Empty<object>();
            }
        }

        // Свойство: имя
        public string Name => _DirectoryInfo.Name;

        // Свойство: путь
        public string Path => _DirectoryInfo.FullName;

        // Свойство: время создания
        public DateTime CreationTime => _DirectoryInfo.CreationTime;



        // Конструктор:
        //public DirectoryViewModel(string Path)
        //{
        //    _DirectoryInfo = new DirectoryInfo(Path);
        //}
        // Или так:
        public DirectoryViewModel(string Path) => _DirectoryInfo = new DirectoryInfo(Path);
        
    }


    class FileViewModel : ViewModel
    {
        private readonly FileInfo _FileInfo;



        // Свойство: имя
        public string Name => _FileInfo.Name;

        // Свойство: путь
        public string Path => _FileInfo.FullName;

        // Свойство: время создания
        public DateTime CreationTime => _FileInfo.CreationTime;



        // Конструктор:
        public FileViewModel(string Path) => _FileInfo = new FileInfo(Path);
        
    }
}
