using Core.Common.Results;
using Core.Entities.Courses;

using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly AppDBContext _context;
        public EnrollmentRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
        public  Task<bool> ExistsAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
        {
            _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId, ct);
              return Task.FromResult(true);
        }

        public async Task<Result<Enrollment>> GetByIdWithDetailsAsync(Guid enrollmentId, CancellationToken ct = default)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId, ct);

            if (enrollment == null)
                return Result<Enrollment>.FromError(
                    Error.NotFound("Enrollment.NotFound", "Enrollment not found"));

            return Result<Enrollment>.FromValue(enrollment);

            //if (enrollment == null)
            //    return Task.FromResult(Result<Enrollment>.FromError(
            //        Error.NotFound("Enrollment.NotFound", "Enrollment not found")));
            //return Task.FromResult(Result<Enrollment>.FromValue(enrollment.Result));
        }

        public async Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId,
               Guid courseId, DateTimeOffset enrollmentDate, CancellationToken ct)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId
                && e.CourseId == courseId && e.EnrollmentDate == enrollmentDate, ct);
        }
    }
}
