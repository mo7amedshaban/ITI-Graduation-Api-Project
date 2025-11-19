using Application.Features.Courses.DTOs;
using AutoMapper;
using Core.Entities.Courses;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Courses.Queries.GetAllCourses;

public class GetAllCoursesCommand : IRequestHandler<GetAllCoursesQuery, List<CourseDto>>
{
    private readonly IGenericRepository<Course> _courseRepository;
    private readonly IMapper _mapper;

    public GetAllCoursesCommand(IGenericRepository<Course> courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<List<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllAsync();
        return _mapper.Map<List<CourseDto>>(courses);
    }
}