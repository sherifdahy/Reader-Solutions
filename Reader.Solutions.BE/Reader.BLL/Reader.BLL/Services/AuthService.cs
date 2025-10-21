using Microsoft.AspNetCore.Identity;
using NOTE.Solutions.BLL.Errors;
using NOTE.Solutions.Entities.Interfaces;
using Reader.BLL.Abstractions;
using Reader.BLL.Authentication;
using Reader.BLL.Contracts.Auth.Requests;
using Reader.BLL.Contracts.Auth.Responses;
using Reader.BLL.Interfaces;
using Reader.Entities.Entities;
using Reader.Entities.Entities.Identity;
using System.Security.Cryptography;

namespace Reader.BLL.Services;
public class AuthService(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,IUnitOfWork unitOfWork, IJWTProvider provider) : IAuthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IJWTProvider _provider = provider;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest authRequest,CancellationToken cancellationToken)
    {

        var applicationUser = await _userManager.FindByEmailAsync(authRequest.Email);

        if(applicationUser is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var checkPasswordResult = await _signInManager
            .PasswordSignInAsync(applicationUser,
            authRequest.Password,
            false,
            true);

        if(checkPasswordResult.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var result = _provider.GeneratedToken(applicationUser, roles);

            // token dosn't related to jwt token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            applicationUser.RefreshTokens.Add(new RefreshToken()
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration,
            });

            await _userManager.UpdateAsync(applicationUser);

            AuthResponse response = new()
            {
                Token = result.token,
                ExpireIn = result.expiresIn,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration,
            };

            return Result.Success(response);
        }

        var error = checkPasswordResult.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : checkPasswordResult.IsLockedOut
            ? UserErrors.LockedUser
            : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponse>(error);        
    }
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _provider.ValidateToken(token);

        if(userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var applicationUser = await _userManager.FindByIdAsync(userId.ToString()!);

        if (applicationUser is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (applicationUser.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedUser);

        var userRefreshToken = applicationUser.RefreshTokens.SingleOrDefault(x=>x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var roles = await _userManager.GetRolesAsync(applicationUser);

        var result = _provider.GeneratedToken(applicationUser, roles);

        // token dosn't related to jwt token
        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        applicationUser.RefreshTokens.Add(new RefreshToken()
        {
            Token = newRefreshToken,
            ExpiresOn = newRefreshTokenExpiration,
        });

        await _userManager.UpdateAsync(applicationUser);

        AuthResponse response = new()
        {
            Token = result.token,
            ExpireIn = result.expiresIn,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiration = newRefreshTokenExpiration,
        };

        return Result.Success(response);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result> RevokeAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _provider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var applicationUser = await _userManager.FindByIdAsync(userId.ToString()!);

        if (applicationUser is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = applicationUser.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(applicationUser);

        return Result.Success();
    }
}
