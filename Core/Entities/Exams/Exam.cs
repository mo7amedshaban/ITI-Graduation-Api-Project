using Core.Common;
using Core.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Exams;

[Index("CourseId", Name = "IX_Exams_CourseId")]
public partial class Exam : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid CourseId { get; set; }

    public int DurationMinutes { get; set; }

    public bool IsPublished { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset EndDate { get; set; }

   

    [ForeignKey("CourseId")]
    [InverseProperty("Exams")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Exam")]
    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    [InverseProperty("Exam")]
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
