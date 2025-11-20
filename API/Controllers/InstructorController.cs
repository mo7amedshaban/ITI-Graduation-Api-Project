using Application.Features.Instructors.Command.CreateInstructor;
using Application.Features.Instructors.Command.RemoveInstructor;
using Application.Features.Instructors.Command.UpdateInstructor;
using Application.Features.Instructors.DTOs;
using Application.Features.Instructors.Queries.GetInstructorById;
using Application.Features.Instructors.Queries.GetInstructors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstructorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateInstructor")]
    public async Task<IActionResult> CreateInstructor([FromBody] CreateInstructorDto dto)
    {
        var command = new CreateInstructorCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("RemoveInstructor/{id}")]
    public async Task<IActionResult> RemoveInstructor(Guid id)
    {
        var command = new RemoveInstructorCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("UpdateInstructor")]
    public async Task<IActionResult> UpdateInstructor([FromBody] InstructorDto dto)
    {
        var command = new UpdateInstructorCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("GetInstructorById/{id}")]
    public async Task<IActionResult> GetInstructorById(Guid id)
    {
        var query = new GetInstructorByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetInstructors")]
    public async Task<IActionResult> GetInstructors()
    {
        var query = new GetInstructorsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}