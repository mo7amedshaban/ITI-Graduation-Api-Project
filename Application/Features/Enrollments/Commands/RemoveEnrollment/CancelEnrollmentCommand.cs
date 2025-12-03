using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Commands.RemoveEnrollment
{

    public class CancelEnrollmentCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; init; } 
        public string? CancellationReason { get; init; }

        public CancelEnrollmentCommand()
        {

        }
        public CancelEnrollmentCommand(Guid id, string? cancellationReason = null)
        {
            Id = id; 
            CancellationReason = cancellationReason;
        }
    }


}
