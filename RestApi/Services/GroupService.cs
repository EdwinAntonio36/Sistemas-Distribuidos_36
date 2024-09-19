using Microsoft.AspNetCore.SignalR;
using RestApi.Models;
using Rest.Api.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService
{

        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository){
            _groupRepository = groupRepository;
        }

    public async Task<GroupUserModel> GetGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if(group is null){
            return null; 
        }

        return new GroupUserModel{

              Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate
        };
    }

public async Task<List<GroupUserModel>> GetGroupByNameAsync(string name, CancellationToken cancellationToken)
{
    var groups = await _groupRepository.GetByNameAsync(name, cancellationToken);
    return groups.Select(g => new GroupUserModel
    {
        Id = g.Id,
        Name = g.Name,
        CreationDate = g.CreationDate
    }).ToList();
}


}