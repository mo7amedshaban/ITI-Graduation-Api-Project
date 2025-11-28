using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authantication.Instructor.Command.Register;

public class RegisterInstructorCommandHandler : IRequestHandler<RegisterInstructorCommand, Result<AuthResult>>
{
    private readonly IGenericRepository<Core.Entities.Instructor> _instructorRepository;

    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<RegisterRequest> _validator;
    private IRequestHandler<RegisterInstructorCommand, Result<AuthResult>> _requestHandlerImplementation;

    public RegisterInstructorCommandHandler(
        UserManager<ApplicationUser> userManager,
        IGenericRepository<Core.Entities.Instructor> instructorRepository,
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
        _instructorRepository = instructorRepository;
    }

    public async Task<Result<AuthResult>> Handle(RegisterInstructorCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<AuthResult>.Error(errors);
        }

        // check if email exists
        var email = request.Email.ToLowerInvariant();
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
            return Result<AuthResult>.Error("An account with this email already exists.");

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

            return Result<AuthResult>.Error("IDENTITY_CREATION_FAILED");
        }


        await _userManager.AddToRoleAsync(user, "Instructor");


        var instructor = new Core.Entities.Instructor
        {
            UserId = user.Id,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.ToLowerInvariant(),
            PhoneNumber = request.PhoneNumber
        };

        await _instructorRepository.AddAsync(instructor);
        await _unitOfWork.CommitAsync(cancellationToken);

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
            Role = "Instructor",
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        var authResult = AuthResult.Success(accessToken, refreshToken, userDto);
        return Result<AuthResult>.Success(authResult);
    }
}