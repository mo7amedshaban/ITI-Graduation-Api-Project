using Core.Entities.Students;

using Infrastructure.Common.GenRepo;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StudentAnswerRepository : GenericRepository<StudentAnswer>, IStudentAnswerRepository
    {
        public StudentAnswerRepository(Data.AppDBContext context) : base(context)
        {
        }
    }
   
}
