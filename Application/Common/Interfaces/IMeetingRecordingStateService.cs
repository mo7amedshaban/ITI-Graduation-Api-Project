using Application.Common.Dtos;

namespace Application.Common.Interfaces;

public interface IMeetingRecordingStateService
{
    void MarkMeetingEnded(string meetingId, ZoomWebhookPayload payload);
    void MarkRecordingCompleted(string meetingId, ZoomWebhookPayload payload);
    bool IsRecordingPending(string meetingId);
    ZoomWebhookPayload? GetMeetingEndPayload(string meetingId);
    void RemoveMeetingState(string meetingId);
}

