using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Index("CourseId", Name = "IX_Modules_CourseId")]
public partial class Module
{
    [Key]
    public Guid Id { get; set; }

    public string Namw { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid CourseId { get; set; }

    public DateTimeOffset? CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Modules")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Module")]
    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
