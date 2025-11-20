using Application.Features.Authantication.Command.Models;
using Application.Features.Authantication.Dtos;
using Core.Common.Results;
using Core.Entities.Identity;
using Core.Entities.Students;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.interfaces;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authantication.Command.Handler
{
    public class RegisterStudentHandler
     : IRequestHandler<RegisterStudentCommand, Result<AuthResult>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RegisterRequest> _validator;

        public RegisterStudentHandler(
            UserManager<ApplicationUser> userManager,
            IStudentRepository studentRepository,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IValidator<RegisterRequest> validator)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<AuthResult>> Handle(
            RegisterStudentCommand command,
            CancellationToken ct)
        {
            var request = command.Request;

            var validation = await _validator.ValidateAsync(request, ct);

            if (!validation.IsValid)
            {
                var errors = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));

                return Result<AuthResult>.FromError(
                    Error.Validation("REGISTRATION_VALIDATION_FAILED", errors)
                );
            }

            // check if email exists
            var email = request.Email.ToLowerInvariant();
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return Result<AuthResult>.FromError(
                    Error.Conflict("EMAIL_EXISTS", "An account with this email already exists.")
                );
            }

            // create user
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber?.Trim(),
                EmailConfirmed = true,
                CreatedDate = DateTime.UtcNow,
                LockoutEnabled = true
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));

                return Result<AuthResult>.FromError(
                    Error.Failure("IDENTITY_CREATION_FAILED", errors)
                );
            }

          
            await _userManager.AddToRoleAsync(user, "Student");


            var student = new Student(user.Id);

            await _studentRepository.AddAsync(student);
            await _unitOfWork.CommitAsync(ct);

            // handle token generation
            var authTokenResult = await _tokenService.GenerateTokenAsync(user);
            var accessToken = authTokenResult.Token;
          
            var refreshTokenResult = await _tokenService.GenerateRefreshTokenAsync();
            var refreshToken = refreshTokenResult.RefreshToken;

            await _refreshTokenRepository.SaveRefreshTokenAsync(user.Id, refreshToken,
                DateTime.UtcNow.AddDays(1));


            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = "Student",
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var authResult = AuthResult.Success(accessToken, refreshToken, userDto);
            return Result<AuthResult>.FromValue(authResult);

           
        }
    }


}
