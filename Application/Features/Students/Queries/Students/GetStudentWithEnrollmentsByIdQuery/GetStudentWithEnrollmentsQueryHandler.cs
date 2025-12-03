using Application.Features.Courses.DTOs;

using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Common.Results;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetStudentWithEnrollmentsQuery
{
    public class GetStudentWithEnrollmentsQueryHandler
      : IRequestHandler<GetStudentWithEnrollmentsQuery, Result<StudentWithEnrollmentsDto>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserContextService _userService; // Add this!
        private readonly IMapper _mapper;
        private readonly ILogger<GetStudentWithEnrollmentsQueryHandler> _logger;

        public GetStudentWithEnrollmentsQueryHandler(
            IStudentRepository studentRepository,
            IUserContextService userService, // Add this dependency
            IMapper mapper,
            ILogger<GetStudentWithEnrollmentsQueryHandler> logger)
        {
            _studentRepository = studentRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StudentWithEnrollmentsDto>> Handle(
            GetStudentWithEnrollmentsQuery request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Getting student {StudentId} with enrollments", request.StudentId);

                // 1. Get student with enrollments
                var student = await _studentRepository.GetWithEnrollmentsAsync(request.StudentId, ct);
                if (student == null)
                {
                    _logger.LogWarning("Student {StudentId} not found", request.StudentId);
                    return Result<StudentWithEnrollmentsDto>.FromError(Error.NotFound("Student not found"));
                }

                // 2. Get user data
                var userResult = await _userService.GetUserByIdAsync(student.UserId);
                if (!userResult.IsSuccess)
                {
                    _logger.LogWarning("User not found for student {StudentId}", request.StudentId);
                    return Result<StudentWithEnrollmentsDto>.FromError(Error.Failure("User not found for studentId"));
                }

                var user = userResult.Value;

                // 3. Map to DTO and combine with user data
                var studentDto = _mapper.Map<StudentWithEnrollmentsDto>(student);

                // 4. Add user data to the DTO
                studentDto.Email = user.Email;
                studentDto.FirstName = user.FirstName;
                studentDto.LastName = user.LastName;
                studentDto.PhoneNumber = user.PhoneNumber;
                studentDto.UserName = user.UserName;

                _logger.LogInformation("Successfully retrieved student {StudentId} with {EnrollmentCount} enrollments",
                    request.StudentId, (studentDto.Enrollments != null ? studentDto.Enrollments.Count : 0));

                return Result<StudentWithEnrollmentsDto>.FromValue(studentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student with enrollments: {StudentId}", request.StudentId);
                return Result<StudentWithEnrollmentsDto>.FromError(Error.Failure("Student.RetrievalFailed", ex.Message));
            }
        }
    }
}

public class StudentWithEnrollmentsDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string UserName { get; set; }
    public List<CourseDto> Enrollments { get; set; }
}
