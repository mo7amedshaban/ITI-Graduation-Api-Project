using Application.Features.Courses.DTOs;
using AutoMapper;
using Core.Entities.Courses;

namespace Application.Features.Courses.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<CourseDto.CreateCourseDto, Course>().ReverseMap();
        CreateMap<CourseDto.UpdateCourseDto, Course>().ReverseMap();
    }
}