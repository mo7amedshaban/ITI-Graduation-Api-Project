using Core.Common.Results;
using Core.Interfaces;
using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Lectures.Commands.RemoveLectures
{
    public class DeleteLectureHandler : IRequestHandler<DeleteLectureCommand, Result<bool>>
    {
        private readonly ILectureRespository _lectureRepository;
        private readonly IZoomMeetingRepository _zoomRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _cache;
        private readonly ILogger<DeleteLectureHandler> _logger;

        public DeleteLectureHandler(
            ILectureRespository lectureRepository,
            IZoomMeetingRepository zoomRepo,
            IUnitOfWork unitOfWork,
            HybridCache cache,
            ILogger<DeleteLectureHandler> logger)
        {
            _lectureRepository = lectureRepository;
            _zoomRepo = zoomRepo;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteLectureCommand request, CancellationToken ct)
        {
            var lecture = await _lectureRepository.GetByIdAsync(request.LectureId);
            if (lecture is null)
                return Result<bool>.FromError(
                    Error.NotFound("Lecture.NotFound", "Lecture not found."));

            // Delete Zoom meeting
            var zoomMeeting = await _zoomRepo.GetMeetingAsync(Convert.ToInt64(lecture.ZoomMeetingId), ct);
            if (zoomMeeting != null)
                await _zoomRepo.DeleteMeetingAsync(Convert.ToInt64(lecture.ZoomMeetingId), ct);

            // Delete Lecture
             _lectureRepository.Delete(lecture);

            await _unitOfWork.CommitAsync(ct);

            // Clear cache
            await _cache.RemoveAsync($"lecture_{request.LectureId}");
            await _cache.RemoveByTagAsync("lectures", ct);

            _logger.LogInformation("Lecture {LectureId} deleted successfully.", request.LectureId);

            return Result<bool>.FromValue(true);
        }
    }
}
