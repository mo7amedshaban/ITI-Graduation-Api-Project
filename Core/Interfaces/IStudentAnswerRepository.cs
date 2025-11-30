using Core.Entities.Students;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IStudentAnswerRepository : IGenericRepository<StudentAnswer> 
    {
    }
}
