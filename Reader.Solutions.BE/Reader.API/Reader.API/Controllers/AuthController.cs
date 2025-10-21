
using Microsoft.AspNetCore.Mvc;
using Reader.Abstractions;
using Reader.BLL.Contracts.Auth.Requests;
using Reader.BLL.Interfaces;

namespace Reader.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("get-token")]
    public async Task<IActionResult> GetToken(LoginRequest authRequest, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(authRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request ,CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken,cancellationToken);
        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }


    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke(RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeAsync(request.Token, request.RefreshToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

}
