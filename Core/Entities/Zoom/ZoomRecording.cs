using Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Zoom;

[Index("ZoomMeetingId1", Name = "IX_ZoomRecordings_ZoomMeetingId1")]
public partial class ZoomRecording : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string RecordingId { get; set; } = null!;

    public long ZoomMeetingId { get; set; }

    public string FileUrl { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public long FileSize { get; set; }

    public int Duration { get; set; }

    public DateTime RecordingStart { get; set; }

    public DateTime RecordingEnd { get; set; }

    public int Status { get; set; }

    public string? ProcessedUrl { get; set; }

    public string? ThumbnailUrl { get; set; }

    public Guid ZoomMeetingId1 { get; set; }

    

    [ForeignKey("ZoomMeetingId1")]
    [InverseProperty("ZoomRecordings")]
    public virtual ZoomMeeting ZoomMeetingId1Navigation { get; set; } = null!;
}
