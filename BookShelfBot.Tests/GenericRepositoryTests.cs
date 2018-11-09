using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookShelfBot.Repositories;
using BookShelfBot.Models;
using System.Linq;

namespace BookShelfBot.Tests
{
    [TestClass]
    public class GenericRepositoryTests
    {
        [TestMethod]
        public void UserRepository_ShouldHandleReadByIdOperation()
        {
            // AAA - Arrange, Act, Assert
            GenericRepository<User> usersRepository = new GenericRepository<User>();
            User user = new User("test-user-id", "test-user", "test-phone", "test-chat");

            usersRepository.Add(user);

            var userFromDb = usersRepository
                .Read(p => p.UserId == user.UserId)
                .Single();

            Assert.IsTrue(user.UserId == userFromDb.UserId);

            int count = usersRepository.Remove(p => p.UserId == user.UserId);

            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        public void UserRepository_ShouldReadAllUsers()
        {
            GenericRepository<User> usersRepository = new GenericRepository<User>();

            var usersFromDb = usersRepository
                .ReadAll();

            Assert.IsNotNull(usersFromDb);
        }
    }
}
