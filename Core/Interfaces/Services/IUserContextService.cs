using Core.Common.Results;
using Core.DTOs;

namespace Core.Interfaces.Services;

public interface IUserContextService
{
    Guid GetUserId();
    string GetRole();

    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
}