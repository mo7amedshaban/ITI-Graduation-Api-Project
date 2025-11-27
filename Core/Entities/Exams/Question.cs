using Core.Common;

namespace Core.Entities.Exams;

public class Question : AuditableEntity
{
    public string Text { get; set; } = default!;
    public string? ImageUrl { get; set; } = default!;
    public decimal? Points { get; set; }


    public List<ExamQuestions> ExamQuestions { get; set; } = new();
    public List<AnswerOption> AnswerOptions { get; set; } = new();
}