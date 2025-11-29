using Application.Features.Certificate.DTOs;
using Ardalis.Result;
using Core.Interfaces;
using Core.Interfaces.Services;
using MediatR;

namespace Application.Features.Certificate.Command;

public class IssueCertificateCommandHandler : IRequestHandler<IssueCertificateCommand, CertificateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public IssueCertificateCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<CertificateDto> Handle(IssueCertificateCommand request, CancellationToken cancellationToken)
    {
        var role = _userContext.GetRole();
        var currentUserId = _userContext.GetUserId();

        if (role == "Student")
            throw new UnauthorizedAccessException("Students cannot issue certificates.");

        if (role == "Instructor")
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null )
                throw new UnauthorizedAccessException("Instructor cannot issue this certificate.");
        }
        if (role == "Admin")
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                throw new UnauthorizedAccessException("Course not found for Admin.");
    
        }

        var existing = await _unitOfWork.Certificates.FindAsync(
            c => c.UserId == request.UserId && c.CourseId == request.CourseId,
            new string[] { "User", "Course" });

        if (existing != null)
        {
            return new CertificateDto
            {
                Id = existing.Id,
                UserName = existing.User.UserName,
                CourseName = existing.Course.Title,
                IssuedAt = existing.IssuedAt,
                CertificateNumber = existing.CertificateNumber
            };
        }
        // create rtificate
        var certificate = new Core.Entities.Certificate
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            IssuedAt = DateTime.UtcNow,
            CertificateNumber = Guid.NewGuid().ToString()
        };

        await _unitOfWork.Certificates.AddAsync(certificate);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // جلب البيانات مع Navigation Properties
        var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(request.UserId);
        if (user == null)
            throw new Exception($"User with ID {request.UserId} does not exist.");
        var courseInfo = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);

        return new CertificateDto
        {
            Id = certificate.Id,
            UserName = user.UserName,
            CourseName = courseInfo.Title,
            IssuedAt = certificate.IssuedAt,
            CertificateNumber = certificate.CertificateNumber
        };
    }
}
