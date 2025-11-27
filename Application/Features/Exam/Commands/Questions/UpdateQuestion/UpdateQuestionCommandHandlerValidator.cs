using FluentValidation;

namespace Application.Features.Exam.Commands.Questions.UpdateQuestion;

public class UpdateQuestionCommandHandlerValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandHandlerValidator()
    {
        RuleFor(x => x.Dto.Text).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Dto.Points).NotEmpty().WithMessage("Point is required.");
    }
}