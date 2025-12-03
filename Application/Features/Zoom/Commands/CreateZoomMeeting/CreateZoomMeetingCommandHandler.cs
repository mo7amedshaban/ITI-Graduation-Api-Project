using AutoMapper;
using Core.Common.Results;
using Core.DTOs;
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

namespace Application.Features.Zoom.Commands.CreateZoomMeeting
{
    public class CreateZoomMeetingCommandHandler : IRequestHandler<CreateZoomMeetingCommand,
        Result<ZoomMeetingResponse>>
    {
        private readonly IZoomService _zoomService;
        private readonly IZoomMeetingRepository _meetingRepository;
        private readonly HybridCache _cacheService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateZoomMeetingCommandHandler> _logger;

        public CreateZoomMeetingCommandHandler(
            IZoomService zoomService,
            IZoomMeetingRepository meetingRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            HybridCache cacheService,
            ILogger<CreateZoomMeetingCommandHandler> logger)
        {
            _zoomService = zoomService;
            _meetingRepository = meetingRepository;
            _cacheService = cacheService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ZoomMeetingResponse>> Handle(CreateZoomMeetingCommand request,
            CancellationToken ct)
        {
            try
            {
                var zoomRequest = _mapper.Map<ZoomMeetingRequest>(request);

                var response = await _zoomService.CreateMeetingAsync(zoomRequest, ct);

                //var meeting = _mapper.Map<ZoomMeeting>(response);
                var meeting = new ZoomMeeting
                {
                    ZoomMeetingId = response.Id,
                    Topic = response.Topic ?? string.Empty,
                    StartTime = DateTime.TryParse(response.StartTime, out var parsedStart)
                        ? parsedStart.ToLocalTime() 
                        : DateTime.UtcNow,
                    Duration = response.Duration,
                    JoinUrl = response.JoinUrl ?? string.Empty,
                    StartUrl = response.StartUrl ?? string.Empty,
                    Password = response.Password ?? string.Empty,
                    Status = response.Status ?? "waiting"
                    
                };




                await _meetingRepository.CreateMeetingAsync(meeting, ct);
                await _unitOfWork.CommitAsync(ct);

                // Invalidate cache for user's meetings
                await _cacheService.RemoveAsync($"user-meetings-{request.UserId}", ct);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create meeting for user {UserId}", request.UserId);
                throw;
            }
        }



     
    }
}
