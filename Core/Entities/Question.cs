using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

public partial class Question
{
    [Key]
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Points { get; set; }

    public DateTimeOffset? CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

    [InverseProperty("Question")]
    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    [InverseProperty("Question")]
    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
