using FluentValidation;
using System;


namespace Application.Features.Students.Queries.Students.GetStudentById
{
    public class GetStudentByIdQueryValidator : AbstractValidator<GetStudentByIdQuery>
    {
        public GetStudentByIdQueryValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required")
                .NotEqual(Guid.Empty).WithMessage("Student ID cannot be empty");
        }
    }
}
