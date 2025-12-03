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
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
       
        public CourseRepository(AppDBContext context) : base(context)
        {
        }


        public async Task<Course?> GetWithLecturesAsync(Guid courseId)
        {
            return await _dbSet
                 .Include(c => c.Modules)
                     .ThenInclude(m => m.Lectures)
                         .ThenInclude(l => l.ZoomMeeting)
                         .ThenInclude(r => r.Recordings)
                 .FirstOrDefaultAsync(c => c.Id == courseId);
        }
        public async Task<Course> GetByIdWithModulesAsync(Guid courseId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(c => c.Modules)  // Load modules
                    .ThenInclude(m => m.Lectures)  // Load lectures for each module
                        .ThenInclude(l => l.ZoomMeeting)  // Load zoom meeting for each lecture
                .AsSplitQuery()  // Optional: Better performance for complex queries
                .FirstOrDefaultAsync(c => c.Id == courseId, ct);
        }
        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _dbSet.Where(c => c.StartDate <= DateTime.UtcNow &&
                                           (c.EndDate == null || c.EndDate >= DateTime.UtcNow))
                               .ToListAsync();
        }

    }
}
