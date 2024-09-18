using RestApi.Models;

namespace RestApi.Services;

public interface IGroupService {
    Task<GroupUserModel> GetGroupByIdAsync (string id, CancellationToken cancellationToken);

    Task<List<GroupUserModel>> GetGroupByNameAsync(string name, CancellationToken cancellationToken);
}