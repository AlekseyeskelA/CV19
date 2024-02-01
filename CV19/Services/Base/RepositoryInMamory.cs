using CV19.Models.Interfaces;
using CV19.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace CV19.Services.Base
{
    abstract class RepositoryInMamory<T> : IRepository<T> where T : IEntity
    {
        /* Хранилище объектов в памяти будет работать на основе списка: */
        private readonly List<T> _Items = new List<T>();
        private int _LastId;

        /* Сделаем пару конструкторов: пустой и параметрический, чтобы можно было указать набор сущностей, которые будут сразу записаны в память:*/
        protected RepositoryInMamory() { }
        protected RepositoryInMamory(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item)); // Проверяем, что добавляемый элемент не является пустой ссылкой.            
            if (_Items.Contains(item)) return;                              // Проверяем, что элемент ещё не находится в нашем списке.

            /* Если не находится, то элемент можно добавить в список, но ему нужно установить идентификатор: */
            item.Id = ++_LastId;
            _Items.Add(item);
        }

        /* GetAll() будет возвращать все элементы списка: */
        public IEnumerable<T> GetAll() => _Items;

        public bool Remove(T item) => _Items.Remove(item);

        public void Update(int id, T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), id, "Индекс не может быть меньше 1");

            /* Так как мы работаем в памяти, то если элемент, который мы пытаемся обновить, уже есть в списке, то обновлять н чего не надо, потому,
             * что он уже будет обновлён:*/
            if (_Items.Contains(item)) return;

            var db_item = ((IRepository<T>) this).Get(id);

            /* Если извлечь из репозитория объект, который мы хотим обновить. Если это не получается, то ругаемся: */
            if (db_item is null)
                throw new InvalidOperationException("Редактируемый элемент не найден в репозитории");

            /* Если извлечь удалось, то обновляем элемент. Но как удалить элемент, ропозиторий не знает. Поэтому оставляем это не совести наследников: */
            Update(item, db_item);
        }

        protected abstract void Update(T Source, T Destination);
    }
}
