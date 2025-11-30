using Application.Features.Lectures.Dtos;
using AutoMapper;
using Core.Common.Results;
using Core.DTOs;
using Core.Entities.Courses;
using Core.Entities.Zoom;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Interface;

using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Commands.createLectures
{
    public class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand, Result<LectureDto>>
    {
        #region DI

        private readonly ILectureRespository _lectureRepository;
        private readonly IZoomMeetingRepository _zoomMeetingRepository;
        private readonly IMeetingRecordingStateService _zoomRecordingRepository;
        private readonly IZoomService _zoomService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _cache;
        private readonly ILogger<CreateLectureCommandHandler> _logger;

        public CreateLectureCommandHandler(
            ILectureRespository lectureRepository,
            IZoomMeetingRepository zoomMeetingRepository,
            IMeetingRecordingStateService zoomRecordingRepository,
            IZoomService zoomService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            HybridCache cache,
            ILogger<CreateLectureCommandHandler> logger)
        {
            _lectureRepository = lectureRepository;
            _zoomMeetingRepository = zoomMeetingRepository;
            _zoomRecordingRepository = zoomRecordingRepository;
            _zoomService = zoomService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }
        #endregion

        public async Task<Result<LectureDto>> Handle(CreateLectureCommand request, CancellationToken ct)
        {

            if (ct.IsCancellationRequested)
                return Result<LectureDto>.FromError(Error.Failure("Lecture.CreationCancelled", "Operation was cancelled."));

            // 1️⃣ Validation
            if (string.IsNullOrWhiteSpace(request.Title))
                return Result<LectureDto>.FromError(Error.Validation("Lecture.TitleRequired", "Title is required."));

            if (request.ScheduledAt == default)
                return Result<LectureDto>.FromError(Error.Validation("Lecture.ScheduledAtRequired", "ScheduledAt is required."));

            if (request.Duration == TimeSpan.Zero)
                return Result<LectureDto>.FromError(Error.Validation("Lecture.DurationInvalid", "Duration must be greater than zero."));

            try
            {
                // 2️⃣ Prepare ZoomMeetingRequest
                var zoomRequest = new ZoomMeetingRequest
                {
                    userId = string.Empty,
                    Topic = request.Title,
                    StartTime = request.ScheduledAt.UtcDateTime,
                    Duration = (int)request.Duration.TotalMinutes,
                    Timezone = "UTC",
                    Agenda = $"Lecture for Course {request.ModuleId}",
                    AutoRecording = "cloud",
                    PreSchedule = true,
                    HostId = Guid.NewGuid()
                };

                // 3️⃣ Call Zoom API to create meeting
                var zoomResponse = await _zoomService.CreateMeetingAsync(zoomRequest, ct);
                if (zoomResponse == null)
                {
                    _logger.LogError("Zoom service returned null meeting for Title={Title}, ScheduledAt={ScheduledAt}", request.Title, request.ScheduledAt);
                    return Result<LectureDto>.FromError(Error.Failure("Lecture.ZoomCreationFailed", "Failed to create Zoom meeting."));
                }

                if (request.ScheduledAt == default || request.ScheduledAt < DateTimeOffset.UtcNow)
                {
                    return Result<LectureDto>.FromError(
                        Error.Validation("Lecture.ScheduledAtInvalid", "ScheduledAt must be a valid future date.")
                    );
                }

                // 4️⃣ Build ZoomMeeting entity from response

                var zoomMeeting = new ZoomMeeting(); // Id is automatically set
                zoomMeeting.ZoomMeetingId = zoomResponse.Id;
                zoomMeeting.Topic = zoomResponse.Topic;
                zoomMeeting.JoinUrl = zoomResponse.JoinUrl;
                zoomMeeting.StartUrl = zoomResponse.StartUrl;
                zoomMeeting.Password = zoomResponse.Password;
                zoomMeeting.Duration = zoomResponse.Duration;
                zoomMeeting.CreatedAtUtc = DateTimeOffset.UtcNow;

                await _zoomMeetingRepository.CreateMeetingAsync(zoomMeeting, ct);

                // 5️⃣ Create Lecture entity linked to ZoomMeeting
                var lectureResult = Lecture.Create(

                         request.Title,
                         //request.CourseId,
                         request.ScheduledAt,
                         request.Duration,
                         request.ModuleId,
                         zoomMeeting.Id,
                         request.IsFree,
                         request.IsPublished

                                    );


                if (!lectureResult.IsSuccess)
                {
                    _logger.LogError("Lecture domain validation failed for Title={Title}", request.Title);
                    return Result<LectureDto>.FromError(Error.Validation("Lecture.ValidationFailed", "Lecture domain validation failed."));
                }

                var lecture = lectureResult.Value;

                await _lectureRepository.AddAsync(lecture, ct);

                // 6️⃣ Commit both entities as single transaction
                await _unitOfWork.CommitAsync(ct);

                // 7️⃣ Map to DTO
                var dto = _mapper.Map<LectureDto>(lecture);
                dto.ZoomMeeting = _mapper.Map<ZoomMeetingDto>(zoomMeeting);

                // 8️⃣ Cache the result
                try
                {
                    await _cache.SetAsync($"lecture_{lecture.Id}", dto);
                    await _cache.RemoveByTagAsync("lectures", ct);
                    await _cache.RemoveByTagAsync("courses");
                    await _cache.RemoveByTagAsync($"module_{request.ModuleId}_lectures", ct);

                }
                catch (Exception cacheEx)
                {
                    _logger.LogWarning(cacheEx, "Cache update failed for lecture {LectureId}", lecture.Id);
                }

                _logger.LogInformation("Lecture '{Title}' created successfully with Zoom meeting {ZoomMeetingId}.", request.Title, zoomMeeting.ZoomMeetingId);

                return Result<LectureDto>.FromValue(dto);
            }
            catch (OperationCanceledException oce) when (ct.IsCancellationRequested)
            {
                _logger.LogInformation(oce, "CreateLectureCommand was cancelled");
                return Result<LectureDto>.FromError(Error.Failure("Lecture.CreationCancelled", "Operation was cancelled."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lecture for ModuleId={ModuleId}, Title={Title}",
                  request.ModuleId, request.Title);

                return Result<LectureDto>.FromError(Error.Failure("Lecture.CreationFailed", ex.Message));
            }
        }

       
    }
}
