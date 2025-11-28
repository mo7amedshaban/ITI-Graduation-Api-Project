using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Authantication.Instructor.Command.Register;

public record RegisterInstructorCommand(RegisterRequest Request) : IRequest<Result<AuthResult>>;