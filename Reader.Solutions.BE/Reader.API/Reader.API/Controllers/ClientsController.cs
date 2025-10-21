using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reader.Abstractions;
using Reader.BLL.Contracts.Client.Requests;
using Reader.BLL.Contracts.User.Requests;
using Reader.BLL.Interfaces;
using Reader.Entities.Abstractions.Consts;
using System.Threading;
using System.Threading.Tasks;

namespace Reader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = DefaultRoles.Admin)]
    public class ClientsController(IClientService userService) : ControllerBase
    {
        private readonly IClientService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var result = await _userService.GetAsync(id,cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientRequest request,CancellationToken cancellationToken)
        {
            var result = await _userService.CreateAsync(request,cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id , UpdateClientRequest request,CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateAsync(id,request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
