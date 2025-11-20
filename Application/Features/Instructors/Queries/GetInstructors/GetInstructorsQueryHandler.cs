using Application.Features.Instructors.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Instructors.Queries.GetInstructors;

public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, List<InstructorDto>>
{
    private readonly IGenericRepository<Instructor> _instructorRepository;
    private readonly IMapper _mapper;

    public GetInstructorsQueryHandler(IGenericRepository<Instructor> instructorRepository, IMapper mapper)
    {
        _instructorRepository = instructorRepository;
        _mapper = mapper;
    }

    public async Task<List<InstructorDto>> Handle(GetInstructorsQuery request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var instructors = await _instructorRepository.GetAllAsync();
        return _mapper.Map<List<InstructorDto>>(instructors);
    }
}