using Application.Features.Authantication.Dtos;
using Ardalis.Result;
using MediatR;

namespace Application.Features.Authantication.Admin.Command.Register;

public record RegisterAdminCommand(RegisterRequest Request) : IRequest<Result<AuthResult>>;