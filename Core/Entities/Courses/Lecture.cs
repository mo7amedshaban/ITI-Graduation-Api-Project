using Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Courses;

[Index("CourseId", Name = "IX_Lectures_CourseId")]
[Index("ModuleId", Name = "IX_Lectures_ModuleId")]
public partial class Lecture : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ContentUrl { get; set; }

    public Guid CourseId { get; set; }

    public DateTimeOffset ScheduledAt { get; set; }

    public TimeOnly Duration { get; set; }

    public bool IsCompleted { get; set; }

    public Guid? ModuleId { get; set; }

  

    [ForeignKey("CourseId")]
    [InverseProperty("Lectures")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("ModuleId")]
    [InverseProperty("Lectures")]
    public virtual Module? Module { get; set; }
}
