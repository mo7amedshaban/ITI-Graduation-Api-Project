using Application.Features.Students.Dtos;
using Application.Features.Students.DTOs;
using Core.Common.Results;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetStudentCourseLecture
{
    public record GetStudentLecturesQuery(Guid StudentId)
     : IRequest<Result<List<StudentCourseLecturesDto>>>;

}
