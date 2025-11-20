using Application.Common.Exceptions;
using Application.Features.Instructors.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Instructors.Queries.GetInstructorById;

public class GetInstructorByIdQueryHandler : IRequestHandler<GetInstructorByIdQuery, InstructorDto>
{
    private readonly IGenericRepository<Instructor> _instructorRepository;
    private readonly IMapper _mapper;

    public GetInstructorByIdQueryHandler(IGenericRepository<Instructor> instructorRepository, IMapper mapper)
    {
        _instructorRepository = instructorRepository;
        _mapper = mapper;
    }

    public async Task<InstructorDto> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorRepository.GetByIdAsync(request.Id);
        if (instructor == null)
            throw new BusinessException($"Instructor with Id {request.Id} not found.");

        return _mapper.Map<InstructorDto>(instructor);
    }
}