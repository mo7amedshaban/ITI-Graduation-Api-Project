using Application.Features.Courses.DTOs;
using MediatR;

namespace Application.Features.Courses.Queries.GetCourseByIdQuery;

public record GetCourseByIdQuery(Guid id) : IRequest<CourseDto>;