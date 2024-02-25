using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Controllers;
using Work.Data.Services;
using Work.Data.ViewModels;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestWork1.UnitTests.Controller
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

            var controller = new PersonController(null, null, mockService.Object, NullLogger<PersonController>.Instance);

            // Act
            var result = await controller.GetAllPersonsWithSkills();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

    }
}
