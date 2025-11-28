using Application.Features.Authantication.Admin.Command.Login;
using Application.Features.Authantication.Admin.Command.Register;
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
    [Route("api/admin")]
    [ApiController]
    public class Auth_AdminController : ControllerBase
    {
        private readonly ISender _sender;

        public Auth_AdminController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        [TranslateResultToActionResult]
        public async Task<Result<AuthResult>> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterAdminCommand(request);

            return await _sender.Send(command);
        }

        [HttpPost("login")]
        [TranslateResultToActionResult]
        public async Task<Result<AuthResult>> Login([FromBody] LoginRequest request)
        {
            var command = new LoginAdminCommand(request);
            return await _sender.Send(command);
        }
    }
}
