using RestApi.Models;

namespace Rest.Api.Repositories;

public interface IGroupRepository{

    Task<GroupModel> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<IEnumerable<GroupModel>> GetByNameAsync(string name, CancellationToken cancellationToken);
}