using RestApi.Models;

namespace RestApi.Services;

public interface IGroupService {
    Task<GroupUserModel> GetGroupByIdAsync (string id, CancellationToken cancellationToken);

    Task<IEnumerable<GroupUserModel>> GetGroupByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken); 

    Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken);
    


    Task <GroupUserModel> CreateGroupAsync(string name, Guid[] users, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken);
 

  Task<GroupUserModel> GetByNameSpecifiedAsync(string name, CancellationToken cancellationToken);


  Task UpdateGroupAsync(string id, string name, Guid [] users, CancellationToken cancellationToken);


}