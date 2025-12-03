using Core.Common.Results;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Commands.RemoveLectures
{
    public class DeleteLectureCommand : IRequest<Result<bool>>
    {
        public Guid LectureId { get; init; }
    }
}
