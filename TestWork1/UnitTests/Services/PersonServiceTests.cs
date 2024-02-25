using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Context;
using Work.Data.Services;
using Work.Models;

namespace TestWork1.UnitTests.Services
{

    [TestClass]
    public class PersonServiceTests
    {
        [TestMethod]
        public async Task GetAllPersonsWithSkillsAsync_ReturnsPersons_WithSkills()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplictaionDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplictaionDbContext(options))
            {
                context.Persons.AddRange(new List<Person>
                {
                    new Person { Id = 1, Name = "John", DisplayName = "John Doe" },
                    new Person { Id = 2, Name = "Jane", DisplayName = "Jane Doe" }
                });

                context.Skills.AddRange(new List<Skill>
                {
                    new Skill { Name = "Skill 1", Level = 1 },
                    new Skill {  Name = "Skill 2", Level = 2 }
                });

                context.Skill_Person.AddRange(new List<Skill_Person>
                {
                    new Skill_Person { PersonId = 1, SkillId = 1 },
                    new Skill_Person { PersonId = 1, SkillId = 2 },
                    new Skill_Person { PersonId = 2, SkillId = 2 }
                });

                context.SaveChanges();
            }

            using (var context = new ApplictaionDbContext(options))
            {
                var loggerMock = new Mock<ILogger<PersonService>>();
                var service = new PersonService(context, loggerMock.Object);

                // Act
                var result = await service.GetAllPersonsWithSkillsAsync();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(2, result.First().SkillId.Count);
                Assert.AreEqual("Skill 1", result.First().SkillId[0].Name);
                Assert.AreEqual("Skill 2", result.First().SkillId[1].Name);
            }
        }
    }
}

