using Core.Common.Results;
using Core.Entities.Exams;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        public Task<Exam?> GetExamWithQuestionsAsync(Guid examId, CancellationToken ct = default);
        public Task<Result<Exam?>> GetExamWithQuestionsAndAnswersAsync(Guid examId, CancellationToken ct = default);
        public Task<Result<List<Exam>>> GetByCourseIdAsync(Guid courseId, CancellationToken ct = default);

       

    }
}
