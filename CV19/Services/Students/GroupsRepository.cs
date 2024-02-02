using CV19.Models.Decanat;
using CV19.Services.Base;

namespace CV19.Services.Students
{
    class GroupsRepository : RepositoryInMamory<Group>
    {
        protected override void Update(Group Source, Group Destination)
        {
            Destination.Name = Source.Name;
            Destination.Description = Source.Description;
        }
    }

    /* Репозиторииготовы. Теперь зарегистрируем их в сервисах в классе Registrator. */
}
