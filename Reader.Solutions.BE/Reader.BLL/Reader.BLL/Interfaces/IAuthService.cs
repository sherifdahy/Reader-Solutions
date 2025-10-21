using Reader.BLL.Abstractions;
using Reader.BLL.Contracts.Auth.Requests;
using Reader.BLL.Contracts.Auth.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader.BLL.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token,string refreshToken,CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetTokenAsync(LoginRequest authRequest, CancellationToken cancellationToken = default);
    Task<Result> RevokeAsync(string token,string refreshToken, CancellationToken cancellationToken = default);
}
