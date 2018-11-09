using BookShelfBot.Models;
using BookShelfBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelfBot.Services
{
    public class UserManagementService
    {
        private readonly GenericRepository<User> _userStorage;

        public User CheckUserRegistration(string userId)
        {
            var user = _userStorage.Read(p => p.UserId == userId)
                .FirstOrDefault();

            return user;
        }
        public UserManagementService()
        {
            _userStorage = new GenericRepository<User>();
        }

        public User SignUpApplicationUser(UserRegistrationModel model)
        {
            var user = new User()
            {
                ChatId = model.ChatId,
                UserName = model.UserName,
                UserId = model.ChatId
            };

            _userStorage.Add(user);

            return user;
        }
    }
}
