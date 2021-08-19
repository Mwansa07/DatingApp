using System.Linq;
using API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace API.Tests
{
    [TestFixture]
    public class UsersControllerTests
    {
        DbContextOptionsBuilder<DataContext> optionsBuilder;
        DataContext dbContext;
        UsersController usersController;

        [SetUp]
        public void Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseInMemoryDatabase("UnitTestDb");
            dbContext = new DataContext(optionsBuilder.Options);
            usersController = new UsersController(dbContext);
        }

        [TearDown]
        public void Dispose()
        {
            optionsBuilder = null;
            foreach(var user in dbContext.Users)
            {
                dbContext.Users.Remove(user);
            }

            dbContext.SaveChanges();
            dbContext.Dispose();
            usersController = null;
        }

        [Test]
        public void GetUsers_ReturnZeroUsers_WhenDatabaseIsEmpty()
        {
            var result = usersController.GetUsers();

            Assert.IsEmpty(result.Result.Value);
        }

        [Test]
        public void GetSingleUser_ReturnZeroUsers_WhenDatabaseIsEmpty()
        {
            var result = usersController.GetUser(1);

            Assert.Null(result.Result.Value);
        }

        [Test]
        public void GetUsers_ReturnThreeUsers_WhenDatabasehasThreeUsers()
        {
            var user1 = new AppUser
            {
                Id = 1,
                Username = "Peter"
            };

            var user2 = new AppUser
            {
                Id = 2,
                Username = "Tom"
            };

            var user3 = new AppUser
            {
                Id = 3,
                Username = "Harry"
            };

            dbContext.Add(user1);
            dbContext.Add(user2);
            dbContext.Add(user3);

            dbContext.SaveChanges();

            var result = usersController.GetUsers();

            Assert.AreEqual(3, result.Result.Value.Count());
            Assert.That(result.Result.Value.ElementAt(2).Username, Is.EqualTo("Harry"));
        }

        [Test]
        public void GetUsers_ReturnOneUser_WhenDatabasehasUserRecords()
        {
            var user1 = new AppUser
            {
                Id = 1,
                Username = "Peter"
            };

            var user2 = new AppUser
            {
                Id = 2,
                Username = "Tom"
            };

            dbContext.Add(user1);
            dbContext.Add(user2);

            dbContext.SaveChanges();

            var result = usersController.GetUser(2);

            Assert.That(result.Result.Value.Id, Is.EqualTo(2));
            Assert.AreEqual("Tom", result.Result.Value.Username);
        }
    }
}
