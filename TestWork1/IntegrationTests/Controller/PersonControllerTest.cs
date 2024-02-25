using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Context;
using Work.Controllers;
using Work.Data.Interface;
using Work.Data.Services;
using Work.Data.ViewModels;
using Work.Models;

namespace TestWork1.IntegrationTests.Controller
{
    [TestClass]
    public class PersonControllerTests
    {
        [TestMethod]
        public async Task GetAllPersonsWithSkills_ReturnsOk_WithPersons()
        {
            // Arrange
            var mockService = new Mock<IPersonService>();
            var persons = new List<VMPersonView>
    {
        new VMPersonView { Id = 1, Name = "John", DisplayName = "John Doe", SkillId = new List<VMSkilView>() },
        new VMPersonView { Id = 2, Name = "Alice", DisplayName = "Alice Smith", SkillId = new List<VMSkilView>() }
    };
            mockService.Setup(repo => repo.GetAllPersonsWithSkillsAsync()).ReturnsAsync(persons);

            var mockLogger = new Mock<ILogger<PersonController>>();
            var controller = new PersonController(null, null, mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllPersonsWithSkills();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreatePerson_ReturnsOk_WithCreatedPerson()
        {
            // Arrange
            var newPerson = new VMPerson { Name = "Дима", DisplayName = "Дмитрий" };

            var mockService = new Mock<IPersonService>();
            mockService.Setup(repo => repo.AddNewPersonAsync(newPerson))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILogger<PersonController>>();
            var controller = new PersonController(null, null, mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreatePerson(newPerson);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(newPerson, okResult.Value);
        }
        [TestMethod]
        public async Task Details_ReturnsOk_WithPersonDetails()
        {
            // Arrange
            var personId = 1;
            var personDetails = new Person
            {
                Id = personId,
                Name = "Дима",
                DisplayName = "Дмитрий",
                Skill_Person = new List<Skill_Person>
                {
                    new Skill_Person { Skill = new Skill { Name = "Навык 1", Level = 1 } },
                    new Skill_Person { Skill = new Skill { Name = "Навык 2", Level = 2 } }
                }
            };

            var mockService = new Mock<IPersonService>();
            mockService.Setup(repo => repo.GetPersonByIdAsync(personId))
                .ReturnsAsync(personDetails);

            var mockLogger = new Mock<ILogger<PersonController>>();
            var controller = new PersonController(null, null, mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetPersonById(personId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOfType(okResult.Value, typeof(VMPersonView));
            var personVM = (VMPersonView)okResult.Value;
            Assert.AreEqual(personDetails.Id, personVM.Id);
            Assert.AreEqual(personDetails.Name, personVM.Name);
            Assert.AreEqual(personDetails.DisplayName, personVM.DisplayName);
            Assert.AreEqual(personDetails.Skill_Person.Count, personVM.SkillId.Count);
            for (int i = 0; i < personDetails.Skill_Person.Count; i++)
            {
                Assert.AreEqual(personDetails.Skill_Person[i].Skill.Name, personVM.SkillId[i].Name);
                Assert.AreEqual(personDetails.Skill_Person[i].Skill.Level, personVM.SkillId[i].Level);
            }
        }
        //Когда не найдет ID
        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenPersonNotFound()
        {
            // Arrange
            var personId = 1;

            var mockService = new Mock<IPersonService>();
            mockService.Setup(repo => repo.GetPersonByIdAsync(personId))
                .ReturnsAsync((Person)null);

            var mockLogger = new Mock<ILogger<PersonController>>();
            var controller = new PersonController(null, null, mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetPersonById(personId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = (NotFoundObjectResult)result;
        }
        [TestMethod]
        public async Task Edit_ReturnsNoContent_WhenPersonUpdatedSuccessfully()
        {
            // Arrange
            var personId = 1;
            var updatedPerson = new VMPerson
            {
                Id = personId,
                Name = "Updated Name",
                DisplayName = "Updated Display Name",
                SkillId = new List<VMSkill>
        {
            new VMSkill { Level = 1, Name = "Навык 1" },
            new VMSkill { Level = 2, Name = "Навык 2" }
        }
            };

            var existingPerson = new Person
            {
                Id = personId,
                Name = "Old Name",
                DisplayName = "Old Display Name",
                Skill_Person = new List<Skill_Person>
        {
            new Skill_Person { Skill = new Skill { Name = "Старый навык 1", Level = 1 } },
            new Skill_Person { Skill = new Skill { Name = "Старый навык 2", Level = 2 } }
        }
            };

            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(repo => repo.GetPersonByIdAsync(personId))
                             .ReturnsAsync(existingPerson);

            var mockDbContext = new Mock<IDbContext>();

            // Setup SaveChangesAsync to return 1 (indicating success)
            mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var controller = new PersonController(null, null, mockPersonService.Object, NullLogger<PersonController>.Instance);
            var result = await controller.Edit(personId, updatedPerson) as NoContentResult;

            // Additional assertions
            // Verify that the existing person was updated correctly
            Assert.AreEqual(updatedPerson.Name, existingPerson.Name);
            Assert.AreEqual(updatedPerson.DisplayName, existingPerson.DisplayName);
            Assert.AreEqual(updatedPerson.SkillId.Count, existingPerson.Skill_Person.Count);

            // Verify that the existing skills were removed
            foreach (var skillPerson in existingPerson.Skill_Person)
            {
                Assert.IsFalse(updatedPerson.SkillId.Any(s => s.Level == skillPerson.SkillId));
            }
        }
        [TestMethod]
        public async Task Delete_ReturnsNoContent_WhenPersonDeletedSuccessfully()
        {
            // Arrange
            var personId = 1;
            var existingPerson = new Person { Id = personId, Name = "Дмитрий" };

            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(repo => repo.GetPersonByIdAsync(personId))
                             .ReturnsAsync(existingPerson);

            var mockDbContext = new Mock<IDbContext>();

            // Установка ожидаемого поведения для метода SaveChanges
            mockDbContext.Setup(db => db.SaveChangesAsync(default)).Returns(Task.FromResult(0));

            var controller = new PersonController(null, null, mockPersonService.Object, NullLogger<PersonController>.Instance); ;

            // Act
            var result = await controller.DeletePersonById(personId) as NoContentResult;

        }
    }
}