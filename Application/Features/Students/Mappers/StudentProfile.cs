using Application.Common.AuthDto;
using Application.Features.Course.Dtos;
using Application.Features.Enrollments.Dto;
using Application.Features.Students.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Courses;
using Domain.Entities.Students;


namespace Application.Features.Students.Mappers
{
    public class StudentProfile : Profile
    {

        public StudentProfile()
        {


            // CreateMap<Domain.Entities.Students.Student, CreateStudentDto>().ReverseMap();

            //CreateMap<Student, StudentDto>()
            //     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            //     .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            //     .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            //     .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            //     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));


            //CreateMap<Enrollment, EnrollmentDto>();
            //// Map Course -> CourseDto
            //CreateMap<Domain.Entities.Courses.Course, CourseDto>();

            //// Map Student -> StudentWithEnrollmentsDto, projecting enrollments to courses
            //CreateMap<Student, StudentWithEnrollmentsDto>()
            //    // if Student.Enrollments is IEnumerable<Enrollment>
            //    .ForMember(dest => dest.Enrollments,
            //    opt => opt.MapFrom(src => src.Enrollments.Select(e => e.Course)));


            //CreateMap<Student, StudentWithEnrollmentsDto>()
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            //    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            //    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            //    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Domain.Entities.Students.Student, CreateStudentDto>().ReverseMap();

            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Enrollment, EnrollmentDto>();
            CreateMap<Domain.Entities.Courses.Course, CourseDto>();

            CreateMap<Student, StudentWithEnrollmentsDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Enrollments,
                    opt => opt.MapFrom(src => src.Enrollments.Select(e => e.Course)));


            CreateMap<Enrollment, StudentEnrollmentTableDto>()
                  .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                  //.ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.join))
                  .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
                  .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Student.User.FirstName + " " + src.Student.User.LastName))
                  //.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Student.User.Gender))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Student.User.PhoneNumber));
                 



        }
    }
}
