using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Authantication.Instructor.Command.Login;

public record LoginInstructorCommand(LoginRequest login) : IRequest<Result<AuthResult>>;