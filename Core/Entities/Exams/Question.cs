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
    #region Properties
    public string Text { get; private set; } = default!;
    public string? ImageUrl { get; private set; } = default!;
    public decimal? Points { get; private set; }


    #endregion

    #region MTOM


    private readonly List<ExamQuestions> _examQuestions = new();
    public IReadOnlyList<ExamQuestions> ExamQuestions => _examQuestions.AsReadOnly();
    #endregion

    #region Navigation
    private readonly List<AnswerOption> _answerOptions = new();
    public IReadOnlyCollection<AnswerOption> AnswerOptions => _answerOptions.AsReadOnly();
    #endregion

}
