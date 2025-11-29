using Application.Features.Certificate.Command;
using Application.Features.Certificate.DTOs;
using Core.Interfaces;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public CertificatesController(IMediator mediator, IUnitOfWork unitOfWork, IUserContextService userContext)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    [HttpPost("issue")]
    public async Task<IActionResult> Issue([FromBody] IssueCertificateCommand command)
    {
        var certificate = await _mediator.Send(command);
        return Ok(certificate);
    }

    [HttpGet("View Certificate")]
    public async Task<IActionResult> GetMyCertificates()
    {
        var query = new Application.Features.Certificate.Query.ViewMyCertificateQuery();
        var certificates = await _mediator.Send(query);
        return Ok(certificates);
    }
}