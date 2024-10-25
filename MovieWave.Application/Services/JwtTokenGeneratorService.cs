using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenGeneratorService : IJwtTokenGeneratorService
{
	private readonly JwtSettings _jwtSettings;

	public JwtTokenGeneratorService(IOptions<JwtSettings> jwtSettings)
	{
		_jwtSettings = jwtSettings.Value;
	}

	public string GenerateToken(User user)
	{
		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.UserRole.ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Lifetime),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}