using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Dtos
{
    public class LectureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool? IsRecordedAvailable { get; set; }
        public DateTimeOffset ScheduledAt { get; set; }
        public bool IsCompleted { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid ModuleId { get; set; }
   

        public ZoomMeetingDto? ZoomMeeting { get; set; }
        public ZoomRecordingDto? ZoomRecording { get; set; }
    }

    public class CreateLectureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset ScheduledAt { get; set; }



    }

    public class ZoomMeetingDto
    {
        public Guid Id { get; set; }
        public long ZoomMeetingId { get; set; } = default!;
        public string Topic { get; set; } = default!;
        public string JoinUrl { get; set; } = default!;
        public string StartUrl { get; set; } = default!;
        public string Password { get; set; } = default!;

        public DateTimeOffset StartTime { get; set; }
        public int Duration { get; set; }
    }


    public class ZoomRecordingDto
    {
        public Guid Id { get; set; }
        public string RecordingUrl { get; set; }
        public TimeSpan Duration { get; set; }
    }
   

}
