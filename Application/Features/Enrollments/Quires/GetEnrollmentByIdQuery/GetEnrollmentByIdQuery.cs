using Application.Features.Enrollments.Dto;
using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Quires.GetEnrollmentByIdQuery
{
    public record GetEnrollmentByIdQuery(Guid EnrollmentId) : IRequest<Result<EnrollmentDetailsDto>>;
}
