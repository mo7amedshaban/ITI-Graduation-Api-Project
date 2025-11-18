using Core.Common;
using Core.Entities.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Exams;

[Index("ExamId", Name = "IX_ExamResults_ExamId")]
[Index("StudentId", Name = "IX_ExamResults_StudentId")]
public partial class ExamResult : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid ExamId { get; set; }

    public int TotalQuestions { get; set; }

    public int CorrectAnswers { get; set; }

    public int WrongAnswers { get; set; }

    public double Score { get; set; }

    public DateTimeOffset? StartedAt { get; set; }

    public string Status { get; set; } = null!;

    public DateTimeOffset? SubmittedAt { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("ExamResults")]
    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("ExamResults")]
    public virtual Student Student { get; set; } = null!;

    [InverseProperty("ExamResult")]
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
