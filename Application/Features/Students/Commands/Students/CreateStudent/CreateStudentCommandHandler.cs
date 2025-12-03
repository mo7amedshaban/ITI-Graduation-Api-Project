
using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Common.Results;
using Core.Entities.Students;
using Core.Interfaces;
using Core.Interfaces.Services;

using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;


namespace Application.Features.Students.Commands.Students.CreateStudent
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserContextService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _cache;
        private readonly ILogger<CreateStudentCommandHandler> _logger;
        private readonly IUserContextService _currentUserService;

        public CreateStudentCommandHandler(
            IUserContextService userService,
            IStudentRepository studentRepository,
            IMapper mapper,
            ILogger<CreateStudentCommandHandler> logger,
            IUnitOfWork unitOfWork,
            HybridCache cache,
            IUserContextService currentUserService)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _userService = userService;
           
            _logger = logger;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken ct)
        {
         
            var userId = _currentUserService.GetUserId();
            if (userId == Guid.Empty)
                return Result<StudentDto>.FromError(Error.Unauthorized("User not authenticated"));


            var userResult = await _userService.GetUserByIdAsync(userId);
            if (!userResult.IsSuccess)
                return Result<StudentDto>.FromError(Error.NotFound("User.NotFound", "User not found"));

            var user = userResult.Value;

            _logger.LogInformation("Creating student for user {UserId}", userId);

            // Check if student already exists
            var existingStudent = await _studentRepository.GetByIdWithUserAsync(userId, ct);
            if (existingStudent != null)
                return Result<StudentDto>.FromError(Error.Conflict("Student profile already exists"));

            var CreateTime = DateTime.UtcNow;
            // Create student
            var studentResult = Student.Create(
                userId
                 
            );

            if (!studentResult.IsSuccess)
                return Result<StudentDto>.FromError(Error.Failure("Error For Create Student "));


            var student = studentResult.Value;
            await _studentRepository.AddAsync(student);
            await _unitOfWork.CommitAsync(ct);

            // Cache
            var studentDto = _mapper.Map<StudentDto>(student);
            await _cache.SetAsync($"student_{student.Id}", studentDto);
            await _cache.RemoveByTagAsync("students", ct);
            await _cache.RemoveByTagAsync("All_Students_With_Course", ct);

            _logger.LogInformation("Student created with ID: {StudentId}", student.Id);
            //return Result<StudentDto>.FromValue(studentDto);

            return Result<StudentDto>.FromValue(new StudentDto
            {
                Id = student.Id,
                UserId = student.UserId,
               
               
                // User data from Auth Service
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            });
        }
    }
}
