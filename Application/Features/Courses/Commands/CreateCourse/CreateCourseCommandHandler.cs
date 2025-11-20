using Application.Features.Courses.DTOs;
using AutoMapper;
using Core.Entities.Courses;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = _mapper.Map<Course>(request.Dto);
        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return _mapper.Map<CourseDto>(course); //---------
    }
}