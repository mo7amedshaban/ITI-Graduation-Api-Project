
using Application.Features.Students.DTOs;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetAllStudentsWithEnrollmentsQuery
{
    public record GetAllStudentsWithEnrollmentsQuery : IRequest<List<StudentEnrollmentTableDto>>;
}
