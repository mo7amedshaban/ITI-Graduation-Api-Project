using Application.Features.Exam.DTOs;
using AutoMapper;
using Core.Entities.Exams;

namespace Application.Features.Exam.Mappers;

public class QuestionsProfile : Profile
{
    public QuestionsProfile()
    {
        CreateMap<CreateQuestionRequestDto, Question>();
        CreateMap<AnswerOptionDto, AnswerOption>();

        CreateMap<Question, QuestionDto>();
        CreateMap<AnswerOption, AnswerOptionDto>();
        CreateMap<UpdateQuestionRequestDto, Question>();
    }
}