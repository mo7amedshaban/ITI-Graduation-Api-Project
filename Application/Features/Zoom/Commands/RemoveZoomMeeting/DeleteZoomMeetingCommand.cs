using Core.Common.Results;

using MediatR;

namespace Application.Features.Zoom.Commands.RemoveZoomMeeting
{
    public record DeleteZoomMeetingCommand : IRequest<Result<bool>>
    {
        public string UserId { get; init; }
        public long MeetingId { get; init; } 

        public DeleteZoomMeetingCommand(string userId, long meetingId)
        {
            UserId = userId;
            MeetingId = meetingId;
        }
    }
}
