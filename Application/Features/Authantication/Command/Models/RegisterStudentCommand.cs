using Application.Features.Authantication.Dtos;
using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authantication.Command.Models
{
    public record RegisterStudentCommand(RegisterRequest Request)
     : IRequest<Result<AuthResult>>;

}
