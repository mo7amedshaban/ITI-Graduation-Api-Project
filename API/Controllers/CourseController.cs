using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Queries.GetCourseByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly IMediator _mediator;

    public CourseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    // [Route("GetCourseById")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CreateCourseCommand commandourse)
    {
        var result = await _mediator.Send(commandourse);
        return Ok(result);
    }
}