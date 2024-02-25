using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Context;
using Work.Data.Interface;
using Work.Data.Services;
using Work.Data.ViewModels;
using Work.Models;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.Extensions.Options;

namespace TestWork1.IntegrationTests
{
    [TestClass]
    public class PersonServiceTest
    {
        private ApplictaionDbContext _dbContext;
        private PersonService _personService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplictaionDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplictaionDbContext(options);
            var loggerMock = new Mock<ILogger<PersonService>>();
            _personService = new PersonService(_dbContext, loggerMock.Object);
        }

        [TestMethod]
        public async Task GetAllPersonsWithSkillsAsync_ReturnsPersons_WithSkills()
        {
            // Arrange
            var persons = new List<Person>
        {
            new Person { Id = 1, Name = "Дима", DisplayName = "Дмитрий" },
            new Person { Id = 2, Name = "Коля", DisplayName = "Николай" }
        };

            var skills = new List<Skill>
        {
            new Skill { Name = "Навык 1", Level = 1 },
            new Skill { Name = "Навык 2", Level = 2 }
        };

            var skillPersons = new List<Skill_Person>
        {
            new Skill_Person { PersonId = 1, SkillId = 1 },
            new Skill_Person { PersonId = 1, SkillId = 2 },
            new Skill_Person { PersonId = 2, SkillId = 2 }
        };

            _dbContext.Persons.AddRange(persons);
            _dbContext.Skills.AddRange(skills);
            _dbContext.Skill_Person.AddRange(skillPersons);
            _dbContext.SaveChanges();

            // Act
            var result = await _personService.GetAllPersonsWithSkillsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.First().SkillId.Count);
            Assert.AreEqual("Навык 1", result.First().SkillId[0].Name);
            Assert.AreEqual("Навык 2", result.First().SkillId[1].Name);
        }
    




    [TestMethod]
        public void YourTestMethod()
        {
            // Arrange
            var persons = new List<Person>
    {
        new Person { Id = 1, Name = "Игорь" },
        new Person { Id = 2, Name = "Паша" }
    };

            var mockSet = new Mock<DbSet<Person>>();
            mockSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(persons.AsQueryable().Provider);
            mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(persons.AsQueryable().Expression);
            mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(persons.AsQueryable().ElementType);
            mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(persons.GetEnumerator());

            var dbContextMock = new Mock<ApplictaionDbContext>();
            dbContextMock.Setup(m => m.Set<Person>()).Returns(mockSet.Object); // Используйте Set<Person>()
        }
    }
}
