using System.Runtime.CompilerServices;
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
    public async Task<List<GroupModel>>  GetByNameAsync(string name, CancellationToken cancellationToken){

    var filter = Builders<GroupEntity>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));   
    var groupEntities = await _groups.Find(filter).ToListAsync(cancellationToken);

    var groupModels = groupEntities.Select(g => new GroupModel
    {
        Id = g.Id,
        Name = g.Name,
        Users = g.Users,
        CreationDate = g.CreatedAt
    }).ToList();

    return groupModels;
    }


}
