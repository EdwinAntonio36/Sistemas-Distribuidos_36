using RestApi.Models;

namespace Rest.Api.Repositories;

public interface IGroupRepository{

    Task<GroupModel> GetByIdAsync(string id, CancellationToken cancellationToken);

     Task<IEnumerable<GroupModel>> GetByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken);
    
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
    Task<GroupModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken);

     Task<GroupModel> GetByNameSpecAsync(string name, CancellationToken cancellationToken);

     Task UpdateGroupAsync (string id, string name, Guid[] users, CancellationToken cancellationToken);
}