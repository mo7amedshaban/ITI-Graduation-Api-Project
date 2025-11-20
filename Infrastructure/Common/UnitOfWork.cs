<<<<<<< HEAD
using Core.Entities.Courses;
using Core.Interfaces;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;

namespace Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
=======
using Core.Entities;
using Core.Entities.Courses;
using Core.Entities.Exams;
using Core.Entities.Identity;
using Core.Entities.Students;
using Core.Entities.Zoom;
using Core.Interfaces;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common;

public class UnitOfWork : IUnitOfWork , IAsyncDisposable
>>>>>>> 655d5c1 (Handle UoW , Create Studednt Repo, Handle Register Configration , Handle Student Registration and Update Token Service)
{
    private readonly AppDBContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;

    public IGenericRepository<ApplicationUser> ApplicationUsers { get; }
    public IGenericRepository<Student> Students { get; }
    public IGenericRepository<StudentAnswer> StudentAnswers { get; }
    public IGenericRepository<Course> Courses { get; }
    public IGenericRepository<Exam> Exams { get; }
    public IGenericRepository<Question> Questions { get; }
    public IGenericRepository<AnswerOption> AnswerOptions { get; }
    public IGenericRepository<Lecture> Lectures { get; }
    public IGenericRepository<Instructor> Instructors { get; }
    public IGenericRepository<Enrollment> Enrollments { get; }
    public IGenericRepository<ExamResult> ExamResults { get; }
    public IGenericRepository<ZoomRecording> ZoomRecordes { get; }
    public IGenericRepository<ZoomMeeting> ZoomMeetings { get; }

    public UnitOfWork(AppDBContext context, ILogger<UnitOfWork> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        Students = new GenericRepository<Student>(context);
        Exams = new GenericRepository<Exam>(context);
        StudentAnswers = new GenericRepository<StudentAnswer>(context);
        Courses = new GenericRepository<Course>(context);
        Lectures = new GenericRepository<Lecture>(context);
        ExamResults = new GenericRepository<ExamResult>(context);
        Questions = new GenericRepository<Question>(context);
        Instructors = new GenericRepository<Instructor>(context);
        Enrollments = new GenericRepository<Enrollment>(context);
        ApplicationUsers = new GenericRepository<ApplicationUser>(context);
        ZoomMeetings = new GenericRepository<ZoomMeeting>(context);
        ZoomRecordes = new GenericRepository<ZoomRecording>(context);
    }

<<<<<<< HEAD
    public IGenericRepository<Course> Courses => new GenericRepository<Course>(_context);
    //public IBookRepository Books => new BookRepository(_context);
=======
    public async Task<int> CommitAsync(CancellationToken ct)
    {
        try
        {
            return await _context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while committing changes");
            throw;
        }
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
>>>>>>> 655d5c1 (Handle UoW , Create Studednt Repo, Handle Register Configration , Handle Student Registration and Update Token Service)

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
            await _transaction.DisposeAsync();

        await _context.DisposeAsync();
    }
}
