using CV19.Models.Decanat;
using CV19.Services.Base;

namespace CV19.Services.Students
{
    /* Определим базовый класс RepositoryInMamory в папке Base в Сервисах. */
    internal class StudentsRepository : RepositoryInMamory<Student>
    {
        protected override void Update(Student Source, Student Destination)
        {
            Destination.Name = Source.Name;
            Destination.Surename = Source.Surename;
            Destination.Patronymic = Source.Patronymic;
            Destination.Birthday = Source.Birthday;
            Destination.Rating = Source.Rating;
        }
    }

    /* Репозиторииготовы. Теперь зарегистрируем их в сервисах в классе Registrator. */
}
