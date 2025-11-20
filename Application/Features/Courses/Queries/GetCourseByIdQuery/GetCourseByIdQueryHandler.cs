using Application.Common.Exceptions;
using Application.Features.Courses.DTOs;
using AutoMapper;
using Core.Entities.Courses;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Courses.Queries.GetCourseByIdQuery;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    private readonly IGenericRepository<Course> _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseByIdQueryHandler(IGenericRepository<Course> courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.id);
        if (course == null)
            throw new BusinessException("Course is Null");
        return _mapper.Map<CourseDto>(course);
    }
}