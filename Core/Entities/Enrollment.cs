using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Index("CourseId", Name = "IX_Enrollments_CourseId")]
[Index("StudentId", Name = "IX_Enrollments_StudentId")]
public partial class Enrollment
{
    [Key]
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public DateTimeOffset EnrollmentDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTimeOffset? StatusChangedAt { get; set; }

    public string? StatusReason { get; set; }

    public DateTimeOffset? CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Enrollments")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Enrollments")]
    public virtual Student Student { get; set; } = null!;
}
