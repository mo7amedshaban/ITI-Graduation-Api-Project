
using Core.Entities.Courses;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Infrastructure.Interface;

namespace Infrastructure.Services
{

    public class LectureRespository : GenericRepository<Lecture>, ILectureRespository
    {

        public LectureRespository(AppDBContext context) : base(context)
        {

        }

        public Task<Lecture?> GetLectureWithZoomAsync(Guid lectureId)
        {
            throw new NotImplementedException();
        }
    }
   
}
