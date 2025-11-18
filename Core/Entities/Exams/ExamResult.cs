using Core.Common;
using Core.Entities.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Exams;


public partial class ExamResult : AuditableEntity
{
    #region Prop

    public Guid StudentId { get; private set; }
    public Guid ExamId { get; private set; }

    public int TotalQuestions { get; private set; }
    public int CorrectAnswers { get; private set; }
    public int WrongAnswers { get; private set; }
    public double Score { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public string Status { get; private set; } = "Submitted";

    #region Additional Properties
    public DateTimeOffset? SubmittedAt { get; private set; }
    public TimeSpan? Duration => SubmittedAt - StartedAt;
    #endregion

    // Navigation Properties
    public Student Student { get; private set; } = default!;

    public Exam Exam { get; private set; } = default!;
    public ICollection<StudentAnswer> StudentAnswers { get; private set; } = new List<StudentAnswer>();


    #endregion
}
