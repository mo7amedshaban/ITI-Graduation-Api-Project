using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Courses.Commands.RemoveCourse;

public class RemoveCourseCommandHandler : IRequestHandler<RemoveCourseCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoveCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(request.Id);
        if (course == null)
            throw new KeyNotFoundException($"Course with Id {request.Id} not found.");
        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}