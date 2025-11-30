using Core.Entities.Exams;

using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
    {
        private readonly AppDBContext _context;
        public ExamResultRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ExamResult?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct)
        {
            //var examResult = await _context.ExamResults
            //     .Include(er => er.Exam)
            //     .Include(er => er.Student)
            //     .Include(er => er.CorrectAnswers)
            //     .FirstOrDefaultAsync(er => er.Id == id, ct);
            // return examResult;


            return await _context.ExamResults
           .Where(er => er.Id == id)
           .Include(er => er.Exam)
               .ThenInclude(e => e.ExamQuestions)
                   .ThenInclude(eq => eq.Question)
                       .ThenInclude(q => q.AnswerOptions)
           .Include(er => er.Student)
           .Include(er => er.StudentAnswers)
               .ThenInclude(sa => sa.Question)
                   .ThenInclude(q => q.AnswerOptions)
           .Include(er => er.StudentAnswers)
               .ThenInclude(sa => sa.SelectedAnswer)
           .FirstOrDefaultAsync(ct);
        }

        public async Task<ExamResult?> GetByStudentAndExamAsync(Guid studentId, Guid examId,
            CancellationToken ct = default)
        {
            var examResult = await _context.ExamResults
                .FirstOrDefaultAsync(er => er.StudentId == studentId && er.ExamId == examId);

            if (examResult == null)
                return null;

            return examResult;



        }

        public async Task<List<ExamResult>> GetByStudentIdAsync(Guid studentId, CancellationToken ct)
        {
            return await _context.ExamResults
                .Where(er => er.StudentId == studentId)
                .Include(er => er.Exam)
                .Include(er => er.Student)
                .OrderByDescending(er => er.SubmittedAt) 
                .ToListAsync(ct);
        }
    }
   
}
