using Core.Entities.Identity;
using Infrastructure.Interfaces;


public interface IRefreshTokenRepository
{
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiration);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    Task RevokeRefreshTokenAsync(Guid userId, string refreshToken);
    public Task<RefreshToken> GetTokenAsync(string refreshToken);
}