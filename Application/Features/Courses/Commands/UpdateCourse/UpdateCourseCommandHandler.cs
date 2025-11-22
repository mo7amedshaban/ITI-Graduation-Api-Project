using Application.Features.Courses.DTOs;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseDto.UpdateCourseDto>
{
    public readonly IMapper _mapper;
    public readonly IUnitOfWork _unitOfWork;


    public UpdateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CourseDto.UpdateCourseDto> Handle(UpdateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var Course = await _unitOfWork.Courses.GetByIdAsync(request.Dto.Id);
        if (Course == null) throw new KeyNotFoundException($"Course with Id {request.Dto.Id} not found.");
        _mapper.Map(request.Dto, Course);

        var trackedCourse = _unitOfWork.Courses.Update(Course);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<CourseDto.UpdateCourseDto>(trackedCourse);
    }
}