using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Work.Context;
using Work.Data.Interface;
using Work.Models;
using Work.Data.Repository;
using Work.Data.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;
using Work.Data.ViewModels;
using Serilog;
using Work.Data;
using Microsoft.Extensions.Logging;

namespace Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IRepositorySkill _skillRepository;
        private readonly IDbContext _applictaionDbContext;
        private readonly IPersonService _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IRepositorySkill skillRepository, IDbContext applictaionDbContext, IPersonService personService, ILogger<PersonController> logger)//, IDbContext dbContext)
        {
            _skillRepository = skillRepository;
            _applictaionDbContext = applictaionDbContext;
            _personService = personService;
            _logger = logger;

            if (_personService == null)
            {
                // Логируйте сообщение об ошибке
                throw new ArgumentNullException(nameof(personService), "PersonService is not registered.");
            }
            _personService = personService;
        }

        // GET: api/person/skills
        [HttpGet("skills")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            var skills = await _skillRepository.GetAllSkillsAsync();
            var skillVM = skills.Select(skill => new VMSkill
            {
                Level = skill.Level,
                Name = skill.Name
            }).ToList();

            return Ok(skillVM);
        }
        // Person-VMPerson
        private VMPerson MapProductDetailsToNewProductVM(Person personDetails)
        {
            return new VMPerson()
            {
                Name = personDetails.Name,
                DisplayName = personDetails.DisplayName,
                SkillId = new List<VMSkill>()
            };
        }

        // POST: api/person
        [HttpPost]
        public async Task<IActionResult> CreatePerson(VMPerson newPerson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid data received. Status: {StatusCode}", 400);
                    return BadRequest(ModelState);
                }
                await _personService.AddNewPersonAsync(newPerson);
                _logger.LogInformation("Request processed successfully. Status: {StatusCode}", 200);
                return Ok(newPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request. StatusCode: {StatusCode}", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPersonsWithSkills()
        {
            try
            {
                // Получаем всех людей с навыками из сервиса
                var personsWithSkills = await _personService.GetAllPersonsWithSkillsAsync();

                // Проверяем, есть ли вообще люди с навыками
                if (personsWithSkills == null || !personsWithSkills.Any())
                {
                    _logger.LogDebug("Page not found. Status: {StatusCode}", 404);
                    return NotFound("No persons with skills found");
                }

                // Возвращаем список всех людей с навыками
                _logger.LogInformation("Request processed successfully. Status: {StatusCode}", 200);
                return Ok(personsWithSkills);
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус 500 и сообщение об ошибке
                _logger.LogError(ex, "An error occurred while processing the request. StatusCode: {StatusCode}", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        // GET: api/person/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(long id)
        {
            try
            {
                // Получаем информацию о персоне из сервиса по ее ID
                var personDetails = await _personService.GetPersonByIdAsync(id);

                // Проверяем, существует ли персона с указанным ID
                if (personDetails == null)
                {
                    _logger.LogDebug("Page not found. Status: {StatusCode}", 404);
                    return NotFound($"Person with id {id} not found");
                }
                else
                {
                    // Преобразуем информацию о персоне в ViewModel
                    var personVM = new VMPersonView
                    {
                        Id = personDetails.Id,
                        Name = personDetails.Name,
                        DisplayName = personDetails.DisplayName,
                        SkillId = personDetails.Skill_Person.Select(skillPerson => new VMSkilView
                        {
                            Level = skillPerson.Skill.Level,
                            Name = skillPerson.Skill.Name
                        }).ToList()
                    };

                    // Возвращаем ViewModel в виде JSON
                    _logger.LogInformation("Request processed successfully. Status: {StatusCode}", 200);
                    return Ok(personVM);
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус 500 и сообщение об ошибке
                _logger.LogError(ex, "An error occurred while processing the request. StatusCode: {StatusCode}", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, VMPerson updatedPerson)
        {
            try
            {
                var existingPerson = await _personService.GetPersonByIdAsync(id);
                if (existingPerson == null)
                {
                    _logger.LogDebug("Page not found. Status: {StatusCode}", 404);
                    return NotFound($"Person with id {id} not found");
                }

                // Обновление свойств персоны
                existingPerson.Name = updatedPerson.Name;
                existingPerson.DisplayName = updatedPerson.DisplayName;

                // Удаление существующих навыков персоны
                _applictaionDbContext.Skill_Person.RemoveRange(existingPerson.Skill_Person);

                // Добавление новых навыков персоны
                foreach (var skillId in updatedPerson.SkillId)
                {
                    var newSkillPerson = new Skill_Person()
                    {
                        PersonId = id,
                        SkillId = skillId.Level
                    };
                    existingPerson.Skill_Person.Add(newSkillPerson);
                }

                // Сохранение изменений в базе данных
                await _applictaionDbContext.SaveChangesAsync();
                _logger.LogInformation("Request processed successfully. Status: {StatusCode}", 200);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request. StatusCode: {StatusCode}", StatusCodes.Status500InternalServerError);

                // Откат изменений, чтобы данные не сохранялись в случае ошибки 500
                var entity = _applictaionDbContext.Entry(updatedPerson);
                if (entity.State == EntityState.Modified)
                {
                    entity.CurrentValues.SetValues(entity.OriginalValues);
                    entity.State = EntityState.Unchanged;
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonById(int id)
        {
            try
            {
                // Проверяем, существует ли персона с указанным ID
                var existingPerson = await _personService.GetPersonByIdAsync(id);
                if (existingPerson == null)
                {
                    _logger.LogDebug("Page not found. Status: {StatusCode}", 404);
                    return NotFound($"Person with id {id} not found");
                }

                // Удаляем все навыки персоны
                await _personService.RemovePersonAsync(id);

                // Удаляем персону
                await _personService.RemovePersonAsync(id);

                _logger.LogInformation("Request processed successfully. Status: {StatusCode}", 200);
                return NoContent(); // HTTP-статус 204 No Content
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус 500 и сообщение об ошибке
                _logger.LogError(ex, "An error occurred while processing the request. StatusCode: {StatusCode}", StatusCodes.Status500InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
