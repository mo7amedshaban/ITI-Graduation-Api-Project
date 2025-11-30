using Application.Common.Behaviours.Interfaces;
using Application.Features.Students.Dtos;
using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Common.Results;
using Core.Entities.Students;
using Core.Interfaces;
using Core.Interfaces.Services;
using Domain.Common.Interface;
using Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetStudentById
{
    public class GetStudentByIdQueryHandler
       : IRequestHandler<GetStudentByIdQuery, Result<StudentDto>>
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IUserContextService _userService; 
        private readonly IMapper _mapper;
        private readonly ILogger<GetStudentByIdQueryHandler> _logger; 

        public GetStudentByIdQueryHandler(
            IGenericRepository<Student> studentRepository,
            IUserContextService userService, 
            IMapper mapper,
            ILogger<GetStudentByIdQueryHandler> logger)
        {
            _studentRepository = studentRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<StudentDto>> Handle(GetStudentByIdQuery request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Getting student by ID: {StudentId}", request.StudentId);

                // 1. Get student from repository
                var student = await _studentRepository.GetByUserIdAsnc(request.StudentId, ct);
                if (student == null)
                {
                    _logger.LogWarning("Student {StudentId} not found", request.StudentId);
                    return Result<StudentDto>.FromError(Error.NotFound("Student not found"));
                }

                // 2. Get user data from Auth service
                var userResult = await _userService.GetUserByIdAsync(student.UserId);
                if (!userResult.IsSuccess)
                {
                    
                    return Result<StudentDto>.FromError(Error.Failure("User not found for studentId"));
                }

                var user = userResult.Value;

              
                var studentDto = new StudentDto
                {
                   
                    Id = student.Id,
                    UserId = student.UserId,
              
                    // User
                    Email = user.Email,
                    Gender = user.Gender,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName
                };

                _logger.LogInformation("Successfully retrieved student {StudentId}", request.StudentId);
                return Result<StudentDto>.FromValue(studentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by ID: {StudentId}", request.StudentId);
                return Result<StudentDto>.FromError(Error.Failure("Student.RetrievalFailed", ex.Message));
            }
        }
    }
}
