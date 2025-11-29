namespace Application.Features.Certificate.DTOs;

public class CertificateDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string CourseName { get; set; }
    public DateTime IssuedAt { get; set; }
    public string CertificateNumber { get; set; }
}