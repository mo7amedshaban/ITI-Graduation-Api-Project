using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Commands.RemoveCourse;
using Application.Features.Courses.Commands.UpdateCourse;
using Application.Features.Courses.DTOs;
using Application.Features.Courses.Queries.GetAllCourses;
using Application.Features.Courses.Queries.GetCourseByIdQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;


    public CourseController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("GetCourseById/{id}", Name = "GetCourseById")]
    public async Task<IActionResult> GetCourseById(Guid id)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateCourse(CreateCourseCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("Update/{id:guid}", Name = "UpdateCourse")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CourseDto.UpdateCourseDto dto)
    {
        var command = new UpdateCourseCommand(dto);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("Delete/{id:guid}", Name = "DeleteCourse")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var command = new RemoveCourseCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("GetAllCourses", Name = "GetAllCourses")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetAllCourses()
    {
        var query = new GetAllCoursesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}