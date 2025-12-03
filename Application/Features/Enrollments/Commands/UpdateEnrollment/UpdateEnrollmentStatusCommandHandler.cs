using Application.Features.Enrollments.Dto;
using AutoMapper;
using Core.Common.Results;
using Core.Interfaces;
using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Commands.UpdateEnrollment
{
    public class UpdateEnrollmentStatusCommandHandler : IRequestHandler<UpdateEnrollmentStatusCommand, Result<EnrollmentDto>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;

        public UpdateEnrollmentStatusCommandHandler(
            IEnrollmentRepository enrollmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            HybridCache cache)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<EnrollmentDto>> Handle(UpdateEnrollmentStatusCommand request, CancellationToken ct)
        {
            // Get enrollment
            var enrollment = await _enrollmentRepository.GetByIdAsync(request.EnrollmentId);
            if (enrollment == null)
                return Result<EnrollmentDto>.FromError(Error.NotFound("Enrollment.NotFound", "Enrollment not found"));

            // Update status
            var updateResult = enrollment.UpdateStatus(request.Status, request.Reason);
            if (!updateResult.IsSuccess)
                return Result<EnrollmentDto>.FromError(Error.Failure());

            // Save changes
             _enrollmentRepository.Update(enrollment);
            await _unitOfWork.CommitAsync(ct);

            // Update cache
            var dto = _mapper.Map<EnrollmentDto>(enrollment);
            await _cache.SetAsync(
                $"Enrollment_{enrollment.Id}",
                dto,
                new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(60) },
                tags: new[] { "Enrollments" },
                cancellationToken: ct
            );

            // Also invalidate student and course related caches
            await _cache.RemoveByTagAsync($"Student_{enrollment.StudentId}", ct);
            await _cache.RemoveByTagAsync($"Course_{enrollment.CourseId}", ct);

            return Result<EnrollmentDto>.FromValue(dto);
        }
    }
}
