
using Application.Features.Courses.DTOs;
using Application.Features.Students.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Enrollments.Dto
{
    public class EnrollmentDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public DateTimeOffset EnrollmentDate { get; set; }
    }

    public class EnrollmentDetailsDto : EnrollmentDto
    {
     
        public string Status { get; set; } = string.Empty;
       
        public StudentDto? Student { get; set; }

        public CourseDto? Course { get; set; }


    }
}
