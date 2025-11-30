using Application.Features.Lectures.Dtos;

using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Common.Results;

using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.Students.GetStudentCourseLecture
{
    public class GetStudentLecturesQueryHandler
     : IRequestHandler<GetStudentLecturesQuery, Result<List<StudentCourseLecturesDto>>>
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;
        private readonly ILogger<GetStudentLecturesQueryHandler> _logger;

        public GetStudentLecturesQueryHandler(
            IEnrollmentRepository enrollmentRepo,
            IMapper mapper,
            HybridCache cache,
            ILogger<GetStudentLecturesQueryHandler> logger)
        {
            _enrollmentRepo = enrollmentRepo;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }
        public async Task<Result<List<StudentCourseLecturesDto>>> Handle(GetStudentLecturesQuery request, CancellationToken ct)
        {
            try
            {
                var cacheKey = $"student_{request.StudentId}_lectures";
                var cacheTags = new[] { "lectures", $"student_{request.StudentId}" };

                var data = await _cache.GetOrCreateAsync(cacheKey, async cancellationToken =>
                {
                    _logger.LogInformation("Cache miss - loading lectures for student {StudentId}", request.StudentId);

                    var spec = new EnrollmentsWithLecturesByStudentIdSpecification(request.StudentId);
                    var enrollments = await _enrollmentRepo.GetAllWithSpecAsync(spec, ct);

                    if (enrollments == null || enrollments.Count() == 0)
                    {
                        _logger.LogInformation("No enrollments found for student {StudentId}", request.StudentId);
                        return new List<StudentCourseLecturesDto>();
                    }

                    var result = enrollments
                        .Select(e => new StudentCourseLecturesDto
                        {
                            CourseId = e.Course.Id,
                            CourseTitle = e.Course.Title,
                            Modules = e.Course.Modules
                                .Select(m => new ModuleLecturesDto
                                {
                                    ModuleId = m.Id,
                                    ModuleTitle = m.Title,
                                    Lectures = _mapper.Map<List<LectureDto>>(m.Lectures)
                                })
                                .ToList()
                        })
                        .ToList();

                    return result;
                }, new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(30) }, cacheTags, ct);

                return Result<List<StudentCourseLecturesDto>>.FromValue(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lectures for student {StudentId}", request.StudentId);
                return Result<List<StudentCourseLecturesDto>>.FromError(Error.Failure("Student.Lectures.RetrievalFailed", ex.Message));
            }
        }
       

    }

}
