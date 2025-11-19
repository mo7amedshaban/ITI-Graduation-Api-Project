namespace Application.Features.Courses.DTOs;

public class CourseDto
{
 
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string TypeStatus { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal Price { get; set; }
        public Guid InstructorId { get; set; }
    

    public class CreateCourseDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string TypeStatus { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal Price { get; set; }
        public Guid InstructorId { get; set; }
    }

    public class UpdateCourseDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal Price { get; set; }
    }
}