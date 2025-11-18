using Core.Common;
using Core.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Zoom;

public partial class ZoomMeeting : AuditableEntity
{
    public long ZoomMeetingId { get; set; } = default!;
    public string Topic { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public int Duration { get; set; } // minutes
    public string JoinUrl { get; set; } = default!;
    public string StartUrl { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Status { get; set; } = "waiting";


    public Guid? CourseId { get; set; }

    public Lecture? Lecture { get; set; }



    public Course? Course { get; private set; }
    public ICollection<ZoomRecording> Recordings { get; private set; } = new List<ZoomRecording>();

}
