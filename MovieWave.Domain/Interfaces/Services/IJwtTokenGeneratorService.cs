using MovieWave.Domain.Entity;

namespace MovieWave.Domain.Interfaces.Services;

public interface IJwtTokenGeneratorService
{
    string GenerateToken(User user);
}
