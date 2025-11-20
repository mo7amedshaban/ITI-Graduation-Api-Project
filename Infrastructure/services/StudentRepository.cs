using Core.Entities.Students;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Infrastructure.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.services
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {

        public StudentRepository(AppDBContext context) : base(context)
        {


        }

        public async Task<List<Student>> GetAllWithEnrollmentsAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .Include(s => s.User)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Student?> GetWithEnrollmentsAsync(Guid studentId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(s => s.Enrollments).
                 ThenInclude(c => c.Course).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId, ct);
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(Guid courseId)
        {
            return await _dbSet
                .Include(s => s.Enrollments)
                .Where(s => s.Enrollments.Any(e => e.CourseId == courseId))
                .ToListAsync();
        }

        public async Task<Student?> GetByIdWithUserAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet
                .Include(s => s.User)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<bool> ExistsAsync(Guid studentId, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(s => s.Id == studentId, ct);
        }
    }
}
