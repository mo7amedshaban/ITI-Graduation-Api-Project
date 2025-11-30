using Application.Features.Lectures.Dtos;
using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Queries.GetWithModules
{
    public record GetLecturesByModuleIdQuery(Guid ModuleId)
    : IRequest<Result<List<LectureDto>>>;

}
