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


    [HttpGet] // Esta es la ruta para buscar por nombre
    public async Task<ActionResult<List<GroupResponse>>> GetGroupsByName([FromQuery]string name, CancellationToken cancellationToken){
    var groups = await _groupService.GetGroupByNameAsync(name, cancellationToken);
    return Ok(groups.Select(g => g.ToDto()).ToList());
    }
}