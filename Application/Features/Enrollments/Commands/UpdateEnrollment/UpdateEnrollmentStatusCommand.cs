using Application.Features.Enrollments.Dto;
using Core.Common.Results;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Commands.UpdateEnrollment
{
    public record UpdateEnrollmentStatusCommand : IRequest<Result<EnrollmentDto>>
    {
        public Guid EnrollmentId { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? Reason { get; init; }
    }
}
