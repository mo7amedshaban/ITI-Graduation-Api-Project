using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authantication.Dtos
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserDto? User { get; set; }

        public static AuthResult Success(string token, string refreshToken,
            UserDto user)
        {
            return new AuthResult
            {
                IsSuccess = true,
                AccessToken = token,
                RefreshToken = refreshToken,
                User = user,
             
                Message = "Success"
            };
        }

        public static AuthResult Fail(string message)
        {
            return new AuthResult
            {
                IsSuccess = false,
                Message = message
            };
        }
    }

    public class UserDto
    {
        public Guid Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public record LoginRequest(string Email, string Password);

    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }


    }
    public class AdminCreateUserRequest : RegisterRequest
    {
        public string Role { get; set; }
    }
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
