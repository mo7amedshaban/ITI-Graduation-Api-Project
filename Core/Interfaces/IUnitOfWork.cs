using Core.Entities;
using Core.Entities.Courses;

namespace Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Course> Courses { get; } //property for Book repository
    IGenericRepository<Instructor> Instructors { get; }
    int Complete();
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}