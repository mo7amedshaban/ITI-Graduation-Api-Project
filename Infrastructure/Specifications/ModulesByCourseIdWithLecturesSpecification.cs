using Core.Common.Specifications;
using Core.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class ModulesByCourseIdWithLecturesSpecification : BaseSpecification<Module>
    {
        public ModulesByCourseIdWithLecturesSpecification(Guid courseId)
             : base(m => m.CourseId == courseId)
        {
            AddInclude(m => m.Lectures);
            ApplyOrderBy(m => m.Title);
            AsNoTrackingEnabled();
        }
    }
}
