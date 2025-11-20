using Core.Common;
using Core.Entities.Courses;
using Core.Entities.Exams;
using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities.Students;

public partial class Student : AuditableEntity
{
    [ForeignKey("User")]
    public Guid UserId { get; private set; }

  


    // Nav properties
    public ApplicationUser User { get; private set; } = default!;
    [JsonIgnore]
    public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();
    public ICollection<ExamResult> ExamResults { get; private set; } = new List<ExamResult>();


    // 
     
    public Student(Guid userId)
    {
        UserId = userId;
    }

}
