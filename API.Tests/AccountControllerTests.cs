using System;
using System.Threading.Tasks;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace API.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        DbContextOptionsBuilder optionsBuilder;
        DataContext context;
        AccountController accountController;

        [SetUp]
        public void Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseInMemoryDatabase("TestDb");
            context = new DataContext(optionsBuilder.Options);
            accountController = new AccountController(context);
        }

        [TearDown]
        public void Dispose()
        {
            optionsBuilder = null;
            foreach (var user in context.Users)
            {
                context.Users.Remove(user);
            }

            context.SaveChanges();
            context.Dispose();
            accountController = null;
        }

        [Test]
        public void CreateUser_Test()
        {
            var testUser = new RegisterDto
            {
                Username = "Phil",
                Password = "StrangerThings"
            };
            var result = accountController.Register(testUser);

            Assert.That(result.Result.Value.Username, Is.EqualTo(testUser.Username.ToLower()));
        }

        [Test]
        public void CreateUsernames_ReturnsBadRequest_WhenUsernameAlreadyExists()
        {
            var testUser = new RegisterDto
            {
                Username = "Tom",
                Password = "StrangerThings"
            };

            var testUser2 = new RegisterDto
            {
                Username = "Tom",
                Password = "Adaptation"
            };

            _ = accountController.Register(testUser);
            var result = accountController.Register(testUser2);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result.Result);
        }
    }
}
