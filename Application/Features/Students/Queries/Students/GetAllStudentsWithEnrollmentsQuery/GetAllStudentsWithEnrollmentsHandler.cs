
using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Interfaces.Services;
using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;


namespace Application.Features.Students.Queries.Students.GetAllStudentsWithEnrollmentsQuery
{
    public class GetAllStudentsWithEnrollmentsHandler
       : IRequestHandler<GetAllStudentsWithEnrollmentsQuery, List<StudentEnrollmentTableDto>>
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllStudentsWithEnrollmentsHandler> _logger;
        private readonly HybridCache _cache;

        public GetAllStudentsWithEnrollmentsHandler(
            IStudentRepository repository,
            IMapper mapper,
            ILogger<GetAllStudentsWithEnrollmentsHandler> logger,
            HybridCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<StudentEnrollmentTableDto>> Handle(
            GetAllStudentsWithEnrollmentsQuery request,
            CancellationToken ct)
        {
            const string cacheKey = "All_Students_With_Course";

          
            var dtos = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    var students = await _repository.GetAllWithEnrollmentsAsync(ct);
                    if (students == null || students.Count == 0)
                        return new List<StudentEnrollmentTableDto>();

                    return students
                        .SelectMany(student => student.Enrollments.Select(enrollment => new StudentEnrollmentTableDto
                        {
                            StudentId = student.Id,
                            FullName = $"{student.User.FirstName} {student.User.LastName}",
                            Gender = student.User.Gender,
                            PhoneNumber = student.User.PhoneNumber,
                            CourseTitle = enrollment.Course.Title,
                            JoinDate = enrollment.EnrollmentDate.DateTime
                        }))
                        .ToList();
                },
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(30)
                },
                tags: new[] { "Students" },
                cancellationToken: ct
            );

            return dtos;
        }
    }


}
