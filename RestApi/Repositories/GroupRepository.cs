using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;
using Rest.Api.Repositories;
using RestApi.Infrastructure.Mongo;
using RestApi.Mappers;
using RestApi.Models;

namespace RestApi.Repositories;

public class GroupRepository : IGroupRepository
{

    private readonly IMongoCollection<GroupEntity> _groups;

    public GroupRepository(IMongoClient mongoClient, IConfiguration configuration){
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Groups:DatabaseName"));
        _groups = database.GetCollection<GroupEntity>(configuration.GetValue<string>("MongoDb:Groups:CollectionName"));
    }

    public async Task<GroupModel> CreateGroupAsync(string name, Guid[] users,   CancellationToken cancellationToken)
    {
        var group = new GroupEntity {
            Name = name,
            Users = users,
            CreatedAt = DateTime.UtcNow,
            Id = ObjectId.GenerateNewId().ToString()
        };

        await _groups.InsertOneAsync(group, new InsertOneOptions(), cancellationToken);
        return group.ToModel();
    }


    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<GroupEntity>.Filter.Eq(s => s.Id, id);
        await _groups.DeleteOneAsync(filter,cancellationToken);
    }

    public async Task<GroupModel> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        try{

            var filter = Builders<GroupEntity>.Filter.Eq(x => x.Id, id);
            var group = await _groups.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return group.ToModel();

        } catch(FormatException){

            return null;

        }
    }

    public async Task<IEnumerable<GroupModel>> GetByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken) // Nuevo método


    public async Task<IEnumerable<GroupModel>> GetByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken) // Nuevo método
    {
        var filter = Builders<GroupEntity>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
        var groups = await _groups.Find(filter).ToListAsync(cancellationToken);
        var orderedGroups = orderBy switch
        {
            "name" => groups.OrderBy(g => g.Name),
            "creationDate" => groups.OrderBy(g => g.CreatedAt),
            _ => groups.OrderBy(g => g.Name)
        };

        return orderedGroups
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToModel();
    }

    public async Task<GroupModel> GetByNameSpecAsync(string name, CancellationToken cancellationToken){
        try{
            var filter = Builders<GroupEntity>.Filter.Eq(x => x.Name, name);
            var group = await _groups.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return group.ToModel();
        }catch(FormatException){
            return null;
        }
    }




    public async Task<IEnumerable<GroupModel>> GetByNameAsync(string name, CancellationToken cancellationToken) // Nuevo método

    {
        var filter = Builders<GroupEntity>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
        var groups = await _groups.Find(filter).ToListAsync(cancellationToken);
        var orderedGroups = orderBy switch
        {
            "name" => groups.OrderBy(g => g.Name),
            "creationDate" => groups.OrderBy(g => g.CreatedAt),
            _ => groups.OrderBy(g => g.Name)
        };

        return orderedGroups
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToModel();
    }


    public async Task<GroupModel> GetByNameSpecAsync(string name, CancellationToken cancellationToken){
        try{
            var filter = Builders<GroupEntity>.Filter.Eq(x => x.Name, name);
            var group = await _groups.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return group.ToModel();
        }catch(FormatException){
            return null;
        }
    }

    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {

        var filter = Builders<GroupEntity>.Filter.Eq(x => x.Id, id);
        var update = Builders<GroupEntity>.Update.Set(s => s.Name,name).Set(s => s.Users,users);

        await _groups.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }



}
