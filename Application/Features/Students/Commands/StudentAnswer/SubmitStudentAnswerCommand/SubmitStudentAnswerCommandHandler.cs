using Core.Common.Results;
using Infrastructure.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.StudentAnswer.SubmitStudentAnswerCommand
{
    public class SubmitStudentAnswerCommandHandler
        : IRequestHandler<SubmitStudentAnswerCommand, Result<Guid>>
    {
        private readonly IStudentAnswerRepository _studentAnswerRepo;
        private readonly IAnswerOptionRepository _answerOptionRepo;
        private readonly IExamResultRepository _examResultRepo;
        private readonly HybridCache _cache;

        public SubmitStudentAnswerCommandHandler(
            IStudentAnswerRepository studentAnswerRepo,
            IAnswerOptionRepository answerOptionRepo,
            IExamResultRepository examResultRepo,
            HybridCache cache)
        {
            _studentAnswerRepo = studentAnswerRepo;
            _answerOptionRepo = answerOptionRepo;
            _examResultRepo = examResultRepo;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(SubmitStudentAnswerCommand request, CancellationToken ct)
        {

            if (request.Answers == null || !request.Answers.Any())
                return Result<Guid>.FromError(Error.Validation("Answers.Empty", "Answers list cannot be empty."));

            var examResult = await _examResultRepo.GetByIdAsync(request.ExamResultId);
            if (examResult == null)
                return Result<Guid>.FromError(Error.NotFound("ExamResult.NotFound",
                    "Exam result not found."));

            
            var processedAnswers = new List<Guid>();

            foreach (var answer in request.Answers)
            {
               
                var option = await _answerOptionRepo.GetByIdAsync(answer.SelectedAnswerId);
                if (option == null || option.QuestionId != answer.QuestionId)
                    return Result<Guid>.FromError(Error.Validation("InvalidOption",
                        $"Invalid selected option for question {answer.QuestionId}"));

             
                var studentAnswerResult = Core.Entities.Students.StudentAnswer.Create(
                    request.ExamResultId,
                    answer.QuestionId,
                    answer.SelectedAnswerId);

                if (studentAnswerResult.IsError)
                    return Result<Guid>.FromError(studentAnswerResult.TopError);

                await _studentAnswerRepo.AddAsync(studentAnswerResult.Value);
                processedAnswers.Add(studentAnswerResult.Value.Id);

               
                examResult.EvaluateAnswer(option.IsCorrect);
            }

            
             _examResultRepo.Update(examResult);

          
            await _cache.RemoveAsync($"ExamResult_{request.ExamResultId}_Answers", ct);

            return Result<Guid>.FromValue(processedAnswers.First());
        }

    }
}

