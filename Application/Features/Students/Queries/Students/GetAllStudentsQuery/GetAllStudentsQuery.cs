
using Application.Features.Students.DTOs;
using Core.Common.Results;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetAllStudentsQuery
{
    public record GetAllStudentsQuery : IRequest<Result<List<StudentDto>>>
    {
        public  Guid userId  { get; set; }
    }

}
