using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[PrimaryKey("ExamId", "QuestionId")]
[Index("QuestionId", Name = "IX_ExamQuestions_QuestionId")]
public partial class ExamQuestion
{
    [Key]
    public Guid ExamId { get; set; }

    [Key]
    public Guid QuestionId { get; set; }

    public int QuestionOrder { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("ExamQuestions")]
    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey("QuestionId")]
    [InverseProperty("ExamQuestions")]
    public virtual Question Question { get; set; } = null!;
}
