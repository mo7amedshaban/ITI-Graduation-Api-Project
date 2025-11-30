
using Application.Features.Students.DTOs;
using Core.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.StudentAnswer.SubmitStudentAnswerCommand
{
    public class SubmitStudentAnswerCommand : IRequest<Result<Guid>>
    {
        [JsonPropertyName("examResultId")]
        public Guid ExamResultId { get; set; }

        [JsonPropertyName("answers")]
        public List<StudentAnswerDto> Answers { get; set; } = new();
       
    }
}
