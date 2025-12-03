using Core.Common.Results;
using Core.DTOs;
using MediatR;
using System;

namespace Application.Features.Zoom.Commands.CreateZoomMeeting
{
    public record CreateZoomMeetingCommand : IRequest<Result<ZoomMeetingResponse>>
    {
        public string UserId { get; init; }
        public string Topic { get; init; }
        public DateTime StartTime { get; init; }
        public int Duration { get; init; }
        public string Timezone { get; init; }
        public string Agenda { get; init; }
        public string AutoRecording { get; init; }
        public bool PreSchedule { get; init; }
    }
}
