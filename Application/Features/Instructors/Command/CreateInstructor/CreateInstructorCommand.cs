using Application.Features.Instructors.DTOs;
using MediatR;

namespace Application.Features.Instructors.Command.CreateInstructor;

public record CreateInstructorCommand(CreateInstructorDto Dto) : IRequest<CreateInstructorDto>;