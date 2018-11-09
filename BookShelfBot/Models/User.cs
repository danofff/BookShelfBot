using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelfBot.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string ChatId { get; set; }

        public User(string userId, string userName, string phoneNumber, string chatId)
        {
            UserId = userId;
            UserName = userName;
            PhoneNumber = phoneNumber;
            ChatId = chatId;
        }

        public User()
        {

        }
    }
}
