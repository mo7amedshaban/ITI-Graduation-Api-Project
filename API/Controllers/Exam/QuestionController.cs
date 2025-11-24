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

    public QuestionController(ILogger<QuestionController> logger, ISender sender, IWebHostEnvironment env,
        IFileStorageService fileStorageService, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _sender = sender;
        _fileStorageService = fileStorageService;
        _env = env;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("CreateQuestion")]
    [TranslateResultToActionResult]
    public async Task<Result<QuestionDto>> CreateQuestion([FromForm] CreateQuestionCommand command,
        IFormFile? image)
    {
        if (command == null)
            return Result.NotFound("Command cannot be null");
        if (image != null)
            command.questionDto.ImageUrl =
                await _fileStorageService.UploadFileAsync(image.FileName, image.OpenReadStream(), "Questions");
        _logger.LogInformation("Uploaded image saved: {ImageUrl}", command.questionDto.ImageUrl);
        return await _sender.Send(command);
    }

    [HttpDelete("RemoveQuestion/{questionId}")]
    [TranslateResultToActionResult]
    public async Task<Result<bool>> RemoveQuestion(Guid questionId)
    {
        return await _sender.Send(new RemoveQuestionByIdCommand(questionId));
    }

    [HttpPut("UpdateQuestion")]
    [TranslateResultToActionResult]
    public async Task<Result<QuestionDto>> UpdateQuestion([FromForm] QuestionDto dto, IFormFile? image)
    {
        if (dto == null)
            return Result.NotFound("Question Data cannot be null");
        if (image != null)
        {
            _logger.LogInformation("Update called with image: {FileName}", image.FileName);

            // If DTO doesn't contain the existing ImageUrl, fetch it from DB so DeleteFileAsync can remove the old file
            var existingPath = dto.ImageUrl;
            if (string.IsNullOrEmpty(existingPath))
                try
                {
                    var q = await _unitOfWork.Questions.GetByIdAsync(dto.Id);
                    existingPath = q?.ImageUrl;
                    _logger.LogInformation("Fetched existing ImageUrl from DB: {ExistingPath}", existingPath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to fetch existing question to determine old ImageUrl");
                }

            dto.ImageUrl =
                await _fileStorageService.UpdateFileAsync(existingPath, image.FileName, image.OpenReadStream(),
                    "Questions");
            _logger.LogInformation("FileStorage returned ImageUrl: {ImageUrl}", dto.ImageUrl);
        }
        else
        {
            _logger.LogInformation("Update called without an image. Keeping existing ImageUrl: {ImageUrl}",
                dto.ImageUrl);
        }

        _logger.LogInformation("Uploaded image saved: {ImageUrl}", dto.ImageUrl);


        return await _sender.Send(new UpdateQuestionCommand(dto));
    }

    [HttpGet("GetAllQuestions")]
    [TranslateResultToActionResult]
    public async Task<Result<List<QuestionDto>>> GetAllQuestions()
    {
        return await _sender.Send(new GetAllQuestionQuery());
    }
}