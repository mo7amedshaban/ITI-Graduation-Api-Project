using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Core.Entities.Courses;
using Core.Entities.Identity;

namespace Core.Entities;

public class Instructor : AuditableEntity
{
    [ForeignKey("User")] public Guid? UserId { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }


    public string Title { get; set; } = "Unknown";
    public ApplicationUser User { get; private set; } = default!;
    public ICollection<Course> Courses { get; private set; } = new List<Course>();
}