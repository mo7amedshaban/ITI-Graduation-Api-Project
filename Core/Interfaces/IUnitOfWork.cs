using Core.Entities;
using Core.Entities.Courses;
using Core.Entities.Exams;
using Core.Entities.Identity;
using Core.Entities.Students;
using Core.Entities.Zoom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Core.Interfaces;

public interface IUnitOfWork : IAsyncDisposable,IDisposable
{
    IGenericRepository<Course> Courses { get; } //property for Book repository
    IGenericRepository<Instructor> Instructors { get; }
    int Complete();
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    IGenericRepository<ApplicationUser> ApplicationUsers { get; }
    IGenericRepository<Student> Students { get; }
    IGenericRepository<StudentAnswer> StudentAnswers { get; }
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Exam> Exams { get; }
    IGenericRepository<Question> Questions { get; }
    IGenericRepository<AnswerOption> AnswerOptions { get; }
    IGenericRepository<Lecture> Lectures { get; }
    IGenericRepository<Instructor> Instructors { get; }
    IGenericRepository<Enrollment> Enrollments { get; }
    IGenericRepository<ExamResult> ExamResults { get; }
    IGenericRepository<ZoomRecording> ZoomRecordes { get; }
    IGenericRepository<ZoomMeeting> ZoomMeetings { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the database.
    /// </summary>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves changes synchronously. (Use rarely)
    /// </summary>
    int Complete();

    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the active transaction.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the active transaction.
    /// </summary>
    Task RollbackTransactionAsync();
}

