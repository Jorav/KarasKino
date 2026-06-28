using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KarasKino.Core.Authentication;
using KarasKino.Core.Interfaces;
using KarasKino.Core.Users;
using Microsoft.IdentityModel.Tokens;

namespace KarasKino.Infrastructure.Authentication;

public class JwtService(IOptions<JwtConfiguration> config) : IJwtService
{
  private readonly JwtConfiguration _config = config.Value;

  public string GenerateToken(User user)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(ClaimTypes.Role, user.Role.ToString()),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
      issuer: _config.Issuer,
      audience: _config.Audience,
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(_config.ExpiryMinutes),
      signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
