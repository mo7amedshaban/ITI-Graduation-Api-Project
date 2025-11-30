using Core.Entities.Zoom;
using Infrastructure.Data;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ZoomMeetingRepository : IZoomMeetingRepository
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ZoomMeetingRepository> _logger;

        public ZoomMeetingRepository(AppDBContext context, ILogger<ZoomMeetingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ZoomMeeting> CreateMeetingAsync(ZoomMeeting meeting, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Creating Zoom meeting with ID: {MeetingId}", meeting.ZoomMeetingId);

                await _context.ZoomMeetings.AddAsync(meeting, ct);
                _logger.LogInformation("Zoom meeting added to context: {MeetingId}", meeting.ZoomMeetingId);
                return meeting;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Zoom meeting with ID: {MeetingId}", meeting.ZoomMeetingId);
                throw;
            }
        }

        public async Task<ZoomMeeting?> GetMeetingAsync(long meetingId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug("Retrieving Zoom meeting with ID: {MeetingId}", meetingId);

                var meeting = await _context.ZoomMeetings
                    .FirstOrDefaultAsync(m => m.ZoomMeetingId == meetingId, ct);

                if (meeting == null)
                {
                    _logger.LogWarning("Zoom meeting not found with ID: {MeetingId}", meetingId);
                }
                else
                {
                    _logger.LogDebug("Successfully retrieved Zoom meeting with ID: {MeetingId}", meetingId);
                }

                return meeting;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Zoom meeting with ID: {MeetingId}", meetingId);
                throw;
            }
        }

        public async Task<bool> UpdateMeetingAsync(ZoomMeeting meeting, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Updating Zoom meeting with ID: {MeetingId}", meeting.ZoomMeetingId);

                var existingMeeting = await _context.ZoomMeetings
                    .FirstOrDefaultAsync(m => m.ZoomMeetingId == meeting.ZoomMeetingId, ct);

                if (existingMeeting == null)
                {
                    _logger.LogWarning("Cannot update - Zoom meeting not found with ID: {MeetingId}", meeting.ZoomMeetingId);
                    return false;
                }

                // Update properties
                _context.Entry(existingMeeting).CurrentValues.SetValues(meeting);
                // REMOVED: await _context.SaveChangesAsync(ct); - Let UnitOfWork handle the commit

                _logger.LogInformation("Zoom meeting updated in context: {MeetingId}", meeting.ZoomMeetingId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update Zoom meeting with ID: {MeetingId}", meeting.ZoomMeetingId);
                throw;
            }
        }

        public async Task<bool> DeleteMeetingAsync(long meetingId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Deleting Zoom meeting with ID: {MeetingId}", meetingId);

                var meeting = await _context.ZoomMeetings
                    .FirstOrDefaultAsync(m => m.ZoomMeetingId == meetingId, ct);

                if (meeting == null)
                {
                    _logger.LogWarning("Cannot delete - Zoom meeting not found with ID: {MeetingId}", meetingId);
                    return false;
                }

                _context.ZoomMeetings.Remove(meeting);
                // REMOVED: await _context.SaveChangesAsync(ct); - Let UnitOfWork handle the commit

                _logger.LogInformation("Zoom meeting marked for deletion: {MeetingId}", meetingId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Zoom meeting with ID: {MeetingId}", meetingId);
                throw;
            }
        }

        public async Task<List<ZoomRecording>> GetRecordingsAsync(long meetingId, CancellationToken ct = default)
        {
            try
            {
                _logger.LogDebug("Retrieving recordings for Zoom meeting ID: {MeetingId}", meetingId);

                var recordings = await _context.ZoomRecordings
                    .Where(r => r.ZoomMeetingId == meetingId)
                    .OrderByDescending(r => r.RecordingStart)
                    .ToListAsync(ct);

                _logger.LogDebug("Found {Count} recordings for meeting ID: {MeetingId}", recordings.Count, meetingId);
                return recordings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve recordings for Zoom meeting ID: {MeetingId}", meetingId);
                throw;
            }
        }

        public async Task<ZoomRecording> SaveRecordingAsync(ZoomRecording recording, CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Saving Zoom recording with ID: {RecordingId}", recording.RecordingId);

                var existingRecording = await _context.ZoomRecordings
                    .FirstOrDefaultAsync(r => r.RecordingId == recording.RecordingId, ct);

                if (existingRecording == null)
                {
                    // Add new recording
                    await _context.ZoomRecordings.AddAsync(recording, ct);
                    _logger.LogDebug("Adding new recording with ID: {RecordingId}", recording.RecordingId);
                }
                else
                {
                    // Update existing recording
                    _context.Entry(existingRecording).CurrentValues.SetValues(recording);
                    _logger.LogDebug("Updating existing recording with ID: {RecordingId}", recording.RecordingId);
                }

                // REMOVED: await _context.SaveChangesAsync(ct); - Let UnitOfWork handle the commit
                _logger.LogInformation("Zoom recording prepared for save: {RecordingId}", recording.RecordingId);

                return existingRecording ?? recording;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save Zoom recording with ID: {RecordingId}", recording.RecordingId);
                throw;
            }

        }
    }
}
