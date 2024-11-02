using System.Security.Claims;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface ITokenGeneratorService
{
	string GenerateAccessToken(IEnumerable<Claim> claims);

	string GenerateRefreshToken();

	ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);

	Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);
}
