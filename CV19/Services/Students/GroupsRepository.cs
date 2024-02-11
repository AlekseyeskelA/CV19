using CV19.Models.Decanat;
using CV19.Services.Base;
using System.Linq;

namespace CV19.Services.Students
{
    class GroupsRepository : RepositoryInMamory<Group>
    {
        public GroupsRepository() : base(TestData.Groups) {}

        // Поиск первой попавшейся группы с указанным именем:
        public Group Get(string GroupName) => GetAll().FirstOrDefault(g => g.Name == GroupName);

        protected override void Update(Group Source, Group Destination)
        {
            Destination.Name = Source.Name;
            Destination.Description = Source.Description;
        }
    }

    /* Репозиторииготовы. Теперь зарегистрируем их в сервисах в классе Registrator. */
}
