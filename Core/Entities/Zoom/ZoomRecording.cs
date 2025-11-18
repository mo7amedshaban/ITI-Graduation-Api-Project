using Core.Common;
using Core.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Zoom;


public partial class ZoomRecording : AuditableEntity
{
    #region Prop

    public string RecordingId { get; private set; } = default!;
    public long ZoomMeetingId { get; private set; } = default!;
    public string FileUrl { get; private set; } = default!;
    public string FileType { get; private set; } = default!;
    public long FileSize { get; private set; }
    public int Duration { get; private set; }
    public DateTime RecordingStart { get; private set; }
    public DateTime RecordingEnd { get; private set; }
    //public RecordingStatus Status { get; private set; } = RecordingStatus.Pending;
    public string? ProcessedUrl { get; private set; }
    public string? ThumbnailUrl { get; private set; }





    // Navigation property
    public ZoomMeeting ZoomMeeting { get; private set; } = default!;
    public Lecture? Lecture { get; set; }

    #endregion
}
