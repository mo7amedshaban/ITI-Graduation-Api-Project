using Application.Features.Certificate.DTOs;
using MediatR;

namespace Application.Features.Certificate.Query;

public record ViewMyCertificateQuery : IRequest<List<CertificateDto>>;
