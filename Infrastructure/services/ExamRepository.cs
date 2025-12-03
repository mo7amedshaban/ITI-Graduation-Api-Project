using Core.Common.Results;
using Core.Entities.Exams;
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
    public class ExamRepository : GenericRepository<Exam> , IExamRepository
    {
        public ExamRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<Result<List<Exam>>> GetByCourseIdAsync(Guid courseId, CancellationToken ct = default)
        {

            var exams = await _dbSet
                  .Where(e => e.CourseId == courseId)
                  .ToListAsync(ct);

            return Result<List<Exam>>.FromValue(exams);

            

        }

     

        public async Task<Exam> GetByUserIdAsnc(Guid id, CancellationToken ct)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public Task<Result<Exam?>> GetExamWithQuestionsAndAnswersAsync(Guid examId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

      

        public async Task<Exam?> GetExamWithQuestionsAsync(Guid examId, CancellationToken ct)
        {
            var exam = await _dbSet
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        //.ThenInclude(q => q.AnswerOptions) 
                .Where(e => e.Id == examId)
                .FirstOrDefaultAsync(ct);

            return exam;
        }
    }
}
