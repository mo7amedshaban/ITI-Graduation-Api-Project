using Core.Common;
using Core.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Zoom;

[Index("CourseId", Name = "IX_ZoomMeetings_CourseId")]
public partial class ZoomMeeting : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public long ZoomMeetingId { get; set; }

    public string Topic { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public int Duration { get; set; }

    public string JoinUrl { get; set; } = null!;

    public string StartUrl { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid? CourseId { get; set; }

 

    [ForeignKey("CourseId")]
    [InverseProperty("ZoomMeetings")]
    public virtual Course? Course { get; set; }

    [InverseProperty("ZoomMeetingId1Navigation")]
    public virtual ICollection<ZoomRecording> ZoomRecordings { get; set; } = new List<ZoomRecording>();
}
