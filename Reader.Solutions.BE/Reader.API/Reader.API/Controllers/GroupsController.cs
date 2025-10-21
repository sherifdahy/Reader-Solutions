using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reader.Abstractions;
using Reader.BLL.Contracts.Group.Requests;
using Reader.BLL.Contracts.User.Requests;
using Reader.BLL.Interfaces;
using Reader.Entities.Abstractions.Consts;

namespace Reader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = DefaultRoles.Admin)]

    public class GroupsController(IGroupService groupService) : ControllerBase
    {
        private readonly IGroupService _groupService = groupService;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _groupService.GetAllAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _groupService.GetAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            var result = await _groupService.CreateAsync(request, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            var result = await _groupService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _groupService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
