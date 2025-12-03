
using Application.Features.Students.DTOs;
using Core.Common.Results;
using MediatR;
using System;


namespace Application.Features.Students.Queries.Students.GetStudentById
{
    public record GetStudentByIdQuery(Guid StudentId)
     : IRequest<Result<StudentDto>>
    {
        public string CacheKey => $"student:{StudentId}";
        public TimeSpan Expiration => TimeSpan.FromMinutes(30);
        public string[]? Tags => new[] { "students", $"student:{StudentId}" };
    }
}
