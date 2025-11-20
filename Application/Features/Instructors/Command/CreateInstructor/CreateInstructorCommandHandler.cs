using Application.Features.Instructors.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Instructors.Command.CreateInstructor;

public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand, CreateInstructorDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInstructorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateInstructorDto> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        if (request.Dto == null)
            throw new ArgumentNullException(nameof(request.Dto), "Instructor data must be provided.");
        var instructor = _mapper.Map<Instructor>(request.Dto);
        await _unitOfWork.Instructors.AddAsync(instructor);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CreateInstructorDto>(instructor);
    }
}