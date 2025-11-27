using Application.Features.Exam.DTOs;
using Ardalis.Result;
using AutoMapper;
using Core.Entities.Exams;
using Core.Interfaces;
using MediatR;

namespace Application.Features.Exam.Queries;

public class GetAllQuestionQueryHandler : IRequestHandler<GetAllQuestionQuery, Result<List<QuestionDto>>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Question> _questions;

    public GetAllQuestionQueryHandler(IGenericRepository<Question> Questions, IMapper mapper)
    {
        _mapper = mapper;
        _questions = Questions;
    }

    public async Task<Result<List<QuestionDto>>> Handle(GetAllQuestionQuery request,
        CancellationToken cancellationToken)
    {
        if (request == null)
            return Result.Error("Question not found");
        var questions = await _questions.FindAllAsync(q => q.AnswerOptions != null,
            new[] { "AnswerOptions" });

        var questionDtos = _mapper.Map<List<QuestionDto>>(questions);
        return Result.Success(questionDtos);
    }
}