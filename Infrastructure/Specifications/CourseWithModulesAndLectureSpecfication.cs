using Core.Common.Specifications;
using Core.Entities.Courses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class CourseWithModulesAndLectureSpecification : BaseSpecification<Course>
    {
        public CourseWithModulesAndLectureSpecification(Guid courseId)
            : base(c => c.Id == courseId)
        {
            AddInclude(c => c.Instructor);

            AddInclude(c => c.Modules);      
            AddInclude("Modules.Lectures");
        }
    }

}
