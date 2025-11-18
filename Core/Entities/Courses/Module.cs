using Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Courses;


public partial class Module : AuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    [ForeignKey("Course")]
    public Guid CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();


}
