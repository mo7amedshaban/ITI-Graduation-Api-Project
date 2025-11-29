using Core.Entities;
using Core.Entities.Courses;
using Core.Entities.Exams;
using Core.Entities.Identity;
using Core.Entities.Students;
using Core.Entities.Zoom;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class AppDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    public AppDBContext()
    {
    }


    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamQuestions> ExamQuestions { get; set; }

    public virtual DbSet<ExamResult> ExamResults { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }

    public virtual DbSet<ZoomMeeting> ZoomMeetings { get; set; }

    public virtual DbSet<ZoomRecording> ZoomRecordings { get; set; }
    public virtual DbSet<Certificate> Certificates { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Important: let IdentityDbContext configure its own entities (roles, user logins, etc.)
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Lecture>()
            .HasOne(l => l.ZoomMeeting)
            .WithOne(z => z.Lecture)
            .HasForeignKey<ZoomMeeting>(z => z.Id);

        modelBuilder.Entity<Lecture>()
            .HasOne(l => l.ZoomRecording)
            .WithOne(z => z.Lecture)
            .HasForeignKey<ZoomRecording>(z => z.Id);

        // Configure ExamQuestions as a join entity with composite key
        modelBuilder.Entity<ExamQuestions>(entity =>
        {
            entity.HasKey(eq => new { eq.ExamId, eq.QuestionId });

            entity.HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: configure property length/order, etc.
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}