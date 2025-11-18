using Core.Common;
using Core.Entities.Courses;
using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Index("UserId", Name = "IX_Instructors_UserId", IsUnique = true)]
public partial class Instructor : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;


    [InverseProperty("Instructor")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [ForeignKey("UserId")]  
    public virtual ApplicationUser User { get; set; } = null!;

}
