using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetStudentWithEnrollmentsQuery
{
    public class GetStudentWithEnrollmentsQueryValidator : AbstractValidator<GetStudentWithEnrollmentsQuery>
    {
        public GetStudentWithEnrollmentsQueryValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Student ID is required");
        }
    }
}
