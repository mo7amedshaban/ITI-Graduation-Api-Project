using Core.Entities.Zoom;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IZoomMeetingRepository
    {

        Task<ZoomMeeting> CreateMeetingAsync(ZoomMeeting meeting, CancellationToken ct = default);
        Task<ZoomMeeting?> GetMeetingAsync(long meetingId, CancellationToken ct = default);
        Task<bool> UpdateMeetingAsync(ZoomMeeting meeting, CancellationToken ct = default);
        Task<bool> DeleteMeetingAsync(long meetingId, CancellationToken ct = default);
        Task<List<ZoomRecording>> GetRecordingsAsync(long meetingId, CancellationToken ct = default);
        Task<ZoomRecording> SaveRecordingAsync(ZoomRecording recording, CancellationToken ct = default);


    }
}
