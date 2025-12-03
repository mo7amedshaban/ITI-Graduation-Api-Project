using Core.Common.Results;
using Core.Entities.Courses;
using Core.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment> 
    {
        Task<Enrollment> GetByStudentAndCourseAsync(Guid studentId, Guid courseId 
         ,DateTimeOffset enrollmentDate , CancellationToken ct );

        Task<Result< Enrollment>> GetByIdWithDetailsAsync(Guid enrollmentId, CancellationToken ct = default);

        Task<bool> ExistsAsync(Guid studentId, Guid courseId, CancellationToken ct = default);
    }
}
