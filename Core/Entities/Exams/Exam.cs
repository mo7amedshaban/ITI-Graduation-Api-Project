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
    #region Properties

    public string Title { get; private set; }
    public string? Description { get; private set; }

    public Guid CourseId { get; private set; }
    public Course Course { get; private set; } = default!;

    public int DurationMinutes { get; private set; }
    public bool IsPublished { get; private set; }

    public DateTimeOffset? PublishedAt { get; private set; } // new 

    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    //

    private readonly List<ExamQuestions> _examQuestions = new();

    public IReadOnlyList<ExamQuestions> ExamQuestions => _examQuestions.AsReadOnly();


    private readonly List<ExamResult> _examResults = new();
    public IReadOnlyCollection<ExamResult> ExamResults => _examResults.AsReadOnly();

    #endregion
}
