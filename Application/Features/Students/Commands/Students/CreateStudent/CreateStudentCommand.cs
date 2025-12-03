
using Application.Features.Students.DTOs;
using Core.Common.Results;

using MediatR;
using System;

namespace Application.Features.Students.Commands.Students.CreateStudent
{
    public record CreateStudentCommand(
      
       string gender
    ) : IRequest<Result<StudentDto>>;
    
}
