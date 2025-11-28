using Application.Features.Authantication.Command.Models;
using Application.Features.Authantication.Dtos;
using Application.Features.Authantication.Instructor.Command.Login;
using Application.Features.Authantication.Instructor.Command.Register;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("register-student")]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterRequest request)
    {
        var command = new RegisterStudentCommand(request);
        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(
            authResult => Ok(authResult),
            errors => BadRequest(errors.Select(e => e.Description ?? "Unknown error").ToList())
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request);
        var result = await _mediator.Send(command);
        return result.Match<IActionResult>(
            authResult => Ok(authResult),
            errors => BadRequest(errors.Select(e => e.Description ?? "Unknown error").ToList())
        );
    }


  
}