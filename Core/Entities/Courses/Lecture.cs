using Core.Common;
using Core.Common.Results;
using Core.Entities.Zoom;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Courses;


public partial class Lecture : AuditableEntity
{
    #region Properties & Navigation

    public string Title { get; private set; } = default!;
    public string? RecordingUrl { get; private set; }
    public bool? IsRecordedAvailable { get; set; }
    public DateTimeOffset ScheduledAt { get; private set; }
    public TimeSpan Duration { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public bool IsPublished { get; private set; } = false;
    public bool IsFree { get; private set; } = false;

    // Foreign Key
    public Guid? CourseId { get; private set; }
    public Course Course { get; private set; } = default!;

    [ForeignKey("ZoomMeetingId")]
    public Guid ZoomMeetingId { get; set; }
    public ZoomMeeting ZoomMeeting { get; set; }


    public Guid ModuleId { get; set; }

    public Module Modules { get; set; }


    public Guid? ZoomRecoredId { get; set; }
    public ZoomRecording? ZoomRecording { get; set; }



    #endregion


    #region Constructors

    private Lecture() : base() { }

    private Lecture(string title, DateTimeOffset scheduledAt, TimeSpan duration, Guid moduleId, Guid zoomMeetId, bool isFree = true, bool isPublished = false)
: base(Guid.NewGuid())
    {
        Title = title;
        ScheduledAt = scheduledAt;
        Duration = duration;
        ModuleId = moduleId;
        ZoomMeetingId = zoomMeetId;
        IsFree = isFree;
        IsPublished = isPublished;
        // Initialize non-nullable navigation properties to default values
        ZoomMeeting = default!;
        Modules = default!;
    }



    #endregion
    #region Factory Methods

    public static Result<Lecture> Create(string title,
        DateTimeOffset scheduledAt, TimeSpan duration, Guid moduleId, Guid zoomMeetId,
        bool isFree = true, bool isPublished = false)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<Lecture>.FromError(
                Error.Validation("Lecture.Title.Empty", "Lecture title is required."));

        if (moduleId == Guid.Empty)
            return Result<Lecture>.FromError(
                Error.Validation("Lecture.Module.Empty", "Module is required."));

        if (duration.TotalMinutes <= 0)
            return Result<Lecture>.FromError(
                Error.Validation("Lecture.Duration.Invalid", "Duration must be greater than 0."));

        var lecture = new Lecture(title, scheduledAt, duration, moduleId, zoomMeetId, isFree, isPublished);
    
        return Result<Lecture>.FromValue(lecture);
    }

    #endregion

    #region Domain Behaviors

    public Result<Success> MarkAsCompleted(string contentUrl)
    {
        if (string.IsNullOrWhiteSpace(contentUrl))
            return Result<Success>.FromError(
                Error.Validation("Lecture.ContentUrl.Empty",
                "ContentUrl is required."));

        RecordingUrl = contentUrl;
        IsCompleted = true;

       return Result<Success>.FromValue(new Success());


    }

    public Result<Success> Reschedule(DateTimeOffset newSchedule)
    {
        if (newSchedule <= DateTimeOffset.UtcNow)
            return Result<Success>.FromError(
                Error.Validation("Lecture.Schedule.Invalid",
                "Schedule must be in the future."));

        ScheduledAt = newSchedule;

        return Result<Success>.FromValue(new Success());
    }

    public Result<Success> Publish()
    {
        if (IsPublished == true)
            return Result<Success>.FromError(Error.Validation("Lecture.Publish.Invalid",
                "Lecture is already published."));

        IsPublished = true;
        return Result<Success>.FromValue(new Success());
    }

    public Result<Success> Unpublish()
    {
        if (IsPublished != true)
            return Result<Success>.FromError(Error.Validation("Lecture.Publish.Invalid",
                "Lecture is not published."));

        IsPublished = false;
        return Result<Success>.FromValue(new Success());
    }

    #endregion
}
