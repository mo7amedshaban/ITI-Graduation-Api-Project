namespace Application.Common.Dtos;

// Minimal placeholder for Zoom webhook payload used by recording state service interface
public class ZoomWebhookPayload
{
    public string Event { get; set; } = string.Empty;
    public string MeetingId { get; set; } = string.Empty;
    // Add additional properties as needed
}

