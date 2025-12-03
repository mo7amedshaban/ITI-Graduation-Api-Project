using Application.Features.Lectures.Dtos;
using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Commands.createLectures
{
    public record CreateLectureCommand(
     string Title,
     DateTimeOffset ScheduledAt,
     TimeSpan Duration,
     Guid ModuleId,
     Guid ZoomMeetingId,
     Guid ZoomRecoredId,
        bool IsFree = false,
        bool IsPublished = false
     ) : IRequest<Result<LectureDto>>;

}
