using FluentValidation;

namespace Application.Features.Exam.Commands.Questions.CreateQuestion;

public class CreateQuestionCommandHandlerValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandHandlerValidator()
    {
        RuleFor(x => x.dto.Text).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.dto.Points).NotEmpty().WithMessage("Point is required.");
    }
}