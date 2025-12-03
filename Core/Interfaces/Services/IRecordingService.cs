using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IRecordingService
    {
        Task HandleRecordingCompletedAsync(long meetingId, string recordingId, string fileUrl,
                                           string fileType, long fileSize,
                                           DateTime start, DateTime end , CancellationToken ct);
    }
 
}
