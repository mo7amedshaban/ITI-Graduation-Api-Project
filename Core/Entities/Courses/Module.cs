using Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Courses;

[Index("CourseId", Name = "IX_Modules_CourseId")]
public partial class Module : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Namw { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid CourseId { get; set; }


    [ForeignKey("CourseId")]
    [InverseProperty("Modules")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Module")]
    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
