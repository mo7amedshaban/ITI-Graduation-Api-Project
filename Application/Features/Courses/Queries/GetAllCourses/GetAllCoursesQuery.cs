using Application.Features.Courses.DTOs;
using MediatR;

namespace Application.Features.Courses.Queries.GetAllCourses;

public record GetAllCoursesQuery : IRequest<List<CourseDto>>;