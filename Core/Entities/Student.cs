using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Index("UserId", Name = "IX_Students_UserId", IsUnique = true)]
public partial class Student
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? Gender { get; set; }

    public DateTimeOffset? CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Student")]
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual AspNetUser User { get; set; } = null!;
}
