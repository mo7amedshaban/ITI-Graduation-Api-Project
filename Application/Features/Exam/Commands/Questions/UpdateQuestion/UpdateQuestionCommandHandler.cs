using Application.Features.Exam.DTOs;
using Ardalis.Result;
using AutoMapper;
using Core.Interfaces;
using Core.Interfaces.Services;
using MediatR;

namespace Application.Features.Exam.Commands.Questions.UpdateQuestion;

public class UpdateQuestionCommandHandler
    : IRequestHandler<UpdateQuestionCommand, Result<QuestionDto>>
{
    private readonly IFileStorageService _fileStorage;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    public async Task<Result<QuestionDto>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var question = await _unitOfWork.Questions.GetByIdAsync(dto.Id);
        if (question == null)
            return Result.NotFound("Question not found");

        // 2) Handle image update
        if (dto.Image != null)
        {
            // delete old file
            if (!string.IsNullOrEmpty(question.ImageUrl))
                await _fileStorage.DeleteFileAsync(question.ImageUrl);

            // upload new file
            var newImageUrl = await _fileStorage.UploadFileAsync(
                dto.Image.FileName,
                dto.Image.OpenReadStream(),
                "Questions"
            );

            dto.ImageUrl = newImageUrl;
        }
        else
        {
            // keep old image
            dto.ImageUrl = question.ImageUrl;
        }

        // 3) Map updates
        _mapper.Map(dto, question);

        // 4) Save
        _unitOfWork.Questions.Update(question);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // 5) Return updated dto
        var updatedDto = _mapper.Map<QuestionDto>(question);
        return Result.Success(updatedDto);
    }
}