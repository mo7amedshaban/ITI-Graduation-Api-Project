using Core.Common;
using Core.Entities.Courses;
using Core.Entities.Identity;

namespace Core.Entities;

public class Certificate: AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string CertificateNumber { get; set; } = Guid.NewGuid().ToString();

    // Navigation
    public ApplicationUser User { get; set; }
    public Course Course { get; set; }
}