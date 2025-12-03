
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

namespace Application.Features.Enrollments.Commands.RemoveEnrollment
{
    public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand, Result<bool>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _cache;

        public CancelEnrollmentCommandHandler(
            IEnrollmentRepository enrollmentRepository,
            IUnitOfWork unitOfWork,
            HybridCache cache)
        {
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(CancelEnrollmentCommand request, CancellationToken ct)
        {
            var enrollmentId = request.Id;
            var enrollment = await _enrollmentRepository.GetByIdAsync(request.Id);
            if (enrollment == null)
                return Result<bool>.FromError(Error.NotFound("Enrollment.NotFound", "Enrollment not found"));

            var cancelResult = enrollment.Cancel(request.CancellationReason);
            if (!cancelResult.IsSuccess)
                return Result<bool>.FromError(Error.Failure("Cancleling Enrollment Failure"));

             _enrollmentRepository.Update(enrollment);
            await _unitOfWork.CommitAsync(ct);

            // Clear caches
            await _cache.RemoveAsync($"Enrollment_{enrollment.Id}", ct);
            await _cache.RemoveByTagAsync($"Student_{enrollment.StudentId}", ct);
            await _cache.RemoveByTagAsync($"Course_{enrollment.CourseId}", ct);
            await _cache.RemoveByTagAsync("Enrollments", ct);

            return Result<bool>.FromValue(true);
        }
    }

}
