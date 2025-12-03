using Application.Features.Enrollments.Dto;
using AutoMapper;
using Core.Common.Results;
using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Quires.GetEnrollmentByIdQuery
{
    public class GetEnrollmentByIdQueryHandler : IRequestHandler<GetEnrollmentByIdQuery, Result<EnrollmentDetailsDto>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;

        public GetEnrollmentByIdQueryHandler(
            IEnrollmentRepository enrollmentRepository,
            IMapper mapper,
            HybridCache cache)
        {
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<Result<EnrollmentDetailsDto>> Handle(GetEnrollmentByIdQuery request, CancellationToken ct)
        {
            var cacheKey = $"enrollment_{request.EnrollmentId}";

            var dto = await _cache.GetOrCreateAsync(
                cacheKey,
                async (ct) => {
                    var enrollmentResult = await _enrollmentRepository
                        .GetByIdWithDetailsAsync(request.EnrollmentId, ct);

                    if (!enrollmentResult.IsSuccess)
                        return null;

                  
                    return _mapper.Map<EnrollmentDetailsDto>(enrollmentResult.Value);
                },
                new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(30) },
                tags: new[] { "enrollments" },
                ct
            );

            return dto != null
                ? Result<EnrollmentDetailsDto>.FromValue(dto)
                : Result<EnrollmentDetailsDto>.FromError(Error.NotFound("Enrollment.NotFound"));
        }
       
    }
}
