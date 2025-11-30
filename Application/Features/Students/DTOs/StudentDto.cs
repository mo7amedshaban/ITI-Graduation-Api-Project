using Application.Features.Courses.DTOs;

namespace Application.Features.Students.DTOs;

public class StudentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Gender { get; set; }


    // From User (Auth Service)
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

public class CreateStudentDto
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
}

public class StudentWithEnrollmentsDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }


    public string Gender { get; set; } = string.Empty;

    // User data
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string UserName { get; set; } = string.Empty;

    // Enrollments
    public List<CourseDto> Enrollments { get; set; } = new();
}

public class StudentAnswerDto
{
    public Guid QuestionId { get; set; }
    public Guid SelectedAnswerId { get; set; }
}

//public class CreateStudentDto
//{

//    public string FirstName { get; private set; } = default!;
//    public string LastName { get; private set; } = default!;
//    public string Email { get; private set; } = default!;
//}

//public class StudentWithEnrollmentsDto
//{
//    public Guid Id { get; set; }
//    public Guid UserId { get; set; }


//    public string Gender { get; set; } = string.Empty;

//    // User data
//    public string Email { get; set; } = string.Empty;
//    public string FirstName { get; set; } = string.Empty;
//    public string LastName { get; set; } = string.Empty;
//    public string? PhoneNumber { get; set; }
//    public string UserName { get; set; } = string.Empty;

//    // Enrollments
//    public List<CourseDto> Enrollments { get; set; } = new();
//}

public class StudentEnrollmentTableDto
{
    public Guid StudentId { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CourseTitle { get; set; }
    public DateTime? JoinDate { get; set; }
}


public class StudentCourseLecturesDto
{
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; }
    public List<ModuleLecturesDto> Modules { get; set; }
}


public class ModuleLecturesDto
{
    public Guid ModuleId { get; set; }
    public string ModuleTitle { get; set; }
    public List<Application.Features.Lectures.Dtos.LectureDto> Lectures { get; set; }
}