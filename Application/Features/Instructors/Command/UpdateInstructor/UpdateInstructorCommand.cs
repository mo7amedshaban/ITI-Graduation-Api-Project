using Application.Features.Instructors.DTOs;
using MediatR;

namespace Application.Features.Instructors.Command.UpdateInstructor;

public record UpdateInstructorCommand(InstructorDto InstructorDto) : IRequest<InstructorDto>;