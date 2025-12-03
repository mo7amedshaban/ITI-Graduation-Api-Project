namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    // Minimal contract used by application handlers
    Guid? UserId { get; }
    string? UserName { get; }
}

