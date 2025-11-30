using Core.Entities.Zoom;
using Core.Entities.Zoom.Enum;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.ViemoService;
using Infrastructure.ZoomServices.RecordingService.BackgroundTask;
using Infrastructure.ZoomServices.RecordingService.Helper;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ZoomServices.RecordingService
{
    public class RecordingService : IRecordingService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        //private readonly VimeoService _vimeoService;
        private readonly WistiaService _wistiaService;

        private readonly DownloadZoomFileAsyncwithAuth _downloadFile;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecordingService> _logger;
        private readonly HybridCache _cache;

        public RecordingService(
            IBackgroundTaskQueue taskQueue,
            //VimeoService vimeoService,
            WistiaService wistiaService,
            DownloadZoomFileAsyncwithAuth downloadFile,
            ILogger<RecordingService> logger,
            IUnitOfWork unitOfWork,
            HybridCache cache)
        {
            _taskQueue = taskQueue;
            //_vimeoService = vimeoService;
            _wistiaService = wistiaService;
            _downloadFile = downloadFile;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleRecordingCompletedAsync(
                long meetingId,
                string recordingId,
                string fileUrl,
                string fileType,
                long fileSize,
                DateTime start,
                DateTime end,
                CancellationToken ct)
        {
            // 1️⃣ Find Zoom meeting
                var zoomMeetings = await _unitOfWork.ZoomMeetings
                                               .FindAsync(m => m.ZoomMeetingId == meetingId);

            var zoomMeeting = zoomMeetings is IQueryable<ZoomMeeting> queryable // DOTO
                ? queryable.FirstOrDefault()
                : zoomMeetings is IEnumerable<ZoomMeeting> enumerable
                    ? enumerable.FirstOrDefault()
                    : zoomMeetings;
                if (zoomMeeting == null)
                {
                    _logger.LogWarning("Zoom meeting not found: {MeetingId}", meetingId);
                    return;
                }

            // 2️⃣ Create recording entity
            var recording = ZoomRecording.Create(
                recordingId,
                meetingId,
                fileUrl,
                fileType,
                fileSize,
                (int)(end - start).TotalSeconds,
                start,
                end);

            await _unitOfWork.ZoomRecordes.AddAsync(recording);
            await _unitOfWork.CommitAsync(ct);

            // 3️⃣ Cache initial status
            await _cache.SetAsync($"recording:{recordingId}", new { status = "Pending" });

            // 4️⃣ Queue background task
            await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                int maxRetries = 3;
                int attempt = 0;
                bool success = false;

                while (attempt < maxRetries && !success)
                {
                    try
                    {
                        attempt++;
                        _logger.LogInformation("Processing recording {RecordingId}, attempt {Attempt}",
                            recordingId, attempt);

                        // 4a. Download Zoom file
                        var localFilePath = await _downloadFile.DownloadZoomFileAsync(fileUrl, recordingId,
                            fileType, token );

                        // 4b. Upload to Vimeo
                        //var embedUrl = await _vimeoService.UploadVideoAsync(localFilePath, 
                        //    $"{zoomMeeting.Topic}-{recordingId}");

                        // Upload for wisita
                        var embedUrl = await _wistiaService.UploadVideoAsync(localFilePath,
                             $"{zoomMeeting.Topic}-{recordingId}");

                        // 5️⃣ Update status to Processing → Processed
                        recording.UpdateStatus(RecordingStatus.Processing, processedUrl: embedUrl);
                        await _unitOfWork.CommitAsync(token);

                        await _cache.SetAsync($"recording:{recordingId}", 
                            new { status = "Processed", url = embedUrl });

                        success = true;

                        // Optional: delete local file
                        if (File.Exists(localFilePath))
                            File.Delete(localFilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing recording {RecordingId}, attempt {Attempt}",
                            recordingId, attempt);

                        if (attempt >= maxRetries)
                        {
                            recording.UpdateStatus(RecordingStatus.Failed);
                            await _unitOfWork.CommitAsync(token);
                            await _cache.SetAsync($"recording:{recordingId}",
                                new { status = "Failed", error = ex.Message });
                        }
                    }
                }
            });
        }
      

    }

}

