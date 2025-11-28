using FluentValidation;

namespace Application.Features.Authantication.Instructor.Command.Login;

public class LoginInstructorCommandValidator:AbstractValidator<LoginInstructorCommand>
{
    public LoginInstructorCommandValidator()
    {
        RuleFor(x => x.login.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is required and must be valid.");

        RuleFor(x => x.login.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
    
}