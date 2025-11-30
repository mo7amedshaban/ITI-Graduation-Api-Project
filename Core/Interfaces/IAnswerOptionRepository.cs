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
    public interface IAnswerOptionRepository : IGenericRepository<AnswerOption> 
    {


        Task<List<AnswerOption>> GetByQuestionIdAsync(Guid questionId, CancellationToken ct);
    }
}
