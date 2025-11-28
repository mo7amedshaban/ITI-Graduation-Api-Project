using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Authantication.Admin.Command.Login;

public record LoginAdminCommand(LoginRequest login) : IRequest<Result<AuthResult>>;
