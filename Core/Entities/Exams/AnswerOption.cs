using Core.Common;

namespace Core.Entities.Exams;

public class AnswerOption : AuditableEntity
{
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = default!;
}