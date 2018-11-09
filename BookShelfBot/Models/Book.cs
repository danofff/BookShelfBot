using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelfBot.Models
{
    public class Book
    {
        [BsonId]
        public ObjectId BookId { get; set; }
        public string BookName { get; set; }
        public string BookUrl { get; set; }
    }
}
