using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Index("InstructorId", Name = "IX_Courses_InstructorId")]
public partial class Course
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string TypeStatus { get; set; } = null!;

    public DateTimeOffset? StartDate { get; set; }

    public DateTimeOffset? EndDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid InstructorId { get; set; }

    public DateTimeOffset? CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [InverseProperty("Course")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    [ForeignKey("InstructorId")]
    [InverseProperty("Courses")]
    public virtual Instructor Instructor { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

    [InverseProperty("Course")]
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    [InverseProperty("Course")]
    public virtual ICollection<ZoomMeeting> ZoomMeetings { get; set; } = new List<ZoomMeeting>();
}
