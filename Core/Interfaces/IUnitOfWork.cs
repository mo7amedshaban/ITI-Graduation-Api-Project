using Core.Entities.Courses;

namespace Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Course> Courses { get; } //property for Book repository
    int Complete();
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}