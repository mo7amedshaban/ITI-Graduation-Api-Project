using Application.Common.Exceptions;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Instructors.Command.RemoveInstructor;

public class RemoveInstructorCommandHandler : IRequestHandler<RemoveInstructorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveInstructorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoveInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdAsync(request.InstructorId);
        if (instructor == null)
            throw new BusinessException($"Instructor with Id {request.InstructorId} not found.");
        _unitOfWork.Instructors.Delete(instructor);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}