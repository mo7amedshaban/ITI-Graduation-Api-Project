
using Core.Common.Results;
using Core.Interfaces.Services;
using Infrastructure.Interface;

using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Zoom.Commands.RemoveZoomMeeting
{
    public class DeleteZoomMeetingCommandHandler : IRequestHandler<DeleteZoomMeetingCommand, Result<bool>>
    {
        private readonly IZoomService _zoomService;
        private readonly IZoomMeetingRepository _meetingRepository;
        private readonly HybridCache _cacheService;
        private readonly ILogger<DeleteZoomMeetingCommandHandler> _logger;

        public DeleteZoomMeetingCommandHandler(
            IZoomService zoomService,
            IZoomMeetingRepository meetingRepository,
            HybridCache cacheService,
            ILogger<DeleteZoomMeetingCommandHandler> logger)
        {
            _zoomService = zoomService;
            _meetingRepository = meetingRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteZoomMeetingCommand request, CancellationToken ct)
        {
            try
            {
                // First, check if the meeting exists in our database and belongs to the user
                var meeting = await _meetingRepository.GetMeetingAsync(request.MeetingId, ct);
                if (meeting == null)
                {
                    return Result<bool>.FromError(Error.Failure("Meeting not found or you don't " +
                        "have permission to delete it"));
                }

                // Check if meeting is in a deletable state
                //if (!IsMeetingDeletable(meeting))
                //{
                //    return Result<bool>.FromError(Error.Failure("Meeting cannot be deleted in its current state"));
                //}

                // Delete from Zoom API
                //var zoomDeleted = await _zoomService.DeleteMeetingAsync(meeting.ZoomMeetingId, ct);
                //if (!zoomDeleted)
                //{
                //    _logger.LogWarning("Failed to delete meeting from Zoom API, but proceeding with " +
                //        "local deletion. Meeting " +
                //        "ID: {MeetingId}", request.MeetingId);
                //    // You might want to handle this differently based on your business requirements
                //}

                // Delete from our database
                await _meetingRepository.DeleteMeetingAsync(request.MeetingId, ct);

                // Invalidate relevant caches
                //await _cacheService.RemoveAsync(request.UserId, meeting.Id, ct);

                _logger.LogInformation("Meeting {MeetingId} deleted successfully for user {UserId}",
                    request.MeetingId, request.UserId);

                return Result<bool>.FromValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting meeting {MeetingId} for user {UserId}",
                    request.MeetingId, request.UserId);

                return Result<bool>.FromError(Error.Failure($"An error occurred while" +
                    $" deleting the meeting: {ex.Message}"));
            }
        }
    }
}