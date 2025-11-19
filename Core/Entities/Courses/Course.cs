using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Core.Common;
using Core.Entities.Exams;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Courses;

[Index("InstructorId", Name = "IX_Courses_InstructorId")]
public class Course : AuditableEntity
{
    #region Prop & Nav

    public const string TypeFree = "Free";

    public const string TypePaid = "Paid";
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ContentType { get; set; }
    public string? CourseDetails { get; set; }
    public string TypeStatus { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    //Nav 
    [ForeignKey("Instructor")] public Guid? InstructorId { get; set; }

    public Instructor Instructor { get; set; } = default!;

    public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    [JsonIgnore] public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public ICollection<Module> Modules { get; set; } = new List<Module>();

    #endregion
}