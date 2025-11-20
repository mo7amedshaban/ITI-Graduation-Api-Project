using Core.Entities.Students;
using Core.Interfaces;
using Infrastructure.Common.GenRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.interfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetWithEnrollmentsAsync(Guid studentId, CancellationToken ct = default);
        public Task<Student?> GetByIdWithUserAsync(Guid id, CancellationToken ct);
        public Task<List<Student>> GetAllWithEnrollmentsAsync(CancellationToken ct = default);
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(Guid courseId);

        Task<bool> ExistsAsync(Guid studentId, CancellationToken ct = default);
    }
}
