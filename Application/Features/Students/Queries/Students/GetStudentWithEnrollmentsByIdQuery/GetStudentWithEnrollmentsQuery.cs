
using Application.Features.Students.DTOs;
using Core.Common.Results;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetStudentWithEnrollmentsQuery
{
    public record GetStudentWithEnrollmentsQuery(Guid StudentId)
     : IRequest<Result<StudentWithEnrollmentsDto>>
    {
        public string CacheKey => $"student-enrollments:{StudentId}";
        public TimeSpan Expiration => TimeSpan.FromMinutes(15);
        public string[]? Tags => new[] { "students", $"student:{StudentId}", "enrollments" };
    }
}
