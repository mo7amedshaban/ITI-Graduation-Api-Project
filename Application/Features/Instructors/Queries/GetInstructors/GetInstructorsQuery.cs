using Application.Features.Instructors.DTOs;
using MediatR;

namespace Application.Features.Instructors.Queries.GetInstructors;

public record GetInstructorsQuery : IRequest<List<InstructorDto>>;