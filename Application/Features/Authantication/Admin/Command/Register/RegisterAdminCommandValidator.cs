using FluentValidation;

namespace Application.Features.Authantication.Admin.Command.Register;

public class RegisterAdminCommandValidator :AbstractValidator<RegisterAdminCommand>
{
    public RegisterAdminCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is required and must be valid.");

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.Request.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");

        RuleFor(x => x.Request.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");
    }
    
}