using System.Security.Claims;
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
}