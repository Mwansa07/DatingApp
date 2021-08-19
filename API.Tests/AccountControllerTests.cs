using System;
using API.Controllers;
using API.Data;
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
            var username = "Phil";
            var password = "StrangerThings";
            var result = accountController.Register(username, password);

            Assert.That(result.Result.Value.Username, Is.EqualTo(username));
        }
    }
}
