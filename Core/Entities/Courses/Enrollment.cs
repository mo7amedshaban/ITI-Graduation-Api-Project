using Core.Common;
using Core.Entities.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Courses;

[Index("CourseId", Name = "IX_Enrollments_CourseId")]
[Index("StudentId", Name = "IX_Enrollments_StudentId")]
public partial class Enrollment : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public DateTimeOffset EnrollmentDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTimeOffset? StatusChangedAt { get; set; }

    public string? StatusReason { get; set; }

  

    [ForeignKey("CourseId")]
    [InverseProperty("Enrollments")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Enrollments")]
    public virtual Student Student { get; set; } = null!;
}
