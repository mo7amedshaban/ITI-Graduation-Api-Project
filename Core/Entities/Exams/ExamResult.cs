using Core.Common;
using Core.Common.Results;
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

    #region Constructors
    private ExamResult() { }

    private ExamResult(Guid studentId, Guid examId, DateTimeOffset? startedAt,
        string status)
    {
        StudentId = studentId;
        ExamId = examId;
        StartedAt = startedAt;
        Status = status;

    }

    private ExamResult(Guid studentId, Guid examId, double score,
        DateTimeOffset? startedAt, string status)
    {
        StudentId = studentId;
        ExamId = examId;
        Score = score;
        StartedAt = startedAt;
        Status = status;
    }
    #endregion

    #region Factory Method

    public static Result<ExamResult> StartExam(Guid studentId, Guid examId)
    {

        var examResult = new ExamResult(
            studentId,
            examId,

            DateTimeOffset.UtcNow,
            "InProgress");

        return Result<ExamResult>.FromValue(examResult);
    }

    public static Result<ExamResult> Create(
        Guid studentId,
        Guid examId,
        double score,
        DateTimeOffset? startedAt,
        string status = "Submitted")
    {

        if (studentId == Guid.Empty)
            return Result<ExamResult>.FromError(
                Error.Validation("ExamResult.StudentId.Empty", "StudentId is required."));

        if (examId == Guid.Empty)
            return Result<ExamResult>.FromError(
                Error.Validation("ExamResult.ExamId.Empty", "ExamId is required."));

        if (score < 0)
            return Result<ExamResult>.FromError(
                Error.Validation("ExamResult.Score.Invalid", "Score must be greater than or equal to 0."));

        if (string.IsNullOrWhiteSpace(status))
            return Result<ExamResult>.FromError(
                Error.Validation("ExamResult.Status.Empty", "Status is required."));


        var examResult = new ExamResult(studentId, examId, score, startedAt, status);

        return Result<ExamResult>.FromValue(examResult);
    }
    #endregion

    #region Behaviors

    public Result<Success> SubmitExam()
    {
        if (Status == "Submitted")
            return Result<Success>.FromError(
                Error.Validation("ExamResult.AlreadySubmitted", "Exam has already been submitted."));

        SubmittedAt = DateTimeOffset.UtcNow;
        Status = "Submitted";


        RecalculateScore();

        return Result<Success>.FromValue(new Success());
    }

    public Result<StudentAnswer> AddStudentAnswer(Guid questionId, Guid? selectedAnswerId, bool isCorrect)
    {
        var studentAnswerResult = StudentAnswer.Create(Id, questionId, selectedAnswerId);

        if (studentAnswerResult.IsError)
            return Result<StudentAnswer>.FromError(studentAnswerResult.TopError);

        var studentAnswer = studentAnswerResult.Value;
        studentAnswer.MarkAsCorrect(isCorrect);

        StudentAnswers.Add(studentAnswer);
        EvaluateAnswer(isCorrect);

        return Result<StudentAnswer>.FromValue(studentAnswer);
    }
    public void UpdateStatistics()
    {

        CorrectAnswers = StudentAnswers.Count(a => a.IsCorrect);
        WrongAnswers = StudentAnswers.Count(a => !a.IsCorrect);
        RecalculateScore();
    }

    public void EvaluateAnswer(bool isCorrect)
    {
        if (isCorrect)
            IncreaseScore();
        else
            RegisterWrongAnswer();
    }

    private void IncreaseScore()
    {
        CorrectAnswers++;
        RecalculateScore();
    }

    private void RegisterWrongAnswer()
    {
        WrongAnswers++;
        RecalculateScore();
    }

    public void SetTotalQuestions(int total)
    {
        if (total <= 0)
            throw new ArgumentException("Total questions must be greater than zero.");

        TotalQuestions = total;
    }


    private void RecalculateScore()
    {
        if (TotalQuestions > 0)
            Score = Math.Round(((double)CorrectAnswers / TotalQuestions) * 100, 2);
    }
    #endregion

}
