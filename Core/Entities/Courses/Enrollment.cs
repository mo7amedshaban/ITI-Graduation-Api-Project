using Core.Common;
using Core.Common.Results;
using Core.Entities.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities.Courses;

public partial class Enrollment : AuditableEntity
{
    public const string StatusActive = "Active";
    public const string StatusPending = "Pending";

    [ForeignKey("Student")]
    public Guid StudentId { get; private set; }


    [ForeignKey("Course")]
    public Guid CourseId { get; private set; }



    public DateTimeOffset EnrollmentDate { get; private set; }
    public string Status { get; private set; } = "Pending";

    public DateTimeOffset? StatusChangedAt { get; private set; }
    public string? StatusReason { get; private set; }

    // Navigation properties
    [JsonIgnore]
    public Student Student { get; private set; } = default!;
    [JsonIgnore]
    public Course Course { get; private set; } = default!;

    public Enrollment() : base() { }

    private Enrollment(Guid studentId, Guid courseId, string status) : base(Guid.NewGuid())
    {
        StudentId = studentId;
        CourseId = courseId;
        EnrollmentDate = DateTimeOffset.UtcNow;
        Status = status;
        StatusChangedAt = DateTimeOffset.UtcNow;
    }

    #region Factory Method

    public static Result<Enrollment> Create(Guid studentId, Guid courseId, string status = "Pending")
    {
        if (studentId == Guid.Empty)
            return Result<Enrollment>.FromError(
                Error.Validation("Enrollment.StudentId.Empty", "StudentId is required."));

        if (courseId == Guid.Empty)
            return Result<Enrollment>.FromError(
                Error.Validation("Enrollment.CourseId.Empty", "CourseId is required."));

        var validStatuses = new[] { "Pending", "Active", "Cancelled", "Rejected" };
        if (!validStatuses.Contains(status))
            return Result<Enrollment>.FromError(
                Error.Validation("Enrollment.Status.Invalid", "Invalid status."));

        var enrollment = new Enrollment(studentId, courseId, status);
        return Result<Enrollment>.FromValue(enrollment);
    }
    #endregion

    #region Behavouir

    public Result<Success> Activate()
    {
        if (Status == "Active")
            return Result<Success>.FromError(
                Error.Conflict("Enrollment.AlreadyActive", "Enrollment is already active."));

        Status = "Active";
        StatusChangedAt = DateTimeOffset.UtcNow;
        StatusReason = "Approved by admin";

        return Result<Success>.FromValue(new Success());
    }

    public Result<Success> Cancel(string reason = "Cancelled by student")
    {
        if (Status == "Cancelled")
            return Result<Success>.FromError(
                Error.Conflict("Enrollment.AlreadyCancelled", "Enrollment is already cancelled."));

        Status = "Cancelled";
        StatusChangedAt = DateTimeOffset.UtcNow;
        StatusReason = reason;

        return Result<Success>.FromValue(new Success());
    }
    public Result<Success> UpdateStatus(string Status, string Reason)
    {

        return Result<Success>.FromValue(new Success());
    }

    public Result<Success> Reject(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return Result<Success>.FromError(
                Error.Validation("Enrollment.Reason.Empty", "Rejection reason is required."));

        Status = "Rejected";
        StatusChangedAt = DateTimeOffset.UtcNow;
        StatusReason = reason;

        return Result<Success>.FromValue(new Success());
    }


    #endregion
}
