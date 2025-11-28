using Application.Features.Authantication.Command.Handler;
using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Features.Authantication.Admin.Command.Login;

public class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, Result<AuthResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IValidator<RegisterRequest> _validator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginAdminCommandHandler(
        UserManager<ApplicationUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        IValidator<RegisterRequest> validator,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _validator = validator;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<Result<AuthResult>> Handle(LoginAdminCommand command, CancellationToken cancellationToken)
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

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
                return Result<AuthResult>.Error("NOT_Admin");

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
                Role = "Admin"
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