using Core.Common;
using Core.Entities.Exams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Students;

[Index("ExamResultId", Name = "IX_StudentAnswers_ExamResultId")]
[Index("QuestionId", Name = "IX_StudentAnswers_QuestionId")]
[Index("SelectedAnswerId", Name = "IX_StudentAnswers_SelectedAnswerId")]
public partial class StudentAnswer : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid ExamResultId { get; set; }

    public Guid QuestionId { get; set; }

    public Guid? SelectedAnswerId { get; set; }

    public bool IsCorrect { get; set; }

    public DateTimeOffset AnsweredAt { get; set; }


    [ForeignKey("ExamResultId")]
    [InverseProperty("StudentAnswers")]
    public virtual ExamResult ExamResult { get; set; } = null!;

    [ForeignKey("QuestionId")]
    [InverseProperty("StudentAnswers")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("SelectedAnswerId")]
    [InverseProperty("StudentAnswers")]
    public virtual AnswerOption? SelectedAnswer { get; set; }
}
