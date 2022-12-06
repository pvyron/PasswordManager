using MongoDB.Bson;
using MongoDB.Driver;
using PasswordManager.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess;

public sealed class MDbClient
{
    private readonly IMongoDatabase _db;

    private readonly ReplaceOptions _replaceOptions = new()
    {
        BypassDocumentValidation = false,
        IsUpsert = false
    };

    public MDbClient()
    {
        var client = new MongoClient("mongodb+srv://pvyron:6FR6odqLhF3VbuKe@passwordmanager.x5wx4kd.mongodb.net/?retryWrites=true&w=majority");
        _db = client.GetDatabase("MasterData");
    }

    public async Task<T?> GetRecordById<T>(string table, Guid id, CancellationToken cancellationToken)
    {
        return await GetRecord<T>(table, ("Id", id), cancellationToken);
    }

    public async Task<T?> GetRecord<T>(string table, (string Field, object Value) filter, CancellationToken cancellationToken)
    {
        var queryFilter = Builders<T>.Filter.Eq(filter.Field, filter.Value);

        var collection = _db.GetCollection<T>(table);

        var result = await collection.FindAsync<T>(queryFilter, cancellationToken: cancellationToken);

        return await result.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetRecords<T>(string table, (string Field, object Value) filter, CancellationToken cancellationToken)
    {
        var queryFilter = Builders<T>.Filter.Eq(filter.Field, filter.Value);

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

    public async Task<T> UpdateRecord<T>(string table, Guid id, T record, CancellationToken cancellationToken)
    {
        var collection = _db.GetCollection<T>(table);

        await collection.ReplaceOneAsync(new BsonDocument("Id", BsonValue.Create(id)), record, _replaceOptions, cancellationToken);

        return record;
    }

    //async Task<T> UpsertRecord<T>(string table, Guid id, T record, CancellationToken cancellationToken)
    //{
    //    var collection = _db.GetCollection<T>(table);

    //    await collection.ReplaceOneAsync(new BsonDocument("Id", BsonValue.Create(id)), record, new ReplaceOptions()
    //    {
    //        IsUpsert = true
    //    }, cancellationToken);

    //    return record;
    //}
}
