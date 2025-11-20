namespace Application.Features.Instructors.DTOs;

public class CreateInstructorDto
{
    public string FirstName { get; set; } //= default!;
    public string LastName { get; set; } //= default!;
    public string Email { get; set; } //= default!;
}

public class InstructorDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Title { get; set; } = "Unknown";
}