using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ParkingManagement.Application.Services;

namespace ParkingManagement.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Guid userId, string username, string role, Guid? businessOperatorId, string fullName)
    {
        var secretKey = _configuration["Jwt:Key"] ?? "SuperSecretKeyForParkingSystem_1234567890!@#$%^&*()";
        var issuer = _configuration["Jwt:Issuer"] ?? "ParkingManagement_Api";
        var audience = _configuration["Jwt:Audience"] ?? "ParkingManagement_Client";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim("FullName", fullName)
        };

        if (businessOperatorId.HasValue)
        {
            claims.Add(new Claim("BusinessOperatorId", businessOperatorId.Value.ToString()));
            claims.Add(new Claim("OperatorId", businessOperatorId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
