using Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Exams;

public class ExamQuestions
{

    public Guid ExamId { get; set; }
    public Guid QuestionId { get; set; }

    public int QuestionOrder { get; set; }



    // Navigation properties
    public Exam Exam { get; set; } = default!;
    public Question Question { get; set; } = default!;
}
