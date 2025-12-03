using Core.Common.Specifications;
using Core.Entities.Courses;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class CourseWithModulesLecturesAndEnrollmentSpecification : BaseSpecification<Enrollment>
    {
        public CourseWithModulesLecturesAndEnrollmentSpecification(Guid studentId, Guid courseId)
            : base(e => e.StudentId == studentId && e.CourseId == courseId)
        {
            // Include the course and its related data: modules and lectures
            AddInclude(e => e.Course); // Include the course itself
            AddInclude($"{nameof(Enrollment.Course)}.{nameof(Course.Modules)}"); // Include course modules
            AddInclude($"{nameof(Enrollment.Course)}.{nameof(Course.Modules)}.{nameof(Module.Lectures)}"); // Include lectures in the modules
        }
    }
}
