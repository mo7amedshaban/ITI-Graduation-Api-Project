using Application.Features.Students.DTOs;
using AutoMapper;
using Core.Common.Results;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;


namespace Application.Features.Students.Queries.Students.GetAllStudentsQuery
{
    public class GetAllStudentsQueryHandler
     : IRequestHandler<GetAllStudentsQuery, Result<List<StudentDto>>>
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;

        public GetAllStudentsQueryHandler(
            IStudentRepository repository,
            IMapper mapper,
           
            HybridCache cache)
        {
            _repository = repository;
            _mapper = mapper;
           
            _cache = cache;
        }

        public async Task<Result<List<StudentDto>>> Handle(GetAllStudentsQuery request, CancellationToken ct)
        {
            var cacheKey = "All_Students";

            var dtos = await _cache.GetOrCreateAsync(
                cacheKey,
                async cancellationToken =>
                {
                    
                    var studentsQuery = _repository.GetAll().AsQueryable()
                        .Include(s => s.User)
                        .AsNoTracking();

                    var studentsResult = await studentsQuery.ToListAsync(ct);

                    if (studentsResult == null)
                        return new List<StudentDto>();

                    return studentsResult.Select(s => new StudentDto
                    {
                        Id = s.Id,
                        FirstName = s.User.FirstName!,
                        LastName = s.User.LastName!,
                        Gender = s.User.Gender!,
                        PhoneNumber = s.User.PhoneNumber,
                        Email = s.User.Email!,
                        UserName = s.User.UserName!,
                        UserId = s.UserId
                    }).ToList();

                },
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(30)
                },
                tags: new[] { "Students" },
                cancellationToken: ct
            );

            return Result<List<StudentDto>>.FromValue(dtos);
           
        }
    }
}
