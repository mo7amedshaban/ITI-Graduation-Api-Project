using Application.Features.Instructors.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Features.Instructors.Mappers;

public class InstructorProfile : Profile
{
    public InstructorProfile()
    {
        CreateMap<Instructor, InstructorDto>().ReverseMap();
        CreateMap<Instructor, CreateInstructorDto>().ReverseMap();
    }
}