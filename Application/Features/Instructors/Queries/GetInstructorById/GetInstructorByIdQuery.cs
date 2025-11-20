using Application.Features.Instructors.DTOs;
using MediatR;

namespace Application.Features.Instructors.Queries.GetInstructorById;

public record GetInstructorByIdQuery(Guid Id) : IRequest<InstructorDto>;