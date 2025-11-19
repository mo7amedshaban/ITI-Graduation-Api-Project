using Application.Features.Courses.DTOs;
using MediatR;

namespace Application.Features.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand(CourseDto.UpdateCourseDto Dto) : IRequest<CourseDto.UpdateCourseDto>;