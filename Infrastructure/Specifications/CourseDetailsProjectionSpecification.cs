using Domain.Common.Specifications;
using Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Specifications
{
    //public class CourseDetailsProjectionSpecification : BaseSpecification<Course, CourseDetailsDto>
    //{
    //    public CourseDetailsProjectionSpecification(Guid courseId)
    //    {
    //        Query
    //            .Where(c => c.Id == courseId)
    //            .Select(c => new CourseDetailsDto
    //            {
    //                Id = c.Id,
    //                Title = c.Title,
    //                Description = c.Description,
    //                TypeStatus = c.TypeStatus,
    //                ImageUrl = c.ImageUrl,
    //                ContentType = c.ContentType,
    //                CourseDetails = c.CourseDetails,
    //                StartDate = c.StartDate,
    //                EndDate = c.EndDate,
    //                Price = c.Price,
    //                InstructorId = c.InstructorId,

    //                // Instructor extra values✔
    //                InstructorImageUrl = c.Instructor.ImageUrl,
    //                InstructorDescription = c.Instructor.Description,

    //                // Modules ✔
    //                Modules = c.Modules
    //                    .OrderBy(m => m.Order)
    //                    .Select(m => new ModuleDto
    //                    {
    //                        Id = m.Id,
    //                        Title = m.Title,

    //                        // Lectures ✔
    //                        Lectures = m.Lectures
    //                            .OrderBy(l => l.ScheduledAt)
    //                            .Select(l => new LectureDto
    //                            {
    //                                Id = l.Id,
    //                                LectureTitle = l.Title,
    //                                ScheduledAt = l.ScheduledAt.UtcDateTime,
    //                                Duration = l.Duration
    //                            })
    //                            .ToList()
    //                    })
    //                    .ToList()
    //            });
    //    }
    //}

}
