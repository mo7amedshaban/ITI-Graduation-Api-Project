using Core.Common;
using Core.Entities.Courses;
using Core.Entities.Exams;
using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Students;

[Index("UserId", Name = "IX_Students_UserId", IsUnique = true)]
public partial class Student : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? Gender { get; set; }

   

    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Student")]
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual ApplicationUser User { get; set; } = null!;
}
