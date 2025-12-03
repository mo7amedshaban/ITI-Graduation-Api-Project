
using Application.Features.Courses.DTOs;
using Application.Features.Enrollments.Dto;
using Application.Features.Students.DTOs;
using AutoMapper;



namespace Application.Features.Students.Mappers
{
    public class StudentProfile : Profile
    {

        public StudentProfile()
        { 

            CreateMap<Core.Entities.Students.Student, CreateStudentDto>().ReverseMap();

            CreateMap<Core.Entities.Students.Student, StudentDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Core.Entities.Courses.Enrollment, EnrollmentDto>();
            CreateMap<Core.Entities.Courses.Course, CourseDto>();

            CreateMap<Core.Entities.Students.Student, StudentWithEnrollmentsDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Enrollments,
                    opt => opt.MapFrom(src => src.Enrollments.Select(e => e.Course)));


            CreateMap<Core.Entities.Courses.Enrollment, StudentEnrollmentTableDto>()
                  .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                  //.ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.join))
                  .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
                  .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Student.User.FirstName + " " + src.Student.User.LastName))
                  //.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Student.User.Gender))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Student.User.PhoneNumber));
                 



        }
    }
}
