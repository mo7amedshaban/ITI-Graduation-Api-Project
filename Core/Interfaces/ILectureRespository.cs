using Core.Entities.Courses;
using Core.Interfaces;



namespace Infrastructure.Interface
{
    public interface ILectureRespository : IGenericRepository<Lecture>
    {
        Task<Lecture?> GetLectureWithZoomAsync(Guid lectureId);
    }
}
