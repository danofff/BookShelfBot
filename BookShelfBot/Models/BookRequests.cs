using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookShelfBot.Models
{
    public class BookRequests
    {
        [BsonId]
        public ObjectId RecordtId { get; set; }
        public  int UserId{ get; set; }
        public DateTime Date { get; set; }
        
    }
}
