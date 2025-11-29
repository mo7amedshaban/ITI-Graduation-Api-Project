namespace Core.Interfaces.Services;

public interface IUserContextService
{
    Guid GetUserId();
    string GetRole();
}