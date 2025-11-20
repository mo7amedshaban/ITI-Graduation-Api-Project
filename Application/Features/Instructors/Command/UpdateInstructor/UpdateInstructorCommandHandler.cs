using Application.Common.Exceptions;
using Application.Features.Instructors.DTOs;
using AutoMapper;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Instructors.Command.UpdateInstructor;

public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand, InstructorDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInstructorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<InstructorDto> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Instructors.GetByIdAsync(request.InstructorDto.Id);
        if (instructor == null)
            throw new BusinessException($"Instructor with Id {request.InstructorDto.Id} not found.");

        _mapper.Map(request.InstructorDto, instructor);
        _unitOfWork.Instructors.Update(instructor);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return _mapper.Map<InstructorDto>(instructor);
    }
}