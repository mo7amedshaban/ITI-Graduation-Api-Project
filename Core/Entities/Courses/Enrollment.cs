using Core.Common;
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


}
