using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Work.Context;
using Work.Data.Base;
using Work.Data.Interface;
using Work.Data.ViewModels;
using Work.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;

namespace Work.Data.Services
{
    public class PersonService : EntityBaseRepository<Person>, IPersonService
    {
        private readonly IDbContext _appDbContext;
        private readonly ILogger<PersonService> _logger;

        public PersonService(ApplictaionDbContext appDbContext, ILogger<PersonService> logger) : base(appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }
        public async Task<List<VMPersonView>> GetAllPersonsWithSkillsAsync()
        {
            // Получение всех людей из базы данных
            var persons = await _appDbContext.Persons
                .Include(p => p.Skill_Person)
                .ThenInclude(sp => sp.Skill) // Убедитесь, что связанные навыки также загружены
                .ToListAsync();

            if (persons == null || !persons.Any())
            {
                return new List<VMPersonView>(); // Возвращаем пустой список, если нет ни одного человека в базе данных
            }

            var personsWithSkills = new List<VMPersonView>();

            // Преобразование данных о людях и их навыках в DTO
            foreach (var person in persons)
            {
                var personView = new VMPersonView
                {
                    Id = person.Id,
                    Name = person.Name,
                    DisplayName = person.DisplayName,
                    SkillId = person.Skill_Person?
                        .Select(skillPerson => new VMSkilView
                        {
                            Level = skillPerson.Skill?.Level ?? 0, // Проверяем, что навык не null
                            Name = skillPerson.Skill?.Name
                        })
                        .ToList() ?? new List<VMSkilView>() // Используем пустой список, если Skill_Person или навык null
                };
                personsWithSkills.Add(personView);
            }

            return personsWithSkills;
        }
        public async Task AddNewPersonAsync(VMPerson newPersonVM)
        {
            using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var newPerson = new Person()
                    {
                        Name = newPersonVM.Name,
                        DisplayName = newPersonVM.DisplayName,
                    };

                    await _appDbContext.AddAsync(newPerson);
                    await _appDbContext.SaveChangesAsync();

                    // Add Person Skill
                    foreach (var skillId in newPersonVM.SkillId)
                    {
                        var newSkillPerson = new Skill_Person()
                        {
                            PersonId = newPerson.Id,
                            SkillId = skillId.Level
                        };
                        await _appDbContext.AddAsync(newSkillPerson);
                    }
                    await _appDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding a new person. Rolling back transaction.");
                    await transaction.RollbackAsync();
                    throw; // Повторно выбрасываем исключение для обработки в контроллере
                }
            }
        }

        public async Task RemovePersonAsync(long id)
        {
            var personDetails = await _appDbContext.Persons
                .Include(p => p.Skill_Person)
                .FirstOrDefaultAsync(person => person.Id == id);

            if (personDetails != null)
            {
                // Удаляем все навыки персоны
                _appDbContext.Skill_Person.RemoveRange(personDetails.Skill_Person);

                // Удаляем персону
                _appDbContext.Persons.Remove(personDetails);

                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Person> GetPersonByIdAsync(long id)
        {
            var personDetails = await _appDbContext.Persons
                .Include(person => person.Skill_Person).ThenInclude(product => product.Skill)
                .FirstOrDefaultAsync(person => person.Id == id);

            return personDetails;
        }

        public async Task UpdatePersonAsync(VMPerson newPersonVM)
        {

            var dbPerson = await _appDbContext.Persons
                .Include(p => p.Skill_Person)
                .FirstOrDefaultAsync(person => person.Id == newPersonVM.Id);

            if (dbPerson != null)
            {
                dbPerson.Name = newPersonVM.Name;
                dbPerson.DisplayName = newPersonVM.DisplayName;

                // Удаление существующих навыков персоны
                _appDbContext.Skill_Person.RemoveRange(dbPerson.Skill_Person);

                // Добавление новых навыков персоны
                foreach (var skillId in newPersonVM.SkillId)
                {
                    var newSkillPerson = new Skill_Person()
                    {
                        PersonId = dbPerson.Id,
                        SkillId = skillId.Level
                    };
                    dbPerson.Skill_Person.Add(newSkillPerson);
                }

                // Сохранение изменений в базе данных
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
