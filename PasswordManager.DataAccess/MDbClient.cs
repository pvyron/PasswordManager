using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess
{
    internal class MDbClient
    {
        private readonly IMongoDatabase _db;

        public MDbClient()
        {
            var client = new MongoClient("mongodb+srv://pvyron:6FR6odqLhF3VbuKe@passwordmanager.x5wx4kd.mongodb.net/?retryWrites=true&w=majority");
            _db = client.GetDatabase("MasterData");
        }

        public async Task<T> GetRecord<T>(string table, Guid id, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object>
            {
                {"Id", id }
            };
            return (await GetRecord<T>(table, new KeyValuePair<string, object>("Id", id), cancellationToken)).First();
        }

        public async Task<IEnumerable<T>> GetRecord<T>(string table, KeyValuePair<string, object> filter, CancellationToken cancellationToken)
        {
            var queryFilter = Builders<T>.Filter.Eq(filter.Key, filter.Value);

            var collection = _db.GetCollection<T>(table);

            return await collection.FindAsync<T>(queryFilter, cancellationToken: cancellationToken).Result.ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetAllRecords<T>(string table, CancellationToken cancellationToken)
        {
            var collection = _db.GetCollection<T>(table);
            return await collection.FindAsync(_ => true, cancellationToken: cancellationToken).Result.ToListAsync(cancellationToken);
        }

        public async Task<T> InsertRecord<T>(string table, T record, CancellationToken cancellationToken)
        {
            var collection = _db.GetCollection<T>(table);
            await collection.InsertOneAsync(record, cancellationToken: cancellationToken);
            return record;
        }

        public async Task UpsertRecord<T>(string table, Guid? id, T record, CancellationToken cancellationToken)
        {
            var collection = _db.GetCollection<T>(table);

            await collection.ReplaceOneAsync(new BsonDocument("id", BsonValue.Create(id)), record, new ReplaceOptions()
            {
                IsUpsert = true
            }, cancellationToken);
        }
    }
}
