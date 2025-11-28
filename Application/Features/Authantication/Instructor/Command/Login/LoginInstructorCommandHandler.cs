using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Authantication.Instructor.Command.Login;

public class LoginInstructorCommandHandler
    : IRequestHandler<LoginInstructorCommand, Result<AuthResult>>
{
    private readonly ILogger<LoginInstructorCommandHandler> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginInstructorCommandHandler(
        UserManager<ApplicationUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        ILogger<LoginInstructorCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<Result<AuthResult>> Handle(LoginInstructorCommand command, CancellationToken cancellationToken)
    {
        var request = command.login;

        try
        {
            var email = request.Email.ToLowerInvariant();
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found for {Email}", request.Email);
                return Result<AuthResult>.Error("INVALID_CREDENTIALS");
            }

            if (!await _userManager.IsInRoleAsync(user, "Instructor"))
                return Result<AuthResult>.Error("NOT_INSTRUCTOR");

            if (await _userManager.IsLockedOutAsync(user)) return Result<AuthResult>.Error("ACCOUNT_LOCKED");

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                await _userManager.AccessFailedAsync(user);
                return Result<AuthResult>.Error("INVALID_CREDENTIALS");
            }

            // Login Success → Reset failed count
            await _userManager.ResetAccessFailedCountAsync(user);

            var accessToken = await _tokenService.GenerateTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

            await _refreshTokenRepository.SaveRefreshTokenAsync(
                user.Id,
                refreshToken.RefreshToken,
                DateTime.UtcNow.AddDays(1)
            );

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Role = "Instructor"
            };

            var authResult = AuthResult.Success(
                accessToken.Token,
                refreshToken.RefreshToken,
                userDto
            );

            return Result<AuthResult>.Success(authResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during instructor login: {Email}", request.Email);
            return Result<AuthResult>.Error("AUTH_ERROR");
        }
    }
}