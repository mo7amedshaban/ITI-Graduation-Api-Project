using Application.Features.Lectures.Dtos;
using AutoMapper;
using Core.Common.Results;
using Infrastructure.Interface;

using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Application.Features.Lectures.Queries.GetWithModules
{
    public class GetLecturesByModuleIdQueryHandler
                  : IRequestHandler<GetLecturesByModuleIdQuery, Result<List<LectureDto>>>
    {
        private readonly ILectureRespository _lectureRepository;
        private readonly HybridCache _cache;
        private readonly ILogger<GetLecturesByModuleIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetLecturesByModuleIdQueryHandler(
            ILectureRespository lectureRepository,
            HybridCache cache,
            ILogger<GetLecturesByModuleIdQueryHandler> logger,
            IMapper mapper)
        {
            _lectureRepository = lectureRepository;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<List<LectureDto>>> Handle(GetLecturesByModuleIdQuery request, CancellationToken ct)
        {
            try
            {
                var cacheKey = $"lectures_module_{request.ModuleId}";
                var cacheTags = new[] { "lectures", $"module_{request.ModuleId}" };

                var lectures = await _cache.GetOrCreateAsync(
                    cacheKey,
                    async cancellationToken =>
                    {
                        _logger.LogInformation("Cache miss - loading lectures for module {ModuleId}", request.ModuleId);

                        var spec = new LecturesByModuleIdSpecification(request.ModuleId);
                        var entities = await _lectureRepository.GetAllWithSpecAsync(spec, ct);

                        return _mapper.Map<List<LectureDto>>(entities);

                        //return entities.Select(l => new LectureDto
                        //{
                        //    Id = l.Id,
                        //    Title = l.Title,
                        //    IsRecordedAvailable = l.IsRecordedAvailable,
                        //    ScheduledAt = l.ScheduledAt,
                        //    IsCompleted = l.IsCompleted,
                        //    ModuleId = l.ModuleId,
                        //    CourseId = l.CourseId,
                        //    ZoomMeeting = l.ZoomMeeting is null ? null : new ZoomMeetingDto
                        //    {
                        //        Id = l.ZoomMeeting.Id,
                        //        Topic = l.ZoomMeeting.Topic,
                        //        JoinUrl = l.ZoomMeeting.JoinUrl,
                        //        StartTime = l.ZoomMeeting.StartTime
                        //    },
                        //    ZoomRecording = l.ZoomRecording is null ? null : new ZoomRecordingDto
                        //    {
                        //        Id = l.ZoomRecording.Id,
                        //        RecordingUrl = l.ZoomRecording.FileUrl,
                        //        //Duration = l.ZoomRecording.Duration
                        //    }
                        //}).ToList();
                    },
                    new HybridCacheEntryOptions
                    {
                        Expiration = TimeSpan.FromMinutes(30)
                    },
                    cacheTags,
                    ct);

                return Result<List<LectureDto>>.FromValue(lectures);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lectures for module {ModuleId}", request.ModuleId);
                return Result<List<LectureDto>>.FromError(Error.Failure("Lecture.RetrievalFailed", ex.Message));
            }
        }
    }

}
