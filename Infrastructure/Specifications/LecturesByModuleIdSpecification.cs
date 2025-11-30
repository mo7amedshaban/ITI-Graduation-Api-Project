using Core.Common.Specifications;
using Core.Entities.Courses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class LecturesByModuleIdSpecification : BaseSpecification<Lecture>
    {
        public LecturesByModuleIdSpecification(Guid moduleId)
            : base(l => l.ModuleId == moduleId)
        {
            AddInclude(l => l.ZoomMeeting);
            AddInclude(l => l.ZoomRecording);
            ApplyOrderBy(l => l.ScheduledAt);
            AsNoTrackingEnabled();
        }
    }

}
