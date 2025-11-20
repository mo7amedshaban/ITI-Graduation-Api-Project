using MediatR;

namespace Application.Features.Instructors.Command.RemoveInstructor;

public record RemoveInstructorCommand(Guid InstructorId) : IRequest<bool>;