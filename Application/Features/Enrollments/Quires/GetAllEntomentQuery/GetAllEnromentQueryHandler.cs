using Application.Features.Enrollments.Dto;
using AutoMapper;
using Core.Common.Results;
using Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Quires.GetAllEntomentQuery
{
    public class GetAllEnromentQueryHandler : IRequestHandler<GetAllEnrollmentQuery,Result<List<EnrollmentDto>>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;

        public GetAllEnromentQueryHandler(IEnrollmentRepository enrollmentRepository, 
            IMapper mapper , HybridCache cache)
        {
            _enrollmentRepository = enrollmentRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<EnrollmentDto>>> Handle(GetAllEnrollmentQuery request, CancellationToken ct)
        {
            const string cacheKey = "Enrollments";

            var dtos = await _cache.GetOrCreateAsync(
                cacheKey,
                async ct =>
                {
                    var enrollments = await _enrollmentRepository.GetAllAsync();
                    if (enrollments == null || !enrollments.Any())
                        return null;

                    return _mapper.Map<List<EnrollmentDto>>(enrollments);
                },
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(60)
                },
                tags: new[] { "Enrollment" },
                cancellationToken: ct
            );

            if (dtos == null)
                return Result<List<EnrollmentDto>>.FromError(Error.NotFound("Enrollments not found"));

            return Result<List<EnrollmentDto>>.FromValue(dtos);
        }
    }
}
