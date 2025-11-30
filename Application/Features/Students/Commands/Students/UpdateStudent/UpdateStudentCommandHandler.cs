
using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Common.Results;
using Core.Entities.Students;
using Core.Interfaces;
using Core.Interfaces.Services;

using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.Students.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Result<StudentDto>>
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IUserContextService _userService; // Add this!
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateStudentCommandHandler> _logger;
        private readonly HybridCache _cache;

        public UpdateStudentCommandHandler(
            IGenericRepository<Student> studentRepository,
            IUserContextService userService, 
            IMapper mapper,
            ILogger<UpdateStudentCommandHandler> logger,
            IUnitOfWork unitOfWork,
            HybridCache cache)
        {
            _studentRepository = studentRepository;
            _userService = userService; // Initialize it
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result<StudentDto>> Handle(UpdateStudentCommand request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Updating student {StudentId}", request.StudentId);

                // 1. Get the student
                var student = await _studentRepository.GetByIdAsync(request.StudentId);
                if (student == null)
                {
                    _logger.LogWarning("Student {StudentId} not found", request.StudentId);
                    return Result<StudentDto>.FromError(Error.NotFound("Student not found"));
                }

                // 2. Get user data for response
                var userResult = await _userService.GetUserByIdAsync(student.UserId);
                if (!userResult.IsSuccess)
                {
                    _logger.LogWarning("User not found for student {StudentId}", request.StudentId);
                    return Result<StudentDto>.FromError(Error.Failure("User not found for studentId"));
                }

                var user = userResult.Value;

              
                var updateResult = student.Update(
                    request.Gender
                                   
                );

                if (!updateResult.IsSuccess)
                {
                  
                    return Result<StudentDto>.FromError(Error.Failure("User not found for studentId"));
                }

                // 4. Save changes
                 _studentRepository.Update(student);
                await _unitOfWork.CommitAsync(ct);

                // 5. Invalidate cache
                await _cache.RemoveAsync($"student_{request.StudentId}", ct);
                await _cache.RemoveByTagAsync("students", ct);

                _logger.LogInformation("Student {StudentId} updated successfully", request.StudentId);

                // 6. Return combined response
                return Result<StudentDto>.FromValue(new StudentDto
                {
                    Id = student.Id,
                    UserId = student.UserId,
                    // Student-specific data
                    //Gender = student.Gender,
                    // Add other student-specific fields

                    // User data from Auth Service
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student {StudentId}", request.StudentId);
                return Result<StudentDto>.FromError(Error.Failure("Student.UpdateFailed", ex.Message));
            }
        }
    }
}
