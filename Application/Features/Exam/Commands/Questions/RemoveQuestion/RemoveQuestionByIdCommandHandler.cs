using Ardalis.Result;
using Core.Interfaces;
using Core.Interfaces.Services;
using MediatR;

namespace Application.Features.Exam.Commands.Questions.RemoveQuestion;

public class RemoveQuestionByIdCommandHandler : IRequestHandler<RemoveQuestionByIdCommand, Result<bool>>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveQuestionByIdCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<bool>> Handle(RemoveQuestionByIdCommand request, CancellationToken cancellationToken)
    {
        var question = _unitOfWork.Questions.GetByIdAsync(request.quesitonId);
        if (question == null) return Result<bool>.NotFound("Question not found");
        _unitOfWork.Questions.Delete(question.Result);

        await _unitOfWork.CompleteAsync(cancellationToken);
        if (!string.IsNullOrEmpty(question.Result.ImageUrl))
            await _fileStorageService.DeleteFileAsync(question.Result.ImageUrl);
        return Result.Success(true, "Question removed successfully");
    }
}