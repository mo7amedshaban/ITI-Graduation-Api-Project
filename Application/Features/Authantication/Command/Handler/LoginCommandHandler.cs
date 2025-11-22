using Application.Features.Authantication.Command.Models;
using Application.Features.Authantication.Dtos;
using Core.Common.Results;
using Core.Entities.Identity;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authantication.Command.Handler
{

    public class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterRequest> _validator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
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

        public async Task<Result<AuthResult>> Handle(LoginCommand command, CancellationToken ct)
        {
            var request = command.login;
            try
            {
                var email = request.Email.ToLowerInvariant();
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: user not found for {Email}", request.Email);
                    return Result<AuthResult>.FromError(Error.Failure("INVALID_CREDENTIALS", "Invalid credentials"));
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return Result<AuthResult>.FromError(
                        Error.Failure("ACCOUNT_LOCKED", "Account is temporarily locked. Try again later."));
                }

                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    await _userManager.AccessFailedAsync(user);
                    return Result<AuthResult>.FromError(Error.Failure("INVALID_CREDENTIALS", "Invalid credentials"));
                }
                // ?? Reset access failed count on successful login
                await _userManager.ResetAccessFailedCountAsync(user);
               
                var accessToken = await _tokenService.GenerateTokenAsync(user);

            
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

                await _refreshTokenRepository.SaveRefreshTokenAsync(
                    user.Id,
                    refreshToken.RefreshToken, 
                    DateTime.UtcNow.AddMinutes(60)
                );

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                var authResult = AuthResult.Success(accessToken.Token, refreshToken.RefreshToken, userDto);

                return Result<AuthResult>.FromValue(authResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during authentication for {Email}", request.Email);
                return Result<AuthResult>.FromError(Error.Failure("AUTH_ERROR", "Unexpected authentication error"));
            }
        }
    }

}
