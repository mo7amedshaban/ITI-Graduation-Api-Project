using Core.Entities;
using Core.Entities.Courses;
using Core.Interfaces;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;

namespace Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDBContext _context;

    public UnitOfWork(AppDBContext context)
    {
        _context = context;
    }

    public IGenericRepository<Instructor> Instructors => new GenericRepository<Instructor>(_context);
    public IGenericRepository<Course> Courses => new GenericRepository<Course>(_context);


    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}