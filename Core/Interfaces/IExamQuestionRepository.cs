using Core.Entities.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IExamQuestionRepository
    {
        Task AddRangeAsync(IEnumerable<ExamQuestions> examQuestions, CancellationToken ct);
    }
}
