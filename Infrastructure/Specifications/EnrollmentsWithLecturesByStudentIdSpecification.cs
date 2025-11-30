
using Core.Common.Specifications;
using Core.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class EnrollmentsWithLecturesByStudentIdSpecification
    : BaseSpecification<Enrollment>
    {
        public EnrollmentsWithLecturesByStudentIdSpecification(Guid studentId)
            : base(e => e.StudentId == studentId)
        {
            AddInclude(e => e.Course);
            AddInclude($"{nameof(Enrollment.Course)}.{nameof(Course.Modules)}");
            AddInclude($"{nameof(Enrollment.Course)}.{nameof(Course.Modules)}.{nameof(Module.Lectures)}");

           
        }
    }

}
