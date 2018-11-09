using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace BookShelfBot.Repositories
{
    public class GenericRepository<T>
    {
        private const string connectionString = @"mongodb://bookshelldb:hL4lsgPOUFligtytI9Fg7NhbZtd0zsmMqQllZtkO9cFqizbLnraLHz4Z0mv9de8a4fkmHbLJksVbDZ0nHaAnJA==@bookshelldb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
        private MongoClientSettings _settings;
        private MongoClient _client;

        private const string databaseName = "bookshelldb";

        public IEnumerable<T> ReadAll()
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<T>(typeof(T).FullName);

            var list = collection.Find<T>(_ => true).ToList();
            return list;
        }

        public IEnumerable<T> Read(Expression<Func<T, bool>> filter)
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<T>(typeof(T).FullName);

            return collection
                .FindSync<T>(filter)
                .ToList();
        }

        public void Add(T t)
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<T>(typeof(T).FullName);

            collection.InsertOne(t);
        }

        public int Remove(Expression<Func<T, bool>> filter)
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<T>(typeof(T).FullName);

            return (int)collection.DeleteMany<T>(filter).DeletedCount;
        }

        public GenericRepository()
        {
            _settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            _settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _client = new MongoClient(_settings);
        }
    }
}
