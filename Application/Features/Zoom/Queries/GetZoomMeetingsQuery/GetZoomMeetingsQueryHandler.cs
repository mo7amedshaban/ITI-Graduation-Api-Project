using AutoMapper;
using Core.Common.Results;
using Core.DTOs;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Zoom.Queries.GetZoomMeetingsQuery
{
    public class GetZoomMeetingsQueryHandler
      : IRequestHandler<GetZoomMeetingsQuery, Result<List<ZoomMeetingResponse>>>
    {
        private readonly IZoomService _zoomService;
        private readonly IMapper _mapper;
        private readonly HybridCache _cacheService;
        private readonly ILogger<GetZoomMeetingsQueryHandler> _logger;

        public GetZoomMeetingsQueryHandler(
            IZoomService zoomService,
            IMapper mapper,
            HybridCache cacheService,
            ILogger<GetZoomMeetingsQueryHandler> logger)
        {
            _zoomService = zoomService;
            _cacheService = cacheService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<List<ZoomMeetingResponse>>> Handle(
        GetZoomMeetingsQuery request,
        CancellationToken ct)
        {
            var cacheKey = $"user-meetings-{request.meetingId}";

            try
            {

                var meetings = await _cacheService.GetOrCreateAsync<List<ZoomMeetingResponse>>(
                    cacheKey,
                    async cancellationToken =>
                    {

                        var response = await _zoomService.GetMeetingAsync(request.meetingId, cancellationToken);

                         var MeetingResponse= _mapper.Map<ZoomMeetingResponse>(response);
                        //if (ZoomMeetingDto.)
                        //{
                        //    _logger.LogWarning("Error retrieving meetings for {UserId}: {Errors}",
                        //        request.meetingId);
                        //    return null;
                        //}
                       return new List<ZoomMeetingResponse> { MeetingResponse };
                         
                      
                    },
                    options: new HybridCacheEntryOptions
                    {
                        Expiration = TimeSpan.FromMinutes(30)
                    },
                    tags: null,
                    cancellationToken: ct);

                if (meetings == null)
                {
                    return Result<List<ZoomMeetingResponse>>.FromError(
                        ZoomErrors.ApiError("Could not retrieve meetings from Zoom."));
                }

                _logger.LogInformation("Returning meetings (possibly from cache) for {UserId}", request.meetingId);

                return Result<List<ZoomMeetingResponse>>.FromValue(meetings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve meetings for user {UserId}", request.meetingId);
                return Result<List<ZoomMeetingResponse>>.FromError(ZoomErrors.ApiError(ex.Message));
            }
        }

    }

}
