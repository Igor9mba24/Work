using Work.Data.Base;
using Work.Data.ViewModels;
using Work.Models;

namespace Work.Data.Services
{
    public interface IPersonService  : IEntityBaseRepository<Person>
    {
        Task<Person> GetPersonByIdAsync(long id);
        Task AddNewPersonAsync(VMPerson newPersonVM);
        Task UpdatePersonAsync(VMPerson newPersontVM);
        Task RemovePersonAsync(long id);
        Task<List<VMPersonView>> GetAllPersonsWithSkillsAsync();
    }
}
