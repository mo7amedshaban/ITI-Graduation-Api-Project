using Core.Common;
using Core.Common.Results;
using Core.Entities.Exams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Students;


public partial class StudentAnswer : AuditableEntity
{
    public Guid ExamResultId { get; private set; }
    public Guid QuestionId { get; private set; }
    public Guid? SelectedAnswerId { get; private set; }
    public bool IsCorrect { get; private set; }
    public DateTimeOffset AnsweredAt { get; private set; }

    // Navigation Properties
    public ExamResult ExamResult { get; private set; } = default!;
    public Question Question { get; private set; } = default!;
    public AnswerOption? SelectedAnswer { get; private set; }

    private StudentAnswer() { }

    private StudentAnswer(Guid examResultId, Guid questionId,
        Guid? selectedAnswerId, DateTimeOffset answeredAt)
    {
        ExamResultId = examResultId;
        QuestionId = questionId;
        SelectedAnswerId = selectedAnswerId;
        AnsweredAt = answeredAt;
    }

    public static Result<StudentAnswer> Create(
     Guid examResultId,
     Guid questionId,
     Guid? selectedAnswerId)
    {
        if (examResultId == Guid.Empty)
            return Result<StudentAnswer>.FromError(
                Error.Validation("StudentAnswer.ExamResultId.Empty",
                "ExamResultId is required."));

        if (questionId == Guid.Empty)
            return Result<StudentAnswer>.FromError(
                Error.Validation("StudentAnswer.QuestionId.Empty",
                "QuestionId is required."));

        var studentAnswer = new StudentAnswer(
            examResultId,
            questionId,
            selectedAnswerId,
            DateTimeOffset.UtcNow);

        return Result<StudentAnswer>.FromValue(studentAnswer);
    }

    public void MarkAsCorrect(bool isCorrect)
    {
        IsCorrect = isCorrect;
    }


}
