using Core.Entities.Exams;
using Core.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        public Task<ExamResult> GetByStudentAndExamAsync(Guid studentId, Guid examId,
            CancellationToken ct = default);
        Task<List<ExamResult>> GetByStudentIdAsync(Guid studentId, CancellationToken ct);
        Task<ExamResult?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct);
    }
}
