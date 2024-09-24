using RestApi.Models;

namespace RestApi.Services;

public interface IGroupService {
    Task<GroupUserModel> GetGroupByIdAsync (string id, CancellationToken cancellationToken);

    Task<IEnumerable<GroupUserModel>> GetGroupByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken); 
}