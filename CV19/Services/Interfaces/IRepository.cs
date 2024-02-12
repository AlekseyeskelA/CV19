using CV19.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CV19.Services.Interfaces
{
    /* Репозиторий должен быть шаблонного типа <T> и позволять хранить различные сущности. Введём понятие сущности в папку  Models/Interfaces/IEntity.
     * Заставим параметр T репозитория стать сущностью. Добавим ограничение where T : IEntity. Теперь мы имеем возможность извлекать все сущности и найти все сущности:*/
    public interface IRepository<T> where T : IEntity
    {
        void Add(T item);       // Возможность добавить сущность в репозиторий.
        IEnumerable<T> GetAll();// Найти все сущности.
        //T Get(int id);          // Bзвлечь все сущности.
        /* Воспользуемся возможностями C#8 (добавим реализацию метода Get): */
        T Get(int id) => GetAll().FirstOrDefault(item => item.Id == id);
        bool Remove(T item);    // Удалеие сущности. Убаление обычно делается с возвращением улевого результата.
        void Update(int id, T item);    // Обновить состояние.

        /* Теперь наделим студентов и группу интерфейсом сущности. */
    }

    /* Альтернативный вариант: */
    //public interface IRepository<IEntity> where IEntity : Models.Interfaces.IEntity
    //{
    //    void Add(IEntity item);
    //    IEnumerable<IEntity> GetAll();
    //    IEntity Get(int id) => GetAll().FirstOrDefault(item => item.Id == id);
    //    bool Remove(IEntity item);
    //    void Update(int id, IEntity item);
    //}
}
