using Application.Features.Authantication.Dtos;
using Application.Features.Authantication.Instructor.Command.Login;
using Application.Features.Authantication.Instructor.Command.Register;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth_InstructorController : ControllerBase
    {

        private readonly ISender _sender;

        public Auth_InstructorController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost("register-Instructor")]
        [TranslateResultToActionResult]
        public async Task<Result<AuthResult>> RegisterInstructor([FromBody] RegisterRequest request)
        {
            var command = new RegisterInstructorCommand(request);

            return await _sender.Send(command);
        }

        [HttpPost("login-Instructor")]
        [TranslateResultToActionResult]
        public async Task<Result<AuthResult>> LoginInstructor([FromBody] LoginRequest request)
        {
            var command = new LoginInstructorCommand(request);
            return await _sender.Send(command);
        }
    }
}
