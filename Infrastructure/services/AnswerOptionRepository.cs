using Core.Entities.Exams;
using Domain.Common.Interface;
using Domain.Entities.Exams;
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
    public class AnswerOptionRepository : GenericRepository<AnswerOption> , IAnswerOptionRepository
    {
        private readonly AppDBContext _context;
        public AnswerOptionRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<AnswerOption>> GetByQuestionIdAsync(Guid questionId, CancellationToken ct)
        {
            return await _context.AnswerOptions
                 .Where(a => a.QuestionId == questionId)
                 .ToListAsync(ct);
        }
    }
}
