using MediatR;

namespace Application.Features.Courses.Commands.RemoveCourse;

public record RemoveCourseCommand(Guid Id) : IRequest<bool>;