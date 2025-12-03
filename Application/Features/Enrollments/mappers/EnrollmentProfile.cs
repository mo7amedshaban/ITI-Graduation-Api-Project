using Application.Features.Enrollments.Dto;
using AutoMapper;
using Core.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.mappers
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile() { 
        
         CreateMap<Enrollment , EnrollmentDto>().ReverseMap();

            CreateMap<Enrollment, EnrollmentDetailsDto>().ReverseMap();
         


           
        }

    }
}
