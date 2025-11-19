using Application.Features.Courses.DTOs;
using MediatR;

namespace Application.Features.Courses.Commands.CreateCourse;

public record CreateCourseCommand(
    CourseDto.CreateCourseDto Dto
) : IRequest<CourseDto>;