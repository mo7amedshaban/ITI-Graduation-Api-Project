using Application.Features.Enrollments.Dto;
using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Commands.CreateEnrollment
{
    public record EnrollInCourseCommand(Guid StudentId, Guid CourseId , 
        DateTimeOffset EnrollmentDate) : IRequest<Result<EnrollmentDto>>;
}
