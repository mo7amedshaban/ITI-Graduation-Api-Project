using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.StudentAnswer.SubmitStudentAnswerCommand
{
    public class SubmitStudentAnswerValidator : AbstractValidator<SubmitStudentAnswerCommand>
    {
        public SubmitStudentAnswerValidator()
        {
            RuleFor(x => x.ExamResultId)
                .NotEmpty().WithMessage("ExamResultId is required");

          
        }
    }
}
