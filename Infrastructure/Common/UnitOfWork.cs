using Infrastructure.Data;
using Core.Interfaces;

namespace Infrastructure.Common;

public class UnitOfWork: IUnitOfWork
{
    private readonly AppDBContext _context;

    public UnitOfWork(AppDBContext context)
    {
        _context = context;
    }

    //public IAuthorRepository Authors => new AuthorRepository(_context);
    //public IBookRepository Books => new BookRepository(_context);

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}