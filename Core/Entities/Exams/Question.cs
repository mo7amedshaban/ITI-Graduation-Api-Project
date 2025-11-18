using Core.Common;
using Core.Entities.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Exams;

public partial class Question : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Points { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

    [InverseProperty("Question")]
    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    [InverseProperty("Question")]
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
