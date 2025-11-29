using Application.Features.Certificate.DTOs;
using MediatR;

namespace Application.Features.Certificate.Command;

public record IssueCertificateCommand(Guid UserId, Guid CourseId) : IRequest<CertificateDto>;