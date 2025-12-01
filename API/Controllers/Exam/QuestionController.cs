using Application.Features.Exam.Commands.Questions.CreateQuestion;
using Application.Features.Exam.Commands.Questions.RemoveQuestion;
using Application.Features.Exam.Commands.Questions.UpdateQuestion;
using Application.Features.Exam.DTOs;
using Application.Features.Exam.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Core.Interfaces;
using Core.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Exam;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<QuestionController> _logger;
    private readonly ISender _sender;
    private readonly IUnitOfWork _unitOfWork;

    public QuestionController(ILogger<QuestionController> logger, ISender sender, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _sender = sender;
        _unitOfWork = unitOfWork;
    }


    [HttpPost("CreateQuestion")]
    [TranslateResultToActionResult]
    public async Task<Result<QuestionDto>> CreateQuestion([FromForm] CreateQuestionRequestDto request)
    {
        if (request == null)
            return Result.NotFound("Question Data cannot be null");

        return await _sender.Send(new CreateQuestionCommand(request));
    }


    [HttpDelete("RemoveQuestion/{questionId}")]
    [TranslateResultToActionResult]
    public async Task<Result<bool>> RemoveQuestion(Guid questionId)
    {
        return await _sender.Send(new RemoveQuestionByIdCommand(questionId));
    }

    [HttpPut("UpdateQuestion")]
    [TranslateResultToActionResult]
    public async Task<Result<QuestionDto>> UpdateQuestion([FromForm] UpdateQuestionRequestDto? dto)
    {
        if (dto == null)
            return Result.NotFound("Question Data cannot be null");

        return await _sender.Send(new UpdateQuestionCommand(dto));
    }


    [HttpGet("GetAllQuestions")]
    [TranslateResultToActionResult]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<Result<List<QuestionDto>>> GetAllQuestions()
    {
        return await _sender.Send(new GetAllQuestionQuery());
    }
}