using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Services;
using RestApi.Mappers;

namespace RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GropusController : ControllerBase{

    private readonly IGroupService _groupService;

    public GropusController(IGroupService groupService){
        _groupService = groupService;
    }
    
    //localhost:port/groups/
    [HttpGet("{id}")]
    public async Task <ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken){
       var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
       if(group is null){
        return NotFound();
       }
       
       return Ok(group.ToDto());
    }
}