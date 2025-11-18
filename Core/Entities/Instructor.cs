using Core.Common;
using Core.Entities.Courses;
using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;


public partial class Instructor : AuditableEntity
{
    [ForeignKey("User")]
    public Guid UserId { get; private set; }
    public string Title { get; set; }
    public ApplicationUser User { get; private set; } = default!;
    public ICollection<Course> Courses { get; private set; } = new List<Course>();



}
