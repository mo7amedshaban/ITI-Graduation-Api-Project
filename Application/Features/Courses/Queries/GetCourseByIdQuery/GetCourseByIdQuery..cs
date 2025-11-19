using Application.Features.Courses.DTOs;
using MediatR;

namespace Application.Features.Courses.Queries.GetCourseByIdQuery;

public record GetCourseByIdQuery(int id) : IRequest<CourseDto>
{
}