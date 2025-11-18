using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Core.Entities.Students;
using Microsoft.EntityFrameworkCore;


namespace Core.Entities.Exams;

[Index("QuestionId", Name = "IX_AnswerOptions_QuestionId")]
public partial class AnswerOption : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public Guid QuestionId { get; set; }


    [ForeignKey("QuestionId")]
    [InverseProperty("AnswerOptions")]
    public virtual Question Question { get; set; } = null!;

    [InverseProperty("SelectedAnswer")]
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
