using Infrastructure.Interfaces;
using Infrastructure.Helper;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;


public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<TokenService> _logger;


    public TokenService(JwtSettings jwtSettings, IConfiguration configuration,
         IRefreshTokenRepository refreshTokenRepository, 
         UserManager<ApplicationUser> userManager, ILogger<TokenService> logger)
    {
        _jwtSettings = jwtSettings;
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
        _logger = logger;
    }


    public async Task<AuthResult> GenerateTokenAsync(ApplicationUser user)
    {
        try
        {
        // handling formatting
        var tokenHandler = new JwtSecurityTokenHandler();

        var AccessTokenExpirationMinutes = _jwtSettings.AccessTokenExpiration;
        var expiration = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);
        var Role = await _userManager.GetRolesAsync(user);
        


          var claims = new List<Claim>
          {  
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // unique identifier for the token
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Iat, // Token issue time
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
                new Claim("exp", new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString())
          
          };
   
         // Add user roles to claims
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Create token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                         SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
       

        return new AuthResult
        {
            Token = jwtToken,
            ExpiresAt = expiration,
            RefreshToken = "",
            IsSuccess = true
        };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating token: {ex.Message}");
            return new AuthResult
            {
                IsSuccess = false,
                Errors = new List<string> { "Error generating token" }
            };
        }

    }

    public Task<AuthResult> GenerateRefreshTokenAsync()
    {
        try
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var newRefreshToken = Convert.ToBase64String(randomNumber);

          
                return Task.FromResult(new AuthResult
                {
                    RefreshToken = newRefreshToken,
                    IsSuccess = true
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating refresh token: {ex.Message}");
            return Task.FromResult(new AuthResult
            {
                IsSuccess = false,
                Errors = new List<string> { "Error generating refresh token" }
            });
        }
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new ArgumentNullException(nameof(refreshToken));

        
        var storedToken = await _refreshTokenRepository.GetTokenAsync(refreshToken);
        if (storedToken == null)
            return false;

        await _refreshTokenRepository.RevokeRefreshTokenAsync(storedToken.UserId, refreshToken);

        return true;
    }

    public async Task<ApplicationUser> GetUserFromTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler();

        // Read token without validating signature
        var jwtToken = handler.ReadJwtToken(token);
        if (jwtToken == null)
            return null;

        // Extract
        var userId = jwtToken.Claims.FirstOrDefault(c =>
            c.Type == JwtRegisteredClaimNames.Sub ||
            c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return null;

       
        var user = await _userManager.FindByIdAsync(userId);

        return user;
    }

}