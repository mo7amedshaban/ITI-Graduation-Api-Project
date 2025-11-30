using Core.Common.Results;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.Students.RemoveStudent
{
    public record DeleteStudentCommand(Guid StudentId) : IRequest<Result<bool>>;
}
