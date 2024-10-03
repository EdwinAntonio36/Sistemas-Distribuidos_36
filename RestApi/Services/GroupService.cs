using Microsoft.AspNetCore.SignalR;
using RestApi.Models;
using Rest.Api.Repositories;
using RestApi.Repositories;
using System.Collections;
using RestApi.Exceptions;
using DnsClient.Protocol;
using MongoDB.Driver.Core.Operations;

namespace RestApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository){
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }


public async Task<GroupUserModel> CreateGroupAsync(string name,  Guid[] users, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken){
        if (users.Length == 0)
        {
            throw new InvalidGroupRequestFormatException();
        }
        var groups = await _groupRepository.GetByNameAsync(name,pageIndex,pageSize,orderBy, cancellationToken);
        if(groups.Any()){
            throw new GroupAlreadyExistsException();
        }
        foreach (var userId in users ){
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null){
                throw new UserNotFoundException();
            }
        }

        var group = await _groupRepository.CreateGroupAsync(name,users,cancellationToken);
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()
        };
    }





    public async Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if(group is null){
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteByIdAsync(id, cancellationToken);

        
    }

    public async Task<GroupUserModel> GetGroupByIdAsync(string Id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(Id, cancellationToken);
        if(group is null){
            return null;
        }
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()

        };
    }



    public async Task<IEnumerable<GroupUserModel>> GetGroupByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);

        var groupUserModels = await Task.WhenAll(groups.Select(async group => 
        {
            var users = await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)));
            return new GroupUserModel
            {
                Id = group.Id,
                Name = group.Name,
                CreationDate = group.CreationDate,
                Users = users.Where(user => user != null).ToList()
            };
        }));

        var orderedGroups = orderBy switch
        {
            "name" => groupUserModels.OrderBy(g => g.Name),
            "creationDate" => groupUserModels.OrderBy(g => g.CreationDate),
            _ => groupUserModels.OrderBy(g => g.Name)
        };

        return orderedGroups
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }


public async Task<GroupUserModel> GetByNameSpecifiedAsync(string name, CancellationToken cancellationToken){
        var group = await _groupRepository.GetByNameSpecAsync(name, cancellationToken);
        if(group is null){
            return null;
        }
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()
        };
    }

    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {
         if (users.Length == 0)
        {
            throw new InvalidGroupRequestFormatException();
        }
        foreach (var userId in users ){
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null){
                throw new UserNotFoundException();
            }
        }

        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
            if(group is null){
                throw new GroupNotFoundException();
            }

            var groups = await _groupRepository.GetByNameSpecAsync(name, cancellationToken);
            if(groups != null && groups.Id != id){
                throw new GroupAlreadyExistsException();
            }



            await _groupRepository.UpdateGroupAsync(id, name, users, cancellationToken);

        
    }
}