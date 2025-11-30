using Core.Common.Results;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.Students.RemoveStudent
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Result<bool>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteStudentCommandHandler> _logger;
        private readonly HybridCache _cache;

        public DeleteStudentCommandHandler(
            IStudentRepository studentRepository,
            ILogger<DeleteStudentCommandHandler> logger,
            IUnitOfWork unitOfWork,
            HybridCache cache)
        {
            _studentRepository = studentRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(DeleteStudentCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Deleting student {StudentId}", request.StudentId);

            var student = await _studentRepository.GetByIdWithUserAsync(request.StudentId, ct);
            if (student == null)
            {
                _logger.LogWarning("Student {StudentId} not found", request.StudentId);
                return Result<bool>.FromError(Error.NotFound("Student not found"));
            }

            // Check if student has enrollments or exam results
            if (student.Enrollments.Any() || student.ExamResults.Any())
            {
                _logger.LogWarning("Cannot delete student {StudentId} - has associated enrollments or exam results",
                    request.StudentId);
                return Result<bool>.FromError(
                    Error.Conflict("Cannot delete student with existing enrollments or exam results"));
            }

             _studentRepository.Delete(student);
            await _unitOfWork.CommitAsync(ct);

            // Invalidate cache
            await _cache.RemoveByTagAsync($"student:{request.StudentId}", ct);
            await _cache.RemoveByTagAsync("students", ct);

            _logger.LogInformation("Student {StudentId} deleted successfully", request.StudentId);

            return Result<bool>.FromValue(true);
        }
    }
}
