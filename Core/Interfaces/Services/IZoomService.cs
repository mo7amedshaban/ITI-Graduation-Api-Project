
using Core.DTOs;
using Core.Entities.Zoom;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IZoomService
    {
        Task<ZoomMeetingResponse> CreateMeetingAsync(ZoomMeetingRequest request, CancellationToken ct);
        Task<ZoomMeeting> GetMeetingAsync(long zoomMeetingId, CancellationToken ct);


    }
}
