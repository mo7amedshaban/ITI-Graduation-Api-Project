using System.Security.Claims;
using Core.Common.Results;
using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
    }


    public string GetRole()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);
        return claim?.Value ?? string.Empty;
    }

    public Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var claim = _httpContextAccessor.HttpContext?.User;
        if (claim != null)
        {
            var userDto = new UserDto
            {
                Id = userId,
                FirstName = claim.Identity?.Name ?? "Unknown",
                Email = claim.FindFirst(ClaimTypes.Email)?.Value,
                Role = claim.FindFirst(ClaimTypes.Role)?.Value ?? "User"
            };
            return Task.FromResult(Result<UserDto>.FromValue(userDto));
        }
        return Task.FromResult(Result<UserDto>.FromError(Error.NotFound("User not found")));
    }

}