using Core.Common;
using Core.Entities.Zoom;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Courses;

[Index("CourseId", Name = "IX_Lectures_CourseId")]
[Index("ModuleId", Name = "IX_Lectures_ModuleId")]
public partial class Lecture : AuditableEntity
{
    #region Properties & Navigation

    public string Title { get; private set; } = default!;
    public string? RecordingUrl { get; private set; }
    public bool? IsRecordedAvailable { get; set; }
    public DateTimeOffset ScheduledAt { get; private set; }
    public TimeSpan Duration { get; private set; }
    public bool IsCompleted { get; private set; } = false;

    // Foreign Key
    public Guid CourseId { get; private set; }
    public Course Course { get; private set; } = default!;

    public Guid ZoomMeetingId { get; set; }
    public ZoomMeeting ZoomMeeting { get; set; }


    public Guid ModuleId { get; set; }

    public Module Modules { get; set; }


    public Guid? ZoomRecoredId { get; set; }
    public ZoomRecording? ZoomRecording { get; set; }



    #endregion
}
